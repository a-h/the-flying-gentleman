using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultureUtilities
{
    public class CustomCultureBuilder : ICustomCultureBuilder
    {
        public CultureAndRegionInfoBuilder BuildCustomCulture(CustomCultureSettings settings)
        {
            // Validate Settings
            var validationIssues = ValidateSettings(settings);

            if (validationIssues.Any())
            {
                throw new ArgumentException(string.Format("Provided Settings are invalid. Issues: {0}", string.Join(", ", validationIssues)));
            }

            // Build Initial Culture
            CultureAndRegionInfoBuilder builder = settings.IsSpecificCulture ? BuildSpecificCulture(settings) : BuildNeutralCulture(settings);

            // Set Required Properties 
            builder.CultureEnglishName = settings.CultureDisplayName ?? builder.CultureEnglishName;
            builder.CultureNativeName = builder.CultureEnglishName;
            builder.ThreeLetterISOLanguageName = settings.ThreeLetterISOLanguageName ?? builder.ThreeLetterISOLanguageName;
            builder.ThreeLetterWindowsLanguageName = builder.ThreeLetterISOLanguageName;
            builder.TwoLetterISOLanguageName = settings.TwoLetterISOLanguageName ?? builder.TwoLetterISOLanguageName;
            builder.IsRightToLeft = settings.IsRightToLeft;
            builder.IetfLanguageTag = settings.CultureName;

            // Set Optional Overriden Properties
            builder.NumberFormat.CurrencyDecimalSeparator = settings.CurrencyDecimalSeperator ?? builder.NumberFormat.CurrencyDecimalSeparator;
            builder.NumberFormat.CurrencyGroupSeparator = settings.CurrencyGroupSeperator ?? builder.NumberFormat.CurrencyGroupSeparator;
            builder.NumberFormat.NumberGroupSeparator = settings.NumberGroupSeperator ?? builder.NumberFormat.NumberGroupSeparator;
            builder.NumberFormat.NumberDecimalSeparator = settings.NumberDecimalSepearator ?? builder.NumberFormat.NumberDecimalSeparator;

            return builder;
        }

        private CultureAndRegionInfoBuilder BuildSpecificCulture(CustomCultureSettings settings)
        {
            var builder = new CultureAndRegionInfoBuilder(settings.CultureName, CultureAndRegionModifiers.None);

            // Apply Parent Culture
            var parentCultureInfo = CultureInfo.GetCultureInfo(settings.ParentCultureName);
            ApplyCultureInfo(builder, parentCultureInfo);

            // Required specific properties
            builder.Parent = parentCultureInfo;
            builder.GeoId = settings.GeoId.Value;
            builder.RegionEnglishName = settings.RegionDisplayName;
            builder.RegionNativeName = settings.RegionDisplayName; // Intentional
            builder.ThreeLetterISORegionName = settings.ThreeLetterISORegionName;
            builder.TwoLetterISORegionName = settings.TwoLetterISORegionName;
            builder.ThreeLetterWindowsRegionName = settings.ThreeLetterISORegionName; // Intentional

            return builder;
        }

        private CultureAndRegionInfoBuilder BuildNeutralCulture(CustomCultureSettings settings)
        {
            var builder = new CultureAndRegionInfoBuilder(settings.CultureName, CultureAndRegionModifiers.Neutral);

            // Apply Parent Culture if specified else Invariant Culture
            var parentCulture = !string.IsNullOrEmpty(settings.ParentCultureName) ? CultureInfo.GetCultureInfo(settings.ParentCultureName) : CultureInfo.InvariantCulture;
            builder.Parent = parentCulture;
            ApplyCultureInfo(builder, parentCulture);

            return builder;
        }

        private void ApplyCultureInfo(CultureAndRegionInfoBuilder builder, CultureInfo ci)
        {
            // Set Default Neutral properties
            builder.ISOCurrencySymbol = "XDR";
            builder.IsMetric = true;
            builder.CurrencyNativeName = "International Monetary Fund";
            builder.CurrencyEnglishName = "International Monetary Fund";

            // Neutral Region properties		
            builder.GeoId = 244;
            builder.RegionEnglishName = "Invariant Country";
            builder.RegionNativeName = "Invariant Country";
            builder.ThreeLetterISORegionName = "ivc";
            builder.TwoLetterISORegionName = "IV";
            builder.ThreeLetterWindowsRegionName = "IVC";

            // Apply Culture Info properties
            builder.TextInfo = ci.TextInfo;
            builder.NumberFormat = ci.NumberFormat;
            builder.GregorianDateTimeFormat = ci.DateTimeFormat;
            builder.CompareInfo = ci.CompareInfo;
        }

        public List<string> ValidateSettings(CustomCultureSettings settings)
        {
            List<string> validationIssues = new List<string>();

            // Check parent if specified
            if (!string.IsNullOrEmpty(settings.ParentCultureName))
            {
                try
                {
                    var parentCulture = CultureInfo.GetCultureInfo(settings.ParentCultureName);
                    if (!parentCulture.IsNeutralCulture)
                    {
                        validationIssues.Add("Parent culture must be a Neutral culture");
                    }
                }
                catch (Exception)
                {
                    validationIssues.Add(string.Format("Specified ParentCultureName {0} does not exist", settings.ParentCultureName));
                }
            }

            // Properties always required
            if (string.IsNullOrEmpty(settings.CultureName))
            {
                validationIssues.Add("CultureName must be specified");
            }

            if (string.IsNullOrEmpty(settings.CultureDisplayName))
            {
                validationIssues.Add("CultureDisplayName must be specified");
            }

            if (string.IsNullOrEmpty(settings.TwoLetterISOLanguageName) || settings.TwoLetterISOLanguageName.Length != 2)
            {
                validationIssues.Add("TwoLetterISOLanguageName must be specified and must be 2 characters in length");
            }

            if (string.IsNullOrEmpty(settings.ThreeLetterISOLanguageName) || settings.ThreeLetterISOLanguageName.Length != 3)
            {
                validationIssues.Add("ThreeLetterISOLanguageName must be specified and must be 3 characters in length");
            }

            if (settings.IsSpecificCulture)
            {
                // Properties required for specific cultures
                if (string.IsNullOrEmpty(settings.RegionDisplayName))
                {
                    validationIssues.Add("RegionDisplayName must be specified");
                }

                if (string.IsNullOrEmpty(settings.TwoLetterISORegionName) || settings.TwoLetterISORegionName.Length != 2)
                {
                    validationIssues.Add("TwoLetterISORegionName must be specified and must be 2 characters in length");
                }

                if (string.IsNullOrEmpty(settings.ThreeLetterISORegionName) || settings.ThreeLetterISORegionName.Length != 3)
                {
                    validationIssues.Add("ThreeLetterISORegionName must be specified and must be 3 characters in length");
                }

                if (!settings.GeoId.HasValue)
                {
                    validationIssues.Add("GeoId must be specified");
                }
            }

            return validationIssues;
        }
    }
}
