using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WebServer
{
	/// <summary>
	/// Used to set pipeline mode of an IIS Application Pool.
	/// </summary>
	[DataContract]
	public enum PipelineMode
	{
		/// <summary>
		/// A setting for Integrated Pipeline Mode (default).
		/// </summary>
		[EnumMember]
		Integrated,

		/// <summary>
		/// Maintained for compatibility with older applications.
		/// </summary>
		[EnumMember]
		Classic
	}
}
