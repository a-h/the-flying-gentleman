using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultureUtilities
{
    public class CustomCultureInstaller
    {
        private ICustomCultureBuilder _builder;
        private List<CultureInfo> _cultures;

        public CustomCultureInstaller(ICustomCultureBuilder builder)
        {
            _builder = builder;
            _cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        }

        /// <summary>
        /// Builds and installs a custom culture from the provided settings
        /// </summary>
        /// <param name="settings">Source settings for Custom Culture</param>
        /// <param name="replaceExisting">Whether to replace any existing Custom Culture of the same name</param>
        /// <param name="isTest">If this installation is a test (culture will be subsequently uninstalled if test)</param>
        /// <returns></returns>
        public CustomCultureInstallationResult Install(CustomCultureSettings settings, bool replaceExisting = true, bool isTest = false)
        {
            try
            {
                var culture = _builder.BuildCustomCulture(settings);

                bool frameworkCultureExists = _cultures.Any(a => a.Name == culture.CultureName && (a.CultureTypes & CultureTypes.UserCustomCulture) != CultureTypes.UserCustomCulture);

                // If a culture of this name already exists and is not a custom culture, it cannot be replaced
                if (!frameworkCultureExists)
                {
                    if (replaceExisting)
                    {
                        TryUninstall(culture.CultureName);
                    }

                    culture.Register();
                }

            }
            catch (Exception ex)
            {
                return new CustomCultureInstallationResult()
                {
                    Success = false,
                    CultureName = settings.CultureName,
                    CultureEnglishName = settings.CultureDisplayName,
                    ExceptionDetail = ex.StackTrace,
                    ExceptionMessage = string.Format("Unable to Install custom culture: {0}", ex.Message),
                };
            }
            finally
            {
                if (isTest)
                {
                    TryUninstall(settings.CultureName);
                }
            }

            return new CustomCultureInstallationResult()
            {
                Success = true,
                CultureEnglishName = settings.CultureDisplayName,
            };
        }

        public bool TryUninstall(string cultureName)
        {
            try
            {
                CultureAndRegionInfoBuilder.Unregister(cultureName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
