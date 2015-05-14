using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;

namespace FlyingGentleman.RemoteServerLibrary.WcfProxy
{
	/// <summary>
	/// Provides instances of servers which are running the
	/// FlyingGentleman Agent.
	/// </summary>
	public class RemoteServerFactory : IServerFactory
	{
		/// <summary>
		/// Creates a Server target by DNS name.
		/// </summary>
		/// <param name="name">The DNS name of the machine.</param>
		/// <param name="action">The action to take when a log event is created.</param>
		/// <returns>The Target to install software onto.</returns>
		public ITarget GetServer(string name, Action<LogEvent> action)
		{
			return new RemoteServerTarget(name, action);
		}
	}
}
