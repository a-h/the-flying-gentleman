using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using FlyingGentleman.Library.WebServer;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;
using Microsoft.Web.Administration;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net;

namespace FlyingGentleman.RemoteServerLibrary.WebServer
{
	/// <summary>
	/// Allows the setup and configuration of IIS 7.0.
	/// </summary>
	[Serializable]
	public class WebServer : LogEventCreatorBase, IWebServer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WebServer"/> class.
		/// </summary>
		public WebServer()
		{
		}

		/// <summary>
		/// Installs this instance.
		/// </summary>
		public void Install()
		{
			LogEventHelper.CreateEvent(this, "Install", "Installing IIS.");

			// See http://forums.iis.net/t/1148892.aspx
			// Also http://technet.microsoft.com/en-us/library/cc722041%28WS.10%29.aspx
			var commandLine = new CommandLine.CommandLine();
			string[] packageNames = new string[]
            {
				"IIS-WebServerRole", 
				"IIS-WebServer",
 				"IIS-CommonHttpFeatures",
				"IIS-DefaultDocument",
				"IIS-HttpErrors",
				"IIS-HttpRedirect",
				"IIS-StaticContent",
				"IIS-HealthAndDiagnostics",
				"IIS-HttpLogging",
				"IIS-RequestMonitor",
				"IIS-Performance",
				"IIS-HttpCompressionDynamic",
				"IIS-HttpCompressionStatic",
				"IIS-Security",
				"IIS-BasicAuthentication",
				"IIS-DigestAuthentication",
				"IIS-IPSecurity",
				"IIS-URLAuthorization",
				"IIS-WindowsAuthentication",
				"IIS-WebServerManagementTools",
				"IIS-ManagementConsole",
				"IIS-ManagementService",
                "IIS-ManagementScriptingTools"
            };

			string joinedList = string.Join(";", packageNames);

			commandLine.ExecuteCommandLine(string.Format("pkgmgr /iu:{0}", joinedList), new TimeSpan(0, 2, 0));

            commandLine.ExecuteCommandLine(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -i", new TimeSpan(0, 2, 0));
		}


        /// <summary>
        /// Installs support for hosting WCF service applications
        /// </summary>
        public void InstallWcfSupport()
        {
            LogEventHelper.CreateEvent(this, "Install", "Installing WCF support.");

            var commandLine = new CommandLine.CommandLine();
            commandLine.ExecuteCommandLine(@"ServerManagerCmd.exe -install Application-Server", new TimeSpan(0, 10, 0), new[] { 1003 });
            commandLine.ExecuteCommandLine(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\ServiceModelReg.exe -ia -q", new TimeSpan(0, 10, 0));
            commandLine.ExecuteCommandLine("pkgmgr /iu:IIS-NetFxExtensibility", new TimeSpan(0, 10, 0));
            commandLine.ExecuteCommandLine("pkgmgr /iu:WAS-ProcessModel", new TimeSpan(0, 10, 0));
            commandLine.ExecuteCommandLine("pkgmgr /iu:WCF-HTTP-Activation", new TimeSpan(0, 10, 0));
        }

        /// <summary>
        /// Installs MVC3 on the server
        /// </summary>
        public void InstallMvc3()
        {
            LogEventHelper.CreateEvent(this, "InstallMvc3", "Installing ASP.NET MVC3.");

            if (Directory.Exists(@"C:\Program Files (x86)\Microsoft ASP.NET\ASP.NET MVC 3"))
            {
                LogEventHelper.CreateEvent(this, "InstallMvc3", "ASP.NET MVC3 appears to be already installed");
            }
            else
            {
                //install MVC3
                var mvcInstallerUrl = @"http://download.microsoft.com/download/F/3/1/F31EF055-3C46-4E35-AB7B-3261A303A3B6/AspNetMVC3ToolsUpdateSetup.exe";
                var downloadFolder = @"C:\temp\mvc3Installer\";
                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }
                var installerPath = Path.Combine(downloadFolder, "installer.exe");
                
                var client = new WebClient();
                client.DownloadFile(mvcInstallerUrl, installerPath);
                LogEventHelper.CreateEvent(this, "InstallMvc3", string.Format("Downloaded installer to {0}", installerPath));

                // Install MVC3.
                string installationCommandLine = string.Format(@"{0} /q", installerPath);
                var commandLineExecutor = new CommandLine.CommandLine();
                commandLineExecutor.ExecuteCommandLine(installationCommandLine, new TimeSpan(1, 0, 0));
                
                LogEventHelper.CreateEvent(this, "InstallMvc3", "Installed MVC3. Deleting installer directory");
                Directory.Delete(downloadFolder, true);
            }
        }

		/// <summary>
		/// Setups the website.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public void SetupWebsite(WebsiteSettings settings)
		{
			LogEventHelper.CreateEvent(this, "SetupWebsite", settings.ToJson());

			using (var iisManager = new ServerManager())
			{
				Site site = iisManager.Sites.SingleOrDefault(s => s.Name.Equals(settings.WebsiteName));

				if (site == null)
				{
					LogEventHelper.CreateEvent(this, "SetupWebsite", "Creating new Website.");
					site = iisManager.Sites.Add(settings.WebsiteName, settings.Path, settings.Port ?? 80);
				}
				else
				{
					LogEventHelper.CreateEvent(this, "SetupWebsite", "The Website already exists.");

					site.LogFile.Directory = settings.LogFileDirectory;

					// Setup Site Bindings.
					site.Bindings.Clear();
					site.Bindings.Add("*:80:", "http");
					site.Bindings.Add("*:443:", "https");

					// Setup SSL where appropriate.
					if (!string.IsNullOrEmpty(settings.SslThumbprint))
					{
						LogEventHelper.CreateEvent(this, "SetupWebsite", "Configuring SSL.");
						var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
						store.Open(OpenFlags.OpenExistingOnly);
						var certificate = store.Certificates.Find(X509FindType.FindByThumbprint, settings.SslThumbprint, true);
						site.Bindings.Add("*:443:", certificate[0].GetCertHash(), "MY");
					}
				}

				iisManager.CommitChanges();
				LogEventHelper.CreateEvent(this, "SetupWebsite", "Website Setup Complete.");
			}
		}

		/// <summary>
		/// Sets up the virtual directory.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public void SetupVirtualDirectory(VirtualDirectorySettings settings)
		{
			LogEventHelper.CreateEvent(this, "SetupVirtualDirectory", settings.ToJson());

			using (var iisManager = new ServerManager())
			{
				Site site = iisManager.Sites.SingleOrDefault(s => s.Name.Equals(settings.WebsiteName));

				if (site == null)
				{
					throw new ArgumentException(string.Format("A Website with name {0} was not found.", settings.WebsiteName), "settings.WebsiteName");
				}

				// The application path is the path to the virtual directory, e.g. /CapacityManagement or /Google/Management/
                var applicationPath = "/" + settings.Name.TrimStart('/').TrimEnd('/');

                // Setup the application if it doesn't already exist.

                // Search for the existing application.
				var application = site.Applications.FirstOrDefault(x => x.Path == applicationPath);

				if (application == null)
				{
                    // The application doesn't exist, so we need to create it.
					LogEventHelper.CreateEvent(this, "SetupVirtualDirectory", "Creating Virtual Directory.");
					application = site.Applications.Add(applicationPath, settings.Path);
					LogEventHelper.CreateEvent(this, "SetupVirtualDirectory", "Created Virtual Directory.");
				}
				else
				{
					LogEventHelper.CreateEvent(this, "SetupVirtualDirectory", "Virtual Directory already exists.");
				}

				// Check that the Application Pool exists.
				ApplicationPool pool = iisManager.ApplicationPools.FirstOrDefault(a => a.Name.Equals(settings.ApplicationPool));

				if (pool == null)
				{
                    // The application pool doesn't exist, so we need to throw an error.  The installation should
                    // have already made a call to SetupApplicationPool, or the name of the application pool 
                    // is incorrect.
					throw new ArgumentException(string.Format("An Application Pool named {0} could not be found.", settings.ApplicationPool), "settings.ApplicationPool");
				}

				// Assign the Application to the Application Pool.
				if (!application.ApplicationPoolName.Equals(settings.ApplicationPool, StringComparison.InvariantCultureIgnoreCase))
				{
					application.ApplicationPoolName = settings.ApplicationPool;
					LogEventHelper.CreateEvent(this, "SetupVirtualDirectory", string.Format("Changed Application Pool to {0}.", settings.ApplicationPool));
				}

                // If we created the Virtual Directory in a previous installation, the path may have changed.
                if (!application.VirtualDirectories["/"].PhysicalPath.Equals(settings.Path, StringComparison.InvariantCultureIgnoreCase))
                {
                    application.VirtualDirectories["/"].PhysicalPath = settings.Path;
                    LogEventHelper.CreateEvent(this, "SetupVirtualDirectory", string.Format("Changed physical path of Virtual Directory to {0}.", settings.Path));
                }

                // Configure authentication methods available to the web application.
                // Forms authenticatoion needs to be set in the web.config for the application:
                // As <authentication mode="Forms" /> for example.
                
                Configuration config = iisManager.GetApplicationHostConfiguration();
                ConfigurationSection anonymousAuthenticationSection =
                    config.GetSection("system.webServer/security/authentication/anonymousAuthentication", string.Format("{0}/{1}", settings.WebsiteName, settings.Name.TrimStart('/')));
                anonymousAuthenticationSection.OverrideMode = OverrideMode.Allow;
                anonymousAuthenticationSection["enabled"] = settings.AuthenticationAllowAnonymousAuthentication;

                ConfigurationSection windowsAuthenticationSection =
                    config.GetSection("system.webServer/security/authentication/windowsAuthentication", string.Format("{0}/{1}", settings.WebsiteName, settings.Name.TrimStart('/')));
                windowsAuthenticationSection.OverrideMode = OverrideMode.Allow;
                windowsAuthenticationSection["enabled"] = (settings.AuthenticationMethod == AuthenticationMethod.WindowsAuthentication);
                
                ConfigurationSection basicAuthenticationSection =
                    config.GetSection("system.webServer/security/authentication/basicAuthentication", string.Format("{0}/{1}", settings.WebsiteName, settings.Name.TrimStart('/')));
                basicAuthenticationSection.OverrideMode = OverrideMode.Allow;
                basicAuthenticationSection["enabled"] = (settings.AuthenticationMethod == AuthenticationMethod.BasicAuthentication
                                                            || settings.AuthenticationMethod == AuthenticationMethod.BasicAuthenticationAndWindowsAuthentication);

				iisManager.CommitChanges();
			}
		}

		/// <summary>
		/// Sets up an Application Pool within IIS.
		/// </summary>
		/// <param name="settings">The Application Pool settings to use.</param>
		public void SetupApplicationPool(ApplicationPoolSettings settings)
		{
			LogEventHelper.CreateEvent(this, "SetupAplicationPool", settings.ToJson());

			using (var iisManager = new ServerManager())
			{
				ApplicationPool pool = iisManager.ApplicationPools.SingleOrDefault(ap => ap.Name.Equals(settings.ApplicationPoolName, StringComparison.InvariantCultureIgnoreCase));

				// Create the application pool if it doesn't exist.
				if (pool == null)
				{
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Creating Application Pool \"{0}\"", settings.ApplicationPoolName));
					pool = iisManager.ApplicationPools.Add(settings.ApplicationPoolName);
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Created Application Pool \"{0}\"", settings.ApplicationPoolName));
				}

				// Ensure that its setup to run under the correct identity, e.g. a specific Windows user.
				ProcessModelIdentityType updatedIdentityType = Convert(settings.IdentityType);
				if (pool.ProcessModel.IdentityType != updatedIdentityType)
				{
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changing Identity Type to \"{0}\"", settings.IdentityType));
					pool.ProcessModel.IdentityType = updatedIdentityType;
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changed Identity Type to \"{0}\"", settings.IdentityType));
				}

				if (settings.IdentityType == IdentityType.SpecificUser)
				{
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Setting Identity of Application Pool \"{0}\" to \"{1}\"", settings.ApplicationPoolName, settings.WindowsUserName));
					pool.ProcessModel.UserName = settings.WindowsUserName;
					pool.ProcessModel.Password = settings.WindowsPassword;
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Set Identity of Application Pool \"{0}\" to \"{1}\"", settings.ApplicationPoolName, settings.WindowsUserName));
				}

				// Set the pipeline mode.
				ManagedPipelineMode updatedPipeLineMode = Convert(settings.PipelineMode);
				if (pool.ManagedPipelineMode != updatedPipeLineMode)
				{
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changing Managed Pipeline Mode to \"{0}\"", settings.PipelineMode));
					pool.ManagedPipelineMode = updatedPipeLineMode;
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changed Managed Pipeline Mode to \"{0}\"", settings.PipelineMode));
				}

				// Update the .net Runtime Version where required.
				string updatedRuntimeVersion = Convert(settings.Runtime);
				if (pool.ManagedRuntimeVersion != updatedRuntimeVersion)
				{
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changing .Net Runtime from \"{0}\" to \"{1}\"", pool.ManagedRuntimeVersion, settings.Runtime));
					pool.ManagedRuntimeVersion = updatedRuntimeVersion;
					LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changed .Net Runtime to \"{0}\"", pool.ManagedRuntimeVersion));
				}

                // Update the Idle Timeout of the Application Pool.
                TimeSpan updatedIdleTimeout = Convert(settings.IdleTimeout);
                if (pool.ProcessModel.IdleTimeout != updatedIdleTimeout)
                {
                    LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changing Idle Timeout from \"{0}\" to \"{1}\"", pool.ProcessModel.IdleTimeout, updatedIdleTimeout));
                    pool.ProcessModel.IdleTimeout = updatedIdleTimeout;
                    LogEventHelper.CreateEvent(this, "SetupAplicationPool", string.Format("Changed Idle Timeout to \"{0}\"", pool.ProcessModel.IdleTimeout));
                }

				iisManager.CommitChanges();
			}
		}

        /// <summary>
        /// Converts the Idle Timeout given in minutes into a TimeSpan.
        /// </summary>
        /// <param name="idleTime">The Flying Gentleman value.</param>
        /// <returns>A Microsoft Web Administration runtime value.</returns>
        private TimeSpan Convert(int idleTime)
        {
            return new TimeSpan(0, idleTime, 0);
        }

		/// <summary>
		/// Converts the Flying Gentleman compatible enums to enums which match the Microsoft Web Administration library.
		/// </summary>
		/// <param name="runtime">The Flying Gentleman .Net Runtime value.</param>
		/// <returns>A Microsoft Web Administration runtime value.</returns>
		private string Convert(Runtime runtime)
		{
			switch (runtime)
			{
				case Runtime.Net20:
					return "v2.0";
				case Runtime.Net40:
					return "v4.0";
				default:
					throw new ArgumentException(string.Format("A runtime of {0} is not supported.", runtime));
			}
		}

		/// <summary>
		/// Converts the Flying Gentleman compatible enum to enums which match the Microsoft Web Administration library.
		/// </summary>
		/// <param name="pipelineMode">The Flying Gentleman value.</param>
		/// <returns>A Microsoft Web Administration value.</returns>
		private ManagedPipelineMode Convert(PipelineMode pipelineMode)
		{
			switch (pipelineMode)
			{
				case PipelineMode.Classic:
					return ManagedPipelineMode.Classic;
				case PipelineMode.Integrated:
					return ManagedPipelineMode.Integrated;
				default:
					throw new ArgumentException(string.Format("The Pipeline mode of {0} is not supported.", pipelineMode));
			}
		}

		/// <summary>
		/// Converts the Flying Gentleman compatible enum to enums which match the Microsoft Web Administration library.
		/// </summary>
		/// <param name="identityType">The Flying Gentleman value.</param>
		/// <returns>A Microsoft Web Administration value.</returns>
		private ProcessModelIdentityType Convert(IdentityType identityType)
		{
			switch (identityType)
			{
				case IdentityType.LocalService:
					return ProcessModelIdentityType.LocalService;
				case IdentityType.LocalSystem:
					return ProcessModelIdentityType.LocalSystem;
				case IdentityType.NetworkService:
					return ProcessModelIdentityType.NetworkService;
				case IdentityType.SpecificUser:
					return ProcessModelIdentityType.SpecificUser;
				default:
					throw new ArgumentException(string.Format("The Identity Type {0} is not supported.", identityType.ToString()));
			}
		}
        
        /// <summary>
        /// Restart the app pool specified
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns>Whether it succeeded</returns>
        [Obsolete("Use Start and Stop methods instead")]
        public bool RestartAppPool(string appPoolName)
        {
            try
            {
                Action<ApplicationPool> stopAction = (a) => a.Stop();
                ControlApplicationPool(appPoolName, ObjectState.Stopped, stopAction, 120);

                Action<ApplicationPool> startAction = (a) => a.Start();
                ControlApplicationPool(appPoolName, ObjectState.Started, startAction, 120);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attemts to Stop the specified Application Pool within a timeout duration
        /// </summary>
        /// <param name="appPoolName">ApplicationPool to Stop</param>
        /// <param name="maximumWaitSeconds">Timeout duration</param>
        public void StopAppPool(string appPoolName, int maximumWaitSeconds)
        {
            Action<ApplicationPool> controlAction = (a) => a.Stop();
            ControlApplicationPool(appPoolName, ObjectState.Stopped, controlAction, maximumWaitSeconds);
        }

        /// <summary>
        /// Attemts to Start the specified Application Pool within a timeout duration
        /// </summary>
        /// <param name="appPoolName">ApplicationPool to Start</param>
        /// <param name="maximumWaitSeconds">Timeout duration</param>
        public void StartAppPool(string appPoolName, int maximumWaitSeconds)
        {
            Action<ApplicationPool> controlAction = (a) => a.Start();
            ControlApplicationPool(appPoolName, ObjectState.Started, controlAction, maximumWaitSeconds);
        }

        /// <summary>
        /// Transitions an Application Pool to a desired state by applying a control action
        /// </summary>
        private void ControlApplicationPool(string appPoolName, ObjectState desiredState, Action<ApplicationPool> controlAction, int maximumWaitSeconds)
        {
            LogEventHelper.CreateEvent(this, "ControlApplicationPool", string.Format("Attempting to transition Application Pool  '{0}' to state '{1}'", appPoolName, desiredState.ToString()));

            var pool = GetAppPool(appPoolName);

            if (pool == null)
            {
                LogEventHelper.CreateEvent(this, "ControlApplicationPool", string.Format("No Application Pool found with name '{0}'.", appPoolName));
            }
            else
            {
                if (pool.State != desiredState)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    while (pool.State != desiredState)
                    {
                        try
                        {
                            controlAction(pool);
                        }
                        catch (Exception ex)
                        {
                            if (sw.Elapsed.TotalSeconds > maximumWaitSeconds)
                            {
                                string message = string.Format("Unable to transition Application Pool {0} to desired state {1} within {2} seconds", appPoolName, desiredState.ToString(), maximumWaitSeconds);
                                throw new Exception(message, ex);
                            }

                            Thread.Sleep(5000);
                        }
                    }
                }

                LogEventHelper.CreateEvent(this, "ControlApplicationPool", string.Format("Sucessfully transitioned Application Pool  '{0}' to state '{1}'", appPoolName, desiredState.ToString()));
            }
        }

        private ApplicationPool GetAppPool(string applicationPoolName)
        {
            ApplicationPool pool;

            using (var iisManager = new ServerManager())
            {
                pool = iisManager.ApplicationPools
                    .SingleOrDefault(ap => ap.Name.Equals(applicationPoolName, StringComparison.OrdinalIgnoreCase));
            }

            return pool;
        }
    }
}
