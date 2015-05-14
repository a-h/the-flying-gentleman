using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.DatabaseServer
{
    /// <summary>
    /// Settings to use when Modifying the database
    /// </summary>
    public abstract class ModifySettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifySettings"/> class.
        /// </summary>
        public ModifySettings()
        {
            ServerName = "localhost";
        }

        /// <summary>
        /// The path to the entities dll to load, 
        /// e.g. C:\Temp\CapacityManagementDeployment\CapacityManagement.Entities.Dll
        /// </summary>
        public string EntitiesDllLocation { get; set; }

        /// <summary>
        /// The name of the entities type to load,
        /// e.g. CapacityManagement.Entities.CapacityManagementEntities
        /// </summary>
        public string EntitiesTypeName { get; set; }

        /// <summary>
        /// The name of the database to use as the backing store for the entities
        /// e.g. CapacityManagement
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// The name of the server to install the database on, defaults to localhost
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The name of the entities method we're going to call
        /// </summary>
        public abstract string MethodName { get; }
    }
}
