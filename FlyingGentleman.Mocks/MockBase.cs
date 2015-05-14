using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;

namespace FlyingGentleman.Mocks
{
	public class MockBase
	{
		public Action<LogEvent> Action { get; set; }

		public void LogEventCreated(LogEvent e)
		{
			if (this.Action != null)
			{
				this.Action.Invoke(e);
			}
		}
	}
}
