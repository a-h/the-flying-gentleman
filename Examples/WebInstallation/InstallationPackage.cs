using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using System.Runtime.Serialization;

namespace WebInstallation
{
	[DataContract]
	public class InstallationPackage : SystemPackage
    {
        public override string Name { get { return "Web Installer Example"; } }
    }
}
