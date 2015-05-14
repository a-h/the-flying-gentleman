# the-flying-gentleman

A deployment framework for writing deployments in C#.

Define:
* Roles (e.g. Database, Website, WindowsService).
* Installation steps against target servers.
 * Command Lines
 * Custom .Net Cultures.
 * Run SQL Server Scripts and Entity Framework Migrations.
 * Mirror directories, delete files and transform App.Config files.
 * Setup RabbitMQ users.
 * Install and setup IIS, set up application pools, start and stop pools.
 * Install and manage Windows services.


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
            
            // Setup the database server to have the database role.
            if (!string.IsNullOrEmpty(settings.DatabaseServer))
            {
                Server dbServer = new Server(settings.DatabaseServer);

                dbServer.Roles.Add(new DatabaseRole(settings));

                installation.AddServer(dbServer);
            }

            // Setup the background-processing server to have the Windows Service role.
            if (!string.IsNullOrEmpty(settings.BulkProcessingServer))
            {
                Server dbServer = new Server(settings.BulkProcessingServer);

                dbServer.Roles.Add(new WindowsServiceRole(settings));

                installation.AddServer(dbServer);
            }

            InstallationHelper.InstallServerPackage(installation, settings.Arguments.IsTest);        
          }
