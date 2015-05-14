using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FlyingGentleman.Library.CommandLine
{
	/// <summary>
	/// An interface which defines how access to run command lines on the remote server is implemented.
	/// </summary>
	[ServiceContract(SessionMode=SessionMode.Required, CallbackContract=typeof(ILogEventCreator))]
	public interface ICommandLine
	{
		/// <summary>
		/// Executes a Command Line on the remote server.  It is not recommended to execute any command line
		/// which could result in a prompt, since it will not be possible for the system to enter any input.
		/// </summary>
		/// <param name="commandLine">The command line to execute, e.g. "dir".</param>
		/// <param name="timeout">The timeout of the command, after which the process will be terminated.  Use TimeSpan.MaxValue for infinity (not recommended, since the process could hang waiting for user input).</param>
		[OperationContract]
		void ExecuteCommandLine(string commandLine, TimeSpan timeout);
	}
}
