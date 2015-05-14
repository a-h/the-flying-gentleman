using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FlyingGentleman.Library.RabbitMqManagement
{
    /// <summary>
    /// Allows the management of various RabbitMq settings
    /// </summary>
    /// 
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILogEventCreator))]
    public interface IRabbitMqManagement
    {
        /// <summary>
        /// Creates a set of users on a RabbitMq server
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="server">The rabbit mq server to call e.g. 192.168.142.50 or PRDUKRAB01</param>
        [OperationContract]
        void CreateUsers(string server, RabbitMqManagementSettings settings);

        /// <summary>
        /// Set user permissions on RabbitMq server
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="server">The rabbit mq server to call e.g. 192.168.142.50 or PRDUKRAB01</param>
        [OperationContract]
        void SetUserPermissions(string server, RabbitMqManagementSettings settings);
    }
}
