using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.WindowsService;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;

namespace FlyingGentleman.Mocks.WindowsService
{
	public class WindowsService : MockBase, IWindowsService
	{
		public WindowsService(Action<LogEvent> a)
		{
			this.Action = a;
		}

        public void Start(string name, string server = ".")
		{
			LogEventHelper.CreateEvent(this.Action, this, "Start", string.Format("Starting service {0}", name));
		}

        public void Stop(string name, string server = ".")
		{
			LogEventHelper.CreateEvent(this.Action, this, "Stop", string.Format("Starting service {0}", name));
		}

		public void Install(WindowsServiceSettings settings)
		{
			LogEventHelper.CreateEvent(this.Action, this, "Install", settings.ToJson());
		}
    }
}
