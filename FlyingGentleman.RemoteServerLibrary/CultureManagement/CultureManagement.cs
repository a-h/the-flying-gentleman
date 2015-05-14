using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using FlyingGentleman.Library.CultureManagement;
using CultureUtilities;

namespace FlyingGentleman.RemoteServerLibrary.CultureManagement
{
    /// <summary>
    /// Facilities for management of custom culture
    /// </summary>
    public class CultureManagement : LogEventCreatorBase, ICultureManagement
    {
        /// <summary>
        /// Installs a custom culture
        /// </summary>
        /// <param name="settings">Settings to install</param>
        /// <returns></returns>
        public CustomCultureInstallationResult InstallCustomCulture(CustomCultureSettings settings)
        {
            LogEventHelper.CreateEvent(this, "Installing Custom Culture",
                string.Format("{0} ({1})", settings.CultureName, settings.CultureDisplayName));

            var installer = new CustomCultureInstaller(new CustomCultureBuilder());
            var result = installer.Install(settings);

            return result;
        }

        /// <summary>
        /// Uninstall custom culture
        /// </summary>
        /// <param name="cultureName">Name of Culture to remove</param>
        public void UninstallCustomCulture(string cultureName)
        {
            var installer = new CustomCultureInstaller(new CustomCultureBuilder());
            installer.TryUninstall(cultureName);
        }
    }
}
