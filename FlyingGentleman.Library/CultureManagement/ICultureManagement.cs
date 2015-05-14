using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using CultureUtilities;

namespace FlyingGentleman.Library.CultureManagement
{
    /// <summary>
    /// Allows the management of various RabbitMq settings
    /// </summary>
    /// 
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILogEventCreator))]
    public interface ICultureManagement
    {
        /// <summary>
        /// Install a custom culture
        /// </summary>
        /// <param name="settings">CustomCultureSettings for installation</param>
        [OperationContract]
        CustomCultureInstallationResult InstallCustomCulture(CustomCultureSettings settings);

        /// <summary>
        /// Uninstall custom culture
        /// </summary>
        /// <param name="cultureName">Name of Culture to remove</param>
        [OperationContract]
        void UninstallCustomCulture(string cultureName);
    }
}
