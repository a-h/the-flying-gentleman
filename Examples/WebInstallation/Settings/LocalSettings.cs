using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInstallation.Settings
{
    public class LocalSettings : ISettings
    {
        public LocalSettings(Arguments arguments)
        {
            this.Arguments = arguments;
        }

        public string WebsiteName
        {
            get { return "Test Web Site"; }
        }

        public string[] WebServers
        {
            get { return new[] { "localhost" }; } 
        }

        public string DatabaseServer
        {
            get { return "localhost"; }
        }

        public string BulkProcessingServer
        {
            get { return "localhost"; }
        }

        public Arguments Arguments { get; private set; }


        public string WebsitePath
        {
            get { return @"C:\inetpub\wwwroot"; }
        }

        public DatabaseActionEnum DatabaseAction
        {
            get { return DatabaseActionEnum.DropAndCreate; }
        }


        public string WebApplicationInstallLocation
        {
            get { return @"C:\Temp\TestWebSite"; }
        }

        public string WebApplicationSourceLocation
        {
            get { return @"F:\AutomatedBuilds\CapacityManagement\LatestBuild\CapacityManagement.Web\"; }
        }

        public string WebsiteLogsPath
        {
            get { return @"C:\Logs\TestWebSite\"; }
        }

        public string WindowsServiceSourceCodeLocation
        {
            get { return @"F:\AutomatedBuilds\GoogleReview\LatestBuild\GteIntegration\GteApiPoller\"; }
        }

        public string WindowsServiceInstallLocation
        {
            get { return @"C:\Services\GteApiPoller\"; }
        }
    }
}
