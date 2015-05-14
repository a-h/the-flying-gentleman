using System;
using System.ServiceProcess;
using System.Threading;

namespace FlyingGentleman.Agent
{
	/// <summary>
	/// The Flying Gentleman Windows Service.
	/// </summary>
	partial class AgentService : ServiceBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AgentService"/> class.
		/// </summary>
		public AgentService()
		{
			InitializeComponent();

			this.Worker = new AgentWorker();
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
		/// </summary>
		/// <param name="args">Data passed by the start command.</param>
		protected override void OnStart(string[] args)
		{
            myWorker = new Thread(() => Worker.Start());
            myWorker.Start();
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
		/// </summary>
		protected override void OnStop()
		{
            myWorker = new Thread(() => Worker.Stop());
            myWorker.Start();
		}

		/// <summary>
		/// Gets or sets the Agent Worker which will run the WCF Services.
		/// </summary>
		/// <value>
		/// The worker.
		/// </value>
		public AgentWorker Worker { get; set; }

        private Thread myWorker = null;
	}
}
