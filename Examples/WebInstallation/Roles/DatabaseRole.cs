using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using FlyingGentleman.Library.DatabaseServer;
using WebInstallation.Settings;

namespace WebInstallation
{
    public class DatabaseRole : Role
    {
        private ISettings settings;

        public DatabaseRole(ISettings settings)
        { 
            this.settings = settings;
        }

        public override string Name { get { return "Test Database"; } }

        public override void PreInstall(ITarget target, ITarget[] buildServers)
        {

        }

        public override void Install(ITarget target, ITarget[] additionalTargets)
        {
            if (settings.DatabaseAction == DatabaseActionEnum.DropAndCreate)
            {
                target.DatabaseServer.DropAndCreateDatabase(new CreateSettings()
                {
                    DatabaseName = "DB_TEST",
                    EntitiesDllLocation = @"\\buildserver\AutomatedBuilds\TestApplication\bin\EntityFramework.dll",
                    EntitiesTypeName = "Lqe.Entities.LqeEntities",
                });

                target.DatabaseServer.CreateLoginAndUserIfNotExists(settings.DatabaseServer, "DB_TEST", @"DOMAIN\User");
                target.DatabaseServer.AddDataReaderAndWriterRoles(settings.DatabaseServer, "DB_TEST", @"DOMAIN\User");
            }
            else if (settings.DatabaseAction == DatabaseActionEnum.Migrate)
            {
                throw new NotImplementedException("Database migration not yet implemented");
            }
            else if (settings.DatabaseAction == DatabaseActionEnum.None)
            {
                //do nothing
            }
            else
            {
                throw new Exception("Unrecognised database action");
            }
        }

        public override void PostInstall(ITarget target, ITarget[] buildServers)
        {

        }
    }
}
