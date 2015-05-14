using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;
using FlyingGentleman.RemoteServerLibrary.WcfProxy;

namespace FlyingGentleman.AgentClient
{
	class Program
	{
		static void Main(string[] args)
		{
			// This could also be done by using an inline action, e.g.:
			// Action<LogEvent> eventWriter2 = (e) => Console.WriteLine(e.Action);
			Action<LogEvent> eventWriter = target_EventCreated;

			using (var target = new RemoteServerTarget("localhost", eventWriter))
			{
				while (true)
				{
					Console.WriteLine("Press any key to list the files on the server.");
					Console.ReadKey(false);

					target.CommandLine.ExecuteCommandLine("dir", new TimeSpan(0, 0, 30));
				}
			}
		}

		static void target_EventCreated(Library.LogEvent e)
		{
			Console.WriteLine(e.ToJson());
		}
	}

	public class LogEventCreator : ILogEventCreator
	{
		public void LogEventCreated(LogEvent e)
		{
			Console.WriteLine(e.Action);
		}
	}
}
