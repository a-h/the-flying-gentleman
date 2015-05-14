using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WebServer
{
	/// <summary>
	/// The .Net runtime version to apply to an application pool.
	/// </summary>
	[DataContract]
	public enum Runtime
	{
		/// <summary>
		/// States that the Runtime should be .Net 2.0.
		/// </summary>
		[EnumMember]
		Net20,

		/// <summary>
		/// States that the runtime should be .Net 4.0.
		/// </summary>
		[EnumMember]
		Net40
	}
}
