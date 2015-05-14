using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using FlyingGentleman.Library.FileSystem;
using FlyingGentleman.Library.WebServer;
using FlyingGentleman.Library.WindowsService;
using FlyingGentleman.Library.CommandLine;
using FlyingGentleman.Library.DatabaseServer;
using FlyingGentleman.Library.RabbitMqManagement;
using FlyingGentleman.Library.CultureManagement;

namespace FlyingGentleman.Mocks
{
	/// <summary>
	/// A Mock server, used for testing deployments.
	/// </summary>
	public class MockTarget : ITarget
	{
		public MockTarget(string name, Action<LogEvent> action)
		{
			this.Name = name;
			this.Action = action;

			// Intercept the log actions to add to the internal collection.
			Action<LogEvent> interceptLogEventsAction = le =>
				{
					this.Events.Add(le);

					if (this.Action != null)
					{
						this.Action.Invoke(le);
					}
				};

			this.FileSystem = new Mocks.FileSystem.FileSystem(interceptLogEventsAction);
			this.WebServer = new Mocks.WebServer.WebServer(interceptLogEventsAction);
			this.WindowsService = new Mocks.WindowsService.WindowsService(interceptLogEventsAction);
			this.CommandLine = new Mocks.CommandLine.CommandLine(interceptLogEventsAction);
			this.DatabaseServer = new Mocks.DatabaseServer.DatabaseServer(interceptLogEventsAction);
		}

		private LogEventCollection _events = new LogEventCollection();

		public IFileSystem FileSystem { get; private set; }
		public IWebServer WebServer { get; private set; }
		public IWindowsService WindowsService { get; private set; }
		public ICommandLine CommandLine { get; private set; }
		public IDatabaseServer DatabaseServer { get; private set; }
        public IRabbitMqManagement RabbitManagement { get; private set; }
        public ICultureManagement CultureManagement { get; private set; }

		/// <summary>
		/// The name of the server.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// All of the Events Created against the target.
		/// </summary>
		public LogEventCollection Events
		{
			get
			{
				return _events;
			}
		}

		public Action<LogEvent> Action { get; set; }
	}
}
