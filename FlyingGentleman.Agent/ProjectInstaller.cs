using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Management;

namespace FlyingGentleman.Agent
{
	/// <summary>
	/// Initializes the installation of the Agent.
	/// </summary>
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
		/// </summary>
		public ProjectInstaller()
		{
			InitializeComponent();
		}

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);

            const string netTcpPortSharing = "NetTcpPortSharing";

            bool exists = ServiceExists(netTcpPortSharing);

            if(exists)
            {
                ManagementPath path = new ManagementPath(string.Format("Win32_Service.Name='{0}'", netTcpPortSharing));

                using(ManagementObject mo = new ManagementObject(path))
                {
                    string startMode = (string)mo["StartMode"];

                    if(string.Equals(startMode, "Disabled", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ManagementBaseObject inParams = mo.GetMethodParameters("ChangeStartMode");
                        inParams["StartMode"] = "Automatic";

                        mo.InvokeMethod("ChangeStartMode", inParams, null);
                    }

                    bool started = (bool)mo["Started"];

                    if(!started)
                    {
                        mo.InvokeMethod("StartService", null);
                    }
                }
            }
        }

        private bool ServiceExists(string serviceName)
        {
            bool exists = false;

            WqlObjectQuery query = new WqlObjectQuery(string.Format("SELECT Name FROM Win32_Service WHERE Name='{0}'", serviceName));

            using(ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                using(ManagementObjectCollection results = searcher.Get())
                {
                    exists = (results.Count > 0);
                }
            }

            return exists;
        }
	}
}
