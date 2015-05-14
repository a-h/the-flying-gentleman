using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInstallation.Settings
{
    public class Arguments
    {
        [Args.ArgsMemberSwitch("e")]
        public EnvironmentEnum Environment { get; set; }

        [Args.ArgsMemberSwitch("c")]
        public bool InitialConfiguration { get; set; }

        [Args.ArgsMemberSwitch("t")]
        public bool IsTest { get; set; }
    }
}
