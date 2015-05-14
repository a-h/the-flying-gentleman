using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.WindowsService;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;
using System.ServiceProcess;

namespace FlyingGentleman.RemoteServerLibrary.WindowsService
{
	/// <summary>
	/// Allows access to the Windows Service functionality.
	/// </summary>
	[Serializable]
	public class WindowsService : LogEventCreatorBase, IWindowsService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WindowsService"/> class.
		/// </summary>
		public WindowsService()
		{
		}

        /// <summary>
        /// Starts a Windows Service, or does nothing if it is already running
        /// </summary>
        /// <param name="name">The name of the Windows Service to Start.</param>
        /// <param name="server">The server.</param>
		public void Start(string name, string server = ".")
		{
			LogEventHelper.CreateEvent(this, "Start", string.Format("Starting service {0} on server {1}", name, server));
            using (var sc = new ServiceController(name, server))
            {
                if (!ServiceExists(sc))
                {
                    var msg = string.Format("Service {0} does not appear to be installed on server {1}, unable to start", name, server);
                    LogEventHelper.CreateEvent(this, "Start", msg);
                    throw new Exception(msg);
                }
                else if (sc.Status == ServiceControllerStatus.Running)
                {
                    LogEventHelper.CreateEvent(this, "Start", string.Format("Service {0} is already running on server {1}", name, server));
                }
                else if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    LogEventHelper.CreateEvent(this, "Start", string.Format("Start command called on service {0} on server {1}", name, server));
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                }
            }
		}

        /// <summary>
        /// Stops a Windows Service, if it is installed and running
        /// </summary>
        /// <param name="name">The name of the Windows Service to Stop.</param>
        /// <param name="server">The server.</param>
        public void Stop(string name, string server = ".")
		{
			LogEventHelper.CreateEvent(this, "Stop", string.Format("Stopping service {0} on server {1}", name, server));
            using (var sc = new ServiceController(name, server))
            {
                if (!ServiceExists(sc))
                {
                    LogEventHelper.CreateEvent(this, "Stop", string.Format("Service {0} does not appear to be installed on server {1}", name, server));
                }
                else if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    LogEventHelper.CreateEvent(this, "Stop", string.Format("Service {0} is already stopped on server {1}", name, server));
                }
                else if (sc.Status == ServiceControllerStatus.StopPending)
                {
                    LogEventHelper.CreateEvent(this, "Stop", string.Format("Service {0} on server {1} is already in state StopPending, waiting for Stopped state", name, server));
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                else if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    LogEventHelper.CreateEvent(this, "Stop", string.Format("Stop command called on service {0} on server {1}", name, server));
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                else
                {
                    string message = string.Format("Service {0} has an invalid status {1}, unable to stop", name, sc.Status);
                    LogEventHelper.CreateEvent(this, "Stop", message);
                    throw new InvalidOperationException(message);
                }
            }
		}

		/// <summary>
		/// Installs a Windows Service.
		/// </summary>
		/// <param name="settings">The settings for the Windows Service.</param>
		public void Install(WindowsServiceSettings settings)
		{
            if (string.IsNullOrEmpty(settings.ServiceName))
            {
                throw new ArgumentException("Need to specify the name of the service", "settings.ServiceName");
            }

			LogEventHelper.CreateEvent(this, "Install", settings.ToJson());
            using (var sc = new ServiceController(settings.ServiceName, settings.ServerName))
            {
                if (ServiceExists(sc))
                {
                    LogEventHelper.CreateEvent(this, "Install", "Service already exists, not installing");
                    return;
                }
            }

            LogEventHelper.CreateEvent(this, "Install", "Service does not exist, installing");
            var installUtilPath = @"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe";
            var command = string.Format("{0} /username={1} /password={2} {3}",
                installUtilPath, settings.UserName, settings.Password, settings.ServicePath); 

            var cmd = new CommandLine.CommandLine();
            cmd.ExecuteCommandLine(command, new TimeSpan(2, 0, 0));
		}


        private static bool ServiceExists(ServiceController sc)
        {
            try
            {
                var status = sc.Status;
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
	}
}
