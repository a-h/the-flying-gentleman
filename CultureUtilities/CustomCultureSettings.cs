using System;
using System.Collections.Generic;

namespace CultureUtilities
{
    public class CustomCultureSettings
    {
        // Required Settings

        public bool IsSpecificCulture { get; set; }

        public string CultureDisplayName { get; set; }

        public string CultureName { get; set; }

        public string ParentCultureName { get; set; }

        public string ThreeLetterISOLanguageName { get; set; }

        public string TwoLetterISOLanguageName { get; set; }

        public bool IsRightToLeft { get; set; }

        // Required if specific culture settings

        public string RegionDisplayName { get; set; }

        public string ThreeLetterISORegionName { get; set; }

        public string TwoLetterISORegionName { get; set; }

        public int? GeoId { get; set; }

        // Optional Settings

        public string CurrencyGroupSeperator { get; set; }

        public string CurrencyDecimalSeperator { get; set; }

        public string NumberGroupSeperator { get; set; }

        public string NumberDecimalSepearator { get; set; }
    }
}