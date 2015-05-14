using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.RabbitMqManagement
{
    /// <summary>
    /// Settings related to managing RabbitMq servers
    /// </summary>
    public class RabbitMqManagementSettings
    {
        /// <summary>
        /// The port number used for the RabbitMq Management site
        /// </summary>
        public string RabbitMqManagementPort { get; set; }

        /// <summary>
        /// The name of the VHost to configure.
        /// </summary>
        public string VHost { get; set; }

        /// <summary>
        /// The user name for the admin account on the RabbitMq server e.g. guest
        /// </summary>
        public string AdminUserName { get; set; }

        /// <summary>
        /// The password for the admin account on RabbitMq server e.g. guest
        /// </summary>
        public string AdminPassword { get; set; }

        /// <summary>
        /// A collection of users to create along with their permissions.
        /// </summary>
        public RabbitMqUser[] Users { get; set; }
    }
}
