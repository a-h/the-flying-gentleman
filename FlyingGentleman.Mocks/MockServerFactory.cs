using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;

namespace FlyingGentleman.Mocks
{
	/// <summary>
	/// A class which generates a Mock Server for testing.
	/// </summary>
	public class MockServerFactory : IServerFactory
	{
		public ITarget GetServer(string name, Action<LogEvent> action)
		{
			return new MockTarget(name, action);
		}
	}
}
