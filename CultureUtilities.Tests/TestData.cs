using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultureUtilities.Tests
{
    public static class TestData
    {
        public static CustomCultureSettings FullyPopulatedSpecificCultureSettings()
        {
            return new CustomCultureSettings()
            {
                IsSpecificCulture = true,
                CultureName = "x-test",
                CultureDisplayName = "Test Culture (Test Region)",
                ParentCultureName = "en",
                ThreeLetterISOLanguageName = "tst",
                TwoLetterISOLanguageName = "ts",
                RegionDisplayName = "Test Region",
                ThreeLetterISORegionName = "tmi",
                TwoLetterISORegionName = "tm",
                IsRightToLeft = true,
                GeoId = 123,
            };
        }

        public static CustomCultureSettings FullyPopulatedNeutralCultureSettings()
        {
            return new CustomCultureSettings()
            {
                IsSpecificCulture = false,
                CultureName = "x-test",
                CultureDisplayName = "Test Culture",
                ParentCultureName = null,
                ThreeLetterISOLanguageName = "tst",
                TwoLetterISOLanguageName = "ts"
            };
        }

        public static CultureAndRegionInfoBuilder IntegrationTestBuilder()
        {
            var builder = new CultureAndRegionInfoBuilder("x-test", CultureAndRegionModifiers.None);

            builder.LoadDataFromCultureInfo(CultureInfo.GetCultureInfo("en-GB")); // Clone English
            builder.LoadDataFromRegionInfo(new RegionInfo("en-GB"));

            return builder;
        }
    }
}
