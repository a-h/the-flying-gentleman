﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultureUtilities
{
    public interface ICustomCultureBuilder
    {
        CultureAndRegionInfoBuilder BuildCustomCulture(CustomCultureSettings settings);
    }
}
