using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInstallation.Settings
{
    public enum EnvironmentEnum
    {
        Local,
        Dev,
        Qa,
        Live
    }

    public enum DatabaseActionEnum
    { 
        DropAndCreate,
        Migrate,
        None
    }
}
