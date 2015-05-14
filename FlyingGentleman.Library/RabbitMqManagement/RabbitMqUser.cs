using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.RabbitMqManagement
{
    /// <summary>
    /// Settings used to create an individual RabbitMq user
    /// </summary>
    public class RabbitMqUser
    {
        /// <summary>
        /// The user name desired for the new user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password for the new user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The tag value for this user. Use one of the predefined values in the static 
        /// class Tags i.e. administrator, monitoring or management
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// A RegEx for setting the users configure permissions
        /// </summary>
        public string ConfigureRegEx { get; set; }

        /// <summary>
        /// A RegEx for setting the users write permissions
        /// </summary>
        public string WriteRegEx { get; set; }

        /// <summary>
        /// A RegEx for setting the users read permissions
        /// </summary>
        public string ReadRegEx { get; set; }
    }
}
