using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WebServer
{
	/// <summary>
	/// Used to configure the Identity Type of an Application Pool.
	/// </summary>
	[DataContract]
	public enum IdentityType
	{
		/// <summary>
		/// The Application Pool will run as the local system user.
		/// </summary>
		[EnumMember]
		LocalSystem = 0,

		/// <summary>
		/// The Application Pool will run as the local service user.
		/// </summary>
		[EnumMember]
		LocalService = 1,

		/// <summary>
		/// The Application Pool will run as NetworkService.
		/// </summary>
		[EnumMember]
		NetworkService = 2,

		/// <summary>
		/// The Application Pool will run as a specific configured Windows User.
		/// </summary>
		[EnumMember]
		SpecificUser = 3,
	}
}
