using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WindowsService
{
	/// <summary>
	/// The start mode of the Windows Service.
	/// </summary>
	[DataContract]
	public enum ServiceStartMode
	{
		/// <summary>
		/// Starts the Service when the Operating System starts up.
		/// </summary>
		[EnumMember]
		Automatic = 2,

		/// <summary>
		/// The service must be manually started.
		/// </summary>
		[EnumMember]
		Manual = 3,

		/// <summary>
		/// The Service is disabled, and must be explicitly enabled.
		/// </summary>
		[EnumMember]
		Disabled = 4
	}
}
