using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Agent.Properties;
using System.ServiceProcess;
using System.IO;

namespace FlyingGentleman.Agent
{
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	class Program
	{
		/// <summary>
		/// Starts the Agent, in Console Mode or Windows Service mode.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
            if (Settings.Default.IsServiceMode)
            {
                // Run the Windows Service.
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[] { new AgentService() };
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                // Run the Service in Console Mode (for use when debugging).
                var worker = new AgentWorker();

                worker.Start();

                Console.WriteLine("The service is ready at net.tcp://localhost/[Service].svc");
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                worker.Stop();
            }
		}
	}
}