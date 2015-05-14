using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// The interface for classes which will create the abstract servers.
	/// </summary>
	public interface IServerFactory
	{
		/// <summary>
		/// Creates a Server target by DNS name.
		/// </summary>
		/// <param name="name">The DNS name of the machine.</param>
		/// <param name="action">The action to take when a log event is created.</param>
		/// <returns>The Target to install software onto.</returns>
		ITarget GetServer(string name, Action<LogEvent> action);
	}
}
