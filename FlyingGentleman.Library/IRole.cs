using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// Represents the installation process for a given server role.
	/// </summary>
	public interface IRole
	{
		/// <summary>
		/// The name of the server role.
		/// </summary>
		string Name { get; }

        /// <summary>
        /// Carries out pre-installation steps against the target.
        /// These will be run before ANY roles in a package have been installed.
        /// </summary>
        void PreInstall(ITarget target, ITarget[] buildServers);

		/// <summary>
		/// Carries out the installation against the target.
		/// </summary>
		void Install(ITarget target, ITarget[] buildServers);

        /// <summary>
        /// Carries out post-installation steps against the target.
        /// These will be run after ALL roles in a package have installed
        /// </summary>
        void PostInstall(ITarget target, ITarget[] buildServers);

        /// <summary>
        /// Any additional targets required for the installation, for example a WCF server to install the db via
        /// </summary>
        List<string> AdditionalTargets { get; }

		/// <summary>
		/// Events created during the installation process.
		/// </summary>
		LogEventCollection InstallationEvents { get; }
	}
}
