using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.DatabaseServer
{
	/// <summary>
	/// Settings for the database installation.
	/// </summary>
	public class InstallSettings
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InstallSettings"/> class.
		/// </summary>
		public InstallSettings()
		{
			// Default to SQL Express 64.
			this.InstallerUri = new Uri("http://care.dlservice.microsoft.com/dl/download/5/1/A/51A153F6-6B08-4F94-A7B2-BA1AD482BC75/SQLEXPR_x64_ENU.exe?lcid=1033&cprod=SQLEXP");
			this.InstallerPath = "C:\\SqlServerInstallation\\";
			this.ServerAdministrators = new List<string>();
			this.InstanceName = "MSSQLSERVER";
		}

		/// <summary>
		/// The path that will be used to store the temporary SQL server installation files.
		/// Defaults to C:\\SqlServerInstallation\\
		/// </summary>
		public string InstallerPath { get; set; }

		/// <summary>
		/// The download link to the installer.  
		/// </summary>
		public Uri InstallerUri { get; set; }

		/// <summary>
		/// A list of users or groups who will be added to the SQL Server
		/// administrators group when the server is installed.
		/// </summary>
		public List<string> ServerAdministrators { get; set; }

		/// <summary>
		/// The name of the SQL Server instance.  The default instance is called
		/// MSSQLSERVER.
		/// </summary>
		public string InstanceName { get; set; }
	}
}
