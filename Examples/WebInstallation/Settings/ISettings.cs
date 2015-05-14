using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInstallation.Settings
{
	/// <summary>
	/// The settings for this installation.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// The name of the Website.
		/// </summary>
		string WebsiteName { get; }

        /// <summary>
        /// The servers to deploy the web application to
        /// </summary>
        string[] WebServers { get; }

        /// <summary>
        /// The path on the servers to the root of the website that the web applications will belong to
        /// </summary>
        string WebsitePath { get; }

        /// <summary>
        /// The server that the database should be deployed to
        /// </summary>
        string DatabaseServer { get; }

        /// <summary>
        /// The server that the windows services should be deployed to
        /// </summary>
        string BulkProcessingServer { get; }
        
        /// <summary>
        /// Whether or not to configure the servers, as well deploy the application
        /// </summary>
        Arguments Arguments { get; }

        /// <summary>
        /// Whether or not to configure the servers, as well deploy the application
        /// </summary>
        DatabaseActionEnum DatabaseAction { get; }

        /// <summary>
        /// Where on the server the web application will live
        /// </summary>
        string WebApplicationInstallLocation { get; }

        /// <summary>
        /// Where on the build server the web application lives
        /// </summary>
        string WebApplicationSourceLocation { get; }

        /// <summary>
        /// Where the website writes its logs
        /// </summary>
        string WebsiteLogsPath { get; }

        /// <summary>
        /// Where the windows service comes from
        /// </summary>
        string WindowsServiceSourceCodeLocation { get; }

        /// <summary>
        /// Where the windows service goes
        /// </summary>
        string WindowsServiceInstallLocation { get; }
    }
}
