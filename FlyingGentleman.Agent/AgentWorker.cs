using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using FlyingGentleman.Library.CommandLine;
using FlyingGentleman.Library.DatabaseServer;
using FlyingGentleman.Library.FileSystem;
using FlyingGentleman.Library.WebServer;
using FlyingGentleman.Library.WindowsService;
using System.ServiceModel;
using FlyingGentleman.RemoteServerLibrary;
using System.ServiceModel.Description;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters;
using System.Collections;

namespace FlyingGentleman.Agent
{
	/// <summary>
	/// The Worker class for the Agent.
	/// </summary>
	public class AgentWorker
	{
		/// <summary>
		/// A list of all the Services that the Agent will run.
		/// </summary>
		List<ServiceHost> hosts = new List<ServiceHost>();

		/// <summary>
		/// Initializes a new instance of the <see cref="AgentWorker"/> class.
		/// </summary>
		public AgentWorker()
		{
			hosts.Add(new ServiceHost(typeof(RemoteServerLibrary.CommandLine.CommandLine)));
			hosts.Add(new ServiceHost(typeof(RemoteServerLibrary.DatabaseServer.DatabaseServer)));
			hosts.Add(new ServiceHost(typeof(RemoteServerLibrary.FileSystem.FileSystem)));
			hosts.Add(new ServiceHost(typeof(RemoteServerLibrary.WebServer.WebServer)));
			hosts.Add(new ServiceHost(typeof(RemoteServerLibrary.WindowsService.WindowsService)));
            hosts.Add(new ServiceHost(typeof(RemoteServerLibrary.RabbitMq.RabbitMqManagement)));
            hosts.Add(new ServiceHost(typeof(RemoteServerLibrary.CultureManagement.CultureManagement)));
		}

		/// <summary>
		/// Starts the Agent Services.
		/// </summary>
		public void Start()
		{
			foreach (ServiceHost host in hosts)
			{
				host.Open();
			}
		}

		/// <summary>
		/// Stops the Agent Services.
		/// </summary>
		public void Stop()
		{
			foreach (ServiceHost host in hosts)
			{
				host.Close();

				if (host is IDisposable)
				{
					((IDisposable)host).Dispose();
				}
			}
		}
	}
}
