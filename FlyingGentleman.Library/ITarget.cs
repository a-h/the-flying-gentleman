using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.FileSystem;
using FlyingGentleman.Library.WebServer;
using FlyingGentleman.Library.WindowsService;
using FlyingGentleman.Library.CommandLine;
using FlyingGentleman.Library.DatabaseServer;
using System.ServiceModel;
using FlyingGentleman.Library.RabbitMqManagement;
using FlyingGentleman.Library.CultureManagement;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// The server to install onto.
	/// </summary>
	public interface ITarget
	{
		/// <summary>
		/// The name of the server (also the name of the server, e.g. UK-WEB-1).
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Provides access to the Servers file system.
		/// </summary>
		IFileSystem FileSystem { get; }

		/// <summary>
		/// Provides the ability to install and configure IIS.
		/// </summary>
		IWebServer WebServer { get; }

		/// <summary>
		/// Provides the ability to install and configure Windows Services.
		/// </summary>
		IWindowsService WindowsService { get; }

		/// <summary>
		/// Provides the ability to execute command lines on the remote server.
		/// </summary>
		ICommandLine CommandLine { get; }

		/// <summary>
		/// Provides the ability to install SQL Server onto the remote server.
		/// </summary>
		IDatabaseServer DatabaseServer { get; }

        /// <summary>
        /// Provides the ability to perform RabbitMq management functions.
        /// </summary>
        IRabbitMqManagement RabbitManagement { get; }

        /// <summary>
        /// Provides management fucntions for installation and uninstallation of custom .NET framework cultures
        /// </summary>
        ICultureManagement CultureManagement { get; }

		/// <summary>
		/// A collection of log events created during interaction with the target.
		/// </summary>
		LogEventCollection Events { get; }
	}
}
