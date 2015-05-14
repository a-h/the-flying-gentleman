using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// A Role is a recipe for a server installation.  In a 3 tier Web Architecture, several roles may be required, 
	/// e.g. "Web Server", "WCF Server", "Database Server", "Bulk Processing Server".
	/// 
	/// Each of these roles could share the same settings, e.g. a settings class might be created which contains
	/// the connection string to the database.  The "Bulk Processing Server" role and the "WCF Server Role" might use
	/// the same settings.
	/// </summary>
	public abstract class Role : IRole
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
		public Role()
		{
			this.InstallationEvents = new LogEventCollection();
            this.AdditionalTargets = new List<string>();
		}

        /// <summary>
        /// Carries out pre-installation steps against the target.
        /// These will be run before ANY roles in a package have been installed.
        /// </summary>
        public abstract void PreInstall(ITarget target, ITarget[] buildServers);

        /// <summary>
        /// Installs the role on the target server
        /// </summary>
        /// <param name="target">the target server</param>
        /// <param name="additionalTargets">The additional targets.</param>
        public abstract void Install(ITarget target, ITarget[] additionalTargets);

        /// <summary>
        /// Carries out post-installation steps against the target.
        /// These will be run after ALL roles in a package have installed
        /// </summary>
        public abstract void PostInstall(ITarget target, ITarget[] buildServers);

		/// <summary>
		/// A collection of Events created during the installation of the Role.
		/// </summary>
        public LogEventCollection InstallationEvents { get; private set; }

        /// <summary>
        /// Any additional targets required for the installation, for example a WCF server to install the db via
        /// </summary>
        public List<string> AdditionalTargets { get; private set; }

        /// <summary>
        /// The name of the role that is being installed, 
        /// e.g. "Translation Kit Exchange public facing web service" or "Capacity Management database"
        /// </summary>
        public abstract string Name { get; }
    }
}
