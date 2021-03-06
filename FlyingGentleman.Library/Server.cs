﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// Maps servers to their given roles.
	/// </summary>
	[DataContract]
	public class Server
	{
		/// <summary>
		/// Creates the map between servers and roles,
		/// e.g. dev-web-1 is both a Web Server and a 
		/// Database Server.
		/// </summary> 
		public Server()
		{
			this.Roles = new List<IRole>();
		}

		/// <summary>
		/// Creates the map between servers and roles,
		/// e.g. dev-web-1 is both a Web Server and a 
		/// Database Server.
		/// </summary>
		/// <param name="serverName">The name of the server.</param>
		public Server(string serverName)
			: this()
		{
			this.ServerName = serverName;
		}

		/// <summary>
		/// The name of the server, e.g. dev-web-1.
		/// </summary>
		[DataMember]
		public string ServerName { get; set; }

		/// <summary>
		/// The roles to install onto the server.
		/// </summary>
		[DataMember]
		public List<IRole> Roles { get; set; }
	}
}
