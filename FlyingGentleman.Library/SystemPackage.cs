using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// The base class for all system releases.
	/// </summary>
    [DataContract]
	public abstract class SystemPackage
	{
        List<Server> servers = new List<Server>();

		/// <summary>
		/// Creates a SystemPackage to install.
		/// </summary>
		public SystemPackage()
		{ }

        /// <summary>
        /// The name of the system that is being deployed
        /// </summary>
        public abstract string Name { get; }

		/// <summary>
		/// Adds a server to the package.
		/// </summary>
		/// <param name="server">The server, with its configured roles</param>
		public void AddServer(Server server)
		{
			servers.Add(server);
		}
        
		/// <summary>
		/// The map between the servers and the roles.
		/// </summary>
		public List<Server> Servers { get { return servers; } }
	}
}
