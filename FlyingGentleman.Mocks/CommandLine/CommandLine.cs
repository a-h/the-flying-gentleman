using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.CommandLine;
using FlyingGentleman.Library;

namespace FlyingGentleman.Mocks.CommandLine
{
	/// <summary>
	/// A mock implementation of the Command Line, used to test deployments.
	/// </summary>
	public class CommandLine : MockBase, ICommandLine
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommandLine"/> class.
		/// </summary>
		/// <param name="a">A.</param>
		public CommandLine(Action<LogEvent> a)
		{
			this.Action = a;
		}

		/// <summary>
		/// Mocks the execution of the command line.
		/// </summary>
		/// <param name="commandLine">The command line.</param>
		public void ExecuteCommandLine(string commandLine, TimeSpan timeout)
		{
			LogEventHelper.CreateEvent(this.Action, this, "ExecuteCommandLine", commandLine + " - Timeout: " + timeout.ToString());
		}
	}
}
