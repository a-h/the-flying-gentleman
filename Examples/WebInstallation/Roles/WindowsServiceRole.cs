using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using FlyingGentleman.Library.WebServer;
using System.Runtime.Serialization;
using WebInstallation.Settings;
using FlyingGentleman.Library.WindowsService;
using FlyingGentleman.Library.FileSystem;
using System.IO;

namespace WebInstallation
{
	[DataContract]
	public class WindowsServiceRole : Role
	{
        private ISettings settings;
        
        public WindowsServiceRole(ISettings settings)
		{
            this.settings = settings;
            this.AdditionalTargets.Add("buildserver");
		}

        public override string Name
        {
            get { return "Test Windows Service"; }
        }

        public override void PreInstall(ITarget target, ITarget[] buildServers)
        {

        }

        public override void Install(ITarget target, ITarget[] additionalServers)
        {
            target.WindowsService.Stop("WindowsService"); //stop the service if it is running

            var buildServer = additionalServers.Single(s => s.Name == "buildserver");
            buildServer.FileSystem.Mirror(settings.WindowsServiceSourceCodeLocation, settings.WindowsServiceInstallLocation, MirrorSettings.DefaultSettings);

            if (settings.Arguments.InitialConfiguration)
            {
                target.WindowsService.Install(new WindowsServiceSettings()
                {
                    UserName = @"DOMAIN\User",
                    Password = @"password",
                    ServicePath = Path.Combine(settings.WindowsServiceInstallLocation, "WindowsService.exe"),
                    ServiceName = "GteApiPoller"
                });
            }

            target.WindowsService.Start("WindowsService"); //stop the service if it is running
            
            this.InstallationEvents.AddRange(target.Events);
            this.InstallationEvents.AddRange(buildServer.Events);
        }

        public override void PostInstall(ITarget target, ITarget[] buildServers)
        {

        }
    }
}
