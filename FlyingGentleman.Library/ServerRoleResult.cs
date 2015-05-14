using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// The results of the installation of a Role onto a server.
	/// </summary>
	public class ServerResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServerResult"/> class.  (The results of the installation of a Role onto a server.)
		/// </summary>
		public ServerResult()
		{
			this.Events = new LogEventCollection();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerResult"/> class.  (The results of the installation of a Role onto a server.)
		/// </summary>
        /// <param name="server">The server role to install.</param>
		/// <param name="target">The target server to install it onto.</param>
		public ServerResult(Server server, ITarget target)
			: this()
		{
			this.Server = server;
			this.Target = target;
		}

		/// <summary>
		/// Gets or sets the server role which is being installed onto the target server.
		/// </summary>
		/// <value>
		/// The server role.
		/// </value>
		public Server Server { get; set; }

		/// <summary>
		/// Gets or sets the target server which the Role will be installed onto.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		public ITarget Target { get; set; }

		/// <summary>
		/// Gets or sets the events created during the installation of the Role onto the Server Target.
		/// </summary>
		/// <value>
		/// The events.
		/// </value>
		public LogEventCollection Events { get; set; }
	}
}
