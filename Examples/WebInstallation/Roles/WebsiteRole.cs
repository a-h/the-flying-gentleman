using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using FlyingGentleman.Library.WebServer;
using System.Runtime.Serialization;
using WebInstallation.Settings;
using FlyingGentleman.Library.FileSystem;
using System.IO;

namespace WebInstallation
{
	[DataContract]
	public class WebsiteRole : Role
	{
        private ISettings settings;

        public WebsiteRole(ISettings settings)
        {
            this.settings = settings;
            this.AdditionalTargets.Add("buildserver");
        }

        public override string Name
        {
            get { return "Test Web Site"; }
        }

        public override void PreInstall(ITarget target, ITarget[] buildServers)
        {

        }

        public override void Install(ITarget target, ITarget[] additionalServers)
        {
            if (settings.Arguments.InitialConfiguration)
            {
                target.WebServer.Install();

                target.WebServer.SetupWebsite(new WebsiteSettings() { 
                    WebsiteName = settings.WebsiteName,
                    Path = settings.WebsitePath, 
                    LogFileDirectory = settings.WebsiteLogsPath
                });

                target.WebServer.SetupApplicationPool(new ApplicationPoolSettings() { 
                    ApplicationPoolName = "TestSiteAppPool",
                    Runtime = Runtime.Net40,
                    PipelineMode = PipelineMode.Integrated,
                    IdentityType = IdentityType.LocalSystem,
                    IdleTimeout = 1740
                });

                target.WebServer.SetupVirtualDirectory(new VirtualDirectorySettings() {
                    WebsiteName = settings.WebsiteName, 
                    Name = "TestSite",
                    Path = settings.WebApplicationInstallLocation,
                    ApplicationPool = "TestSiteAppPool",
                    AuthenticationAllowAnonymousAuthentication = false,
                    AuthenticationMethod = AuthenticationMethod.WindowsAuthentication
                });
            }

            var mirrorSettings = new MirrorSettings();
            mirrorSettings.IgnoreSourceDirectoryPatterns.Add(IgnorePatterns.SvnDirectory);
            mirrorSettings.IgnoreTargetFileNamePatterns.Add(IgnorePatterns.AppOfflineFile);

            var serverUncPath = string.Format(@"\\{0}", target.Name);
            var installLocation = Path.Combine(serverUncPath, settings.WebApplicationInstallLocation.Replace(':', '$'));

            var buildServer = additionalServers.Single(s => s.Name == "buildserver");
            buildServer.FileSystem.Mirror(settings.WebApplicationSourceLocation, installLocation, mirrorSettings);
            
            //don't keep track of what code we've deployed, this is just a test
            //buildServer.FileSystem.Mirror(settings.WebApplicationSourceLocation, deployedSourcePath, mirrorSettings);

            //if we're not deploying to local, make sure we're using the correct web.config
            if (settings.Arguments.Environment != EnvironmentEnum.Local)
            {   
                var configTarget = Path.Combine(installLocation, "web.config");
                var configToCopy = configTarget + "." + settings.Arguments.Environment;

                buildServer.FileSystem.CopyFile(configToCopy, configTarget, true);
                buildServer.FileSystem.Delete(configToCopy);
            }
            
            this.InstallationEvents.AddRange(target.Events);
        }

        public override void PostInstall(ITarget target, ITarget[] buildServers)
        {

        }
    }
}
