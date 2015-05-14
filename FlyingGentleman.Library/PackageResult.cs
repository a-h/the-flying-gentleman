using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// The Package Result represents the overall installation of Roles onto the Target Servers.
	/// </summary>
	public class PackageResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PackageResult"/> class.  (The Package Result represents the overall installation of Roles onto the Target Servers.)
		/// </summary>
		public PackageResult()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PackageResult"/> class.  (The Package Result represents the overall installation of Roles onto the Target Servers.)
		/// </summary>
		/// <param name="package">The package which was installed.</param>
		public PackageResult(SystemPackage package)
		{
			this.SystemPackage = package;
		}

		/// <summary>
		/// The package which was installed onto the servers.
		/// </summary>
		public SystemPackage SystemPackage { get; set; }

		/// <summary>
		/// The results of the installation of Roles onto Servers.
		/// </summary>
		public List<ServerResult> ServerRoleResults { get; set; }
	}
}
