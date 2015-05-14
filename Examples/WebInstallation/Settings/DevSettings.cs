using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInstallation.Settings
{
    class DevSettings : ISettings
    {
        public DevSettings(Arguments arguments)
        {
            this.Arguments = arguments;
        }

        public string WebsiteName
        {
            get { return "Test Web Site"; }
        }

        public string[] WebServers
        {
            get { return new[] { "dev-gms-web1", "dev-gms-web2" }; } 
        }

        public string DatabaseServer
        {
            get { return "dev-gms-db"; }
        }

        public string BulkProcessingServer
        {
            get { return "dev-gms-bp"; }
        }

        public Arguments Arguments { get; private set; }

        public string WebsitePath
        {
            get { return @"C:\Websites\GMS\SiteRoot\"; }
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
            get { return @"E:\Logs\TestWebSite\"; }
        }

        public string WindowsServiceSourceCodeLocation
        {
            get { return @"F:\AutomatedBuilds\GoogleReview\LatestBuild\GteIntegration\GteApiPoller\"; }
        }

        public string WindowsServiceInstallLocation
        {
            get { return @"F:\Services\GteService\"; }
        }
    }
}
