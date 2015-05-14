using System;
using System.IO;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;
using FlyingGentleman.Library.InstallationReport;
using FlyingGentleman.Mocks;
using FlyingGentleman.RemoteServerLibrary.WcfProxy;

namespace FlyingGentleman.Client
{
    public static class InstallationHelper
    {
        public static void InstallServerPackage(SystemPackage package, bool isTest)
        {
            IServerFactory factory;

            if (isTest)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("About to test the deployment of {0}", package.Name);

                // Create a pretend server.
                factory = new MockServerFactory();
            }
            else
            {
                Console.WriteLine("About to deploy {0}", package.Name);

                // Create a real server.
                factory = new RemoteServerFactory();
            }

            Console.WriteLine("The following servers will be affected:");
            Console.WriteLine();

            foreach (Server server in package.Servers)
            {
                Console.WriteLine("Server Name: {0}", server.ServerName);
                Console.WriteLine("Roles: {0}", server.Roles.Select(r => r.Name).ToArray().ToJson());
                Console.WriteLine();
            }

            Console.WriteLine("Starting Deployment.");
            Console.WriteLine();

            // Reverse server and role orders for pre-installation events
            package.Servers.ForEach(e => e.Roles.Reverse());
            package.Servers.Reverse();

            // Perform pre-installation steps for all roles on all servers
            foreach (Server server in package.Servers)
            {
                foreach (IRole role in server.Roles)
                {
                    var target = factory.GetServer(server.ServerName, WriteEvent);
                    var additionalTargets = role.AdditionalTargets
                        .Select(t => factory.GetServer(t, WriteEvent)).ToArray();

                    Console.WriteLine("Performing Pre-Install steps for Role {0} on Server {1}", role.Name, server.ServerName);
                    role.PreInstall(target, additionalTargets);
                }
            }

            // Reverse server and role orders back for installation and post-installation events
            package.Servers.ForEach(e => e.Roles.Reverse());
            package.Servers.Reverse();

            // Install all roles on all servers
            foreach (Server server in package.Servers)
            {
                foreach (IRole role in server.Roles)
                {
                    var target = factory.GetServer(server.ServerName, WriteEvent);
                    var additionalTargets = role.AdditionalTargets
                        .Select(t => factory.GetServer(t, WriteEvent)).ToArray();

                    Console.WriteLine("Installing Role {0} on Server {1}", role.Name, server.ServerName);
                    role.Install(target, additionalTargets);
                }
            }

            // Perform post-installation steps for all roles on all servers
            foreach (Server server in package.Servers)
            {
                foreach (IRole role in server.Roles)
                {
                    var target = factory.GetServer(server.ServerName, WriteEvent);
                    var additionalTargets = role.AdditionalTargets
                        .Select(t => factory.GetServer(t, WriteEvent)).ToArray();

                    Console.WriteLine("Performing Post-Install steps for Role {0} on Server {1}", role.Name, server.ServerName);
                    role.PostInstall(target, additionalTargets);
                }
            }

            Console.WriteLine("Writing report.");
            var reportBuilder = new ReportBuilder();
            WriteReport("Report.html", reportBuilder.CreateReport(package));
        }

        /// <summary>
        /// Writes the release report to disk.
        /// </summary>
        /// <param name="fileName">The location to writ  the report to.</param>
        /// <param name="reportText">The contents of the report.</param>
        private static void WriteReport(string fileName, string reportText)
        {
            using (var sw = new StreamWriter(fileName, false, UnicodeEncoding.UTF8))
            {
                sw.Write(reportText);
                sw.Flush();
            }
        }

        /// <summary>
        /// An event handler used when server targets fire an event.
        /// </summary>
        /// <param name="e">The event.</param>
        static void target_EventCreated(LogEvent e)
        {
            WriteEvent(e);
        }

        /// <summary>
        /// Writes an event to the console.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        private static void WriteEvent(LogEvent logEvent)
        {
            string entry = string.Format("{0}\t{1}\t{2}", logEvent.Category, logEvent.Action, logEvent.Message);
            Console.WriteLine(entry);
        }
    }
}
