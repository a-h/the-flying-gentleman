using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using System.ServiceModel.Channels;
using System.ServiceModel;
using FlyingGentleman.Library.FileSystem;
using FlyingGentleman.Library.WebServer;
using FlyingGentleman.Library.WindowsService;
using FlyingGentleman.Library.CommandLine;
using FlyingGentleman.Library.DatabaseServer;
using FlyingGentleman.Library.RabbitMqManagement;
using FlyingGentleman.Library.CultureManagement;

namespace FlyingGentleman.RemoteServerLibrary.WcfProxy
{
	/// <summary>
	/// Used when the deployment user is not running the FlyingGentleman Console
	/// application on the server itself. The services will run on port 9001
	/// </summary>
	public class RemoteServerTarget : ITarget, IDisposable
	{
		public RemoteServerTarget()
            :this(Environment.MachineName){}

        /// <summary>
        /// Creates server target for a given machine. 
        /// The services will run on port 9001
        /// </summary>
        /// <param name="name">The machine name</param>
        public RemoteServerTarget(string name)
            : this(name, null){}

		public RemoteServerTarget(string name, Action<LogEvent> action)
            :this(name, 9001, action){}

        public RemoteServerTarget(string name, int port, Action<LogEvent> action)
        {
            this.Name = name;
            this.Port = port;

            var callbackHandler = new AddToCollectionCallbackHandler(_events, action);

            this.FileSystem = DuplexChannelFactory<IFileSystem>.CreateChannel(callbackHandler, CreateBinding(), CreateAddress(name, "FileSystem"));
            this.WebServer = DuplexChannelFactory<IWebServer>.CreateChannel(callbackHandler, CreateBinding(), CreateAddress(name, "WebServer"));
            this.WindowsService = DuplexChannelFactory<IWindowsService>.CreateChannel(callbackHandler, CreateBinding(), CreateAddress(name, "WindowsService"));
            this.CommandLine = DuplexChannelFactory<ICommandLine>.CreateChannel(callbackHandler, CreateBinding(), CreateAddress(name, "CommandLine"));
            this.DatabaseServer = DuplexChannelFactory<IDatabaseServer>.CreateChannel(callbackHandler, CreateBinding(), CreateAddress(name, "DatabaseServer"));
            this.RabbitManagement = DuplexChannelFactory<IRabbitMqManagement>.CreateChannel(callbackHandler, CreateBinding(), CreateAddress(name, "RabbitMqManagement"));
            this.CultureManagement = DuplexChannelFactory<ICultureManagement>.CreateChannel(callbackHandler, CreateBinding(), CreateAddress(name, "CultureManagement"));
        }

		private EndpointAddress CreateAddress(string serverName, string operation)
		{
			string url = string.Format("net.tcp://{0}:{1}/{2}.svc", serverName, Port, operation);
			return new EndpointAddress(url);
		}

		private Binding CreateBinding()
		{
			var binding = new NetTcpBinding();
			binding.ReceiveTimeout = new TimeSpan(1, 0, 0, 0);
			binding.SendTimeout = new TimeSpan(1, 0, 0);
			binding.Security.Mode = SecurityMode.None;
            binding.PortSharingEnabled = true;

			return binding;
		}

		private LogEventCollection _events = new LogEventCollection();

		public LogEventCollection Events
		{
			get
			{
				return _events;
			}
		}

		public IFileSystem FileSystem { get; private set; }
		public IWebServer WebServer { get; private set; }
		public IWindowsService WindowsService { get; private set; }
		public ICommandLine CommandLine { get; private set; }
		public IDatabaseServer DatabaseServer { get; private set; }
        public IRabbitMqManagement RabbitManagement { get; private set; }
        public ICultureManagement CultureManagement { get; private set; }

        /// <summary>
        /// Gets process Id for this process
        /// </summary>
        public int ProcessId
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().Id;
            }
        }

		/// <summary>
		/// The name of the server.
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// The port the service runs on
        /// </summary>
        public int Port { get; set; }

		public void Dispose()
		{
			DisposeOfCommunicationObject<IFileSystem>(this.FileSystem);
			DisposeOfCommunicationObject<IFileSystem>(this.FileSystem);
			DisposeOfCommunicationObject<IWebServer>(this.WebServer);
			DisposeOfCommunicationObject<IWindowsService>(this.WindowsService);
			DisposeOfCommunicationObject<ICommandLine>(this.CommandLine);
			DisposeOfCommunicationObject<IDatabaseServer>(this.DatabaseServer);
            DisposeOfCommunicationObject<IRabbitMqManagement>(this.RabbitManagement);
            DisposeOfCommunicationObject<ICultureManagement>(this.CultureManagement);
		}

		private void DisposeOfCommunicationObject<TInterface>(TInterface o)
		{
			if (o != null)
			{
				if (o is CommunicationObject)
				{
					try
					{
						(o as CommunicationObject).Abort();
					}
					finally
					{
					}

					try
					{
						(o as CommunicationObject).Close();
					}
					finally
					{
					}
				}

				if (o is IDisposable)
				{
					(o as IDisposable).Dispose();
				}
			}
		}
	}
}
