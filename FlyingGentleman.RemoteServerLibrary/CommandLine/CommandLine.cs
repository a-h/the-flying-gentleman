using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.CommandLine;
using FlyingGentleman.Library;
using System.Diagnostics;

namespace FlyingGentleman.RemoteServerLibrary.CommandLine
{
    /// <summary>
    /// A class which executes command lines on a server.
    /// </summary>
    [Serializable]
    public class CommandLine : LogEventCreatorBase, ICommandLine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine"/> class.
        /// </summary>
        public CommandLine()
        {
        }

        /// <summary>
        /// Executes a Command Line on the remote server.  It is not recommended to execute any command line
        /// which could result in a prompt, since it will not be possible for the system to enter any input.
        /// Any exit code other than 0 will result in an exception.
        /// </summary>
        /// <param name="commandLine">The command line to execute, e.g. "dir".</param>
        /// <param name="timeout">The timeout of the command, after which the process will be terminated.  Use TimeSpan.MaxValue for infinity (not recommended, since the process could hang waiting for user input).</param>
        public void ExecuteCommandLine(string commandLine, TimeSpan timeout)
        {
            ExecuteCommandLine(commandLine, timeout, new int[] { });
        }

        /// <summary>
        /// Executes a Command Line on the remote server.  It is not recommended to execute any command line
        /// which could result in a prompt, since it will not be possible for the system to enter any input.
        /// </summary>
        /// <param name="commandLine">The command line to execute, e.g. "dir".</param>
        /// <param name="timeout">The timeout of the command, after which the process will be terminated.  Use TimeSpan.MaxValue for infinity (not recommended, since the process could hang waiting for user input).</param>
        /// <param name="validExitCodes">Exit codes, other than zero, that are expected.</param>
        public void ExecuteCommandLine(string commandLine, TimeSpan timeout, int[] validExitCodes)
        {
            LogEventHelper.CreateEvent(this, "ExecuteCommandLine", string.Format("Executing Command Line: {0}", commandLine));

            var start = new ProcessStartInfo("cmd", "/c " + commandLine);

            start.RedirectStandardError = true;
            start.RedirectStandardOutput = true;
            start.UseShellExecute = false;

            // Do not create the black window.

            start.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            using (var process = new Process())
            {
                process.StartInfo = start;
                process.Start();

                string output = null;
                string error = null;
                while ((output = process.StandardOutput.ReadLine()) != null || (error = process.StandardError.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(output))
                    {
                        LogEventHelper.CreateEvent(this, "ExecuteCommandLine:Output", output);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        LogEventHelper.CreateEvent(this, "ExecuteCommandLine:Error", error);
                    }
                }

                process.WaitForExit((int)timeout.TotalMilliseconds);

                if (!process.HasExited)
                {
                    throw new Exception(string.Format("Command {0} failed to exit within timeout {1}", commandLine, timeout));
                }
                else if (process.ExitCode != 0 && !validExitCodes.Contains(process.ExitCode))
                {
                    throw new Exception(string.Format("Command {0} exited with exit code {1}", commandLine, process.ExitCode));
                }

                process.Close();
            }
        }
    }
}



