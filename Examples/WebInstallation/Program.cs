using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman;
using FlyingGentleman.Library;
using FlyingGentleman.Client;
using WebInstallation.Settings;

namespace WebInstallation
{
    class Program
    {
        static void Main(string[] args)
        {
            ISettings settings = GetSettings(args);
            var installation = new InstallationPackage();

            //Setup the web servers to have the Website role.
            foreach (var serverName in settings.WebServers)
            {
                Server webServer = new Server(serverName);

                webServer.Roles.Add(new WebsiteRole(settings));

                installation.AddServer(webServer);
            }
            
            // Setup the dev-web-1 server to have the Website role.
            if (!string.IsNullOrEmpty(settings.DatabaseServer))
            {
                Server dbServer = new Server(settings.DatabaseServer);

                dbServer.Roles.Add(new DatabaseRole(settings));

                installation.AddServer(dbServer);
            }

            // Setup the dev-web-1 server to have the Website role.
            if (!string.IsNullOrEmpty(settings.BulkProcessingServer))
            {
                Server dbServer = new Server(settings.BulkProcessingServer);

                dbServer.Roles.Add(new WindowsServiceRole(settings));

                installation.AddServer(dbServer);
            }

            InstallationHelper.InstallServerPackage(installation, settings.Arguments.IsTest);        
        }

        private static ISettings GetSettings(string[] args)
        {
            var arguments = Args.Configuration.Configure<Arguments>().CreateAndBind(args);

            switch (arguments.Environment)
            {
                case EnvironmentEnum.Local:
                    return new LocalSettings(arguments);
                case EnvironmentEnum.Dev:
                    return new DevSettings(arguments);
                case EnvironmentEnum.Qa:
                    throw new NotImplementedException("Deploy to QA not implemented");
                case EnvironmentEnum.Live:
                    throw new NotImplementedException("Deploy to live not implemented");
                default:
                    throw new Exception("Unrecognised environment");
            }
        }
    }
}