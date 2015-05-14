using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.WebServer;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;

namespace FlyingGentleman.Mocks.WebServer
{
	public class WebServer : MockBase, IWebServer
	{
		public WebServer(Action<LogEvent> a)
		{
			this.Action = a;
		}

		public void Install()
		{
			LogEventHelper.CreateEvent(this.Action, this, "Install", "Installing a Web Server.");
		}

        public void InstallWcfSupport()
        {
            LogEventHelper.CreateEvent(this, "Install", "Installing WCF support.");
        }
        
        public void InstallMvc3()
        {
            LogEventHelper.CreateEvent(this, "InstallMvc3", "Installing ASP.NET MVC3.");
        }

		public void SetupWebsite(WebsiteSettings settings)
		{
			LogEventHelper.CreateEvent(this.Action, this, "SetupWebsite", settings.ToJson());
		}

		public void SetupVirtualDirectory(VirtualDirectorySettings settings)
		{
			LogEventHelper.CreateEvent(this.Action, this, "SetupVirtualDirectory", settings.ToJson());
		}

		public void SetupApplicationPool(ApplicationPoolSettings settings)
		{
			LogEventHelper.CreateEvent(this.Action, this, "SetupAplicationPool", settings.ToJson());
		}
        
        public bool RestartAppPool(string appPoolName)
        {
            LogEventHelper.CreateEvent(this.Action, this, "RestartAppPool", appPoolName);
            return true;
        }

        public void StopAppPool(string appPoolName, int maximumWaitSeconds)
        {
            LogEventHelper.CreateEvent(this.Action, this, "StopAppPool", appPoolName);
        }

        public void StartAppPool(string appPoolName, int maximumWaitSeconds)
        {
            LogEventHelper.CreateEvent(this.Action, this, "StartAppPool", appPoolName);
        }
    }
}
