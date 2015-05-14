using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace CultureUtilities.Tests
{
    public static class Mappings
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<CultureAndRegionInfoBuilder, CustomCultureSettings>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
