using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.RabbitMqManagement
{
    /// <summary>
    /// Tags given to a particular user to set their "role". See http://www.rabbitmq.com/management.html -> Permissions
    /// for more details
    /// </summary>
    public static class Tags
    {
        /// <summary>
        /// Tag for administrator
        /// </summary>
        public static string Administrator { get { return "administrator"; } }

        /// <summary>
        /// Tag for monitoring
        /// </summary>
        public static string Monitoring { get { return "monitoring"; } }

        /// <summary>
        /// Tag for management
        /// </summary>
        public static string Management { get { return "management"; } }
    }
}
