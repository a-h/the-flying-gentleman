using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultureUtilities
{
    public class CustomCultureInstallationResult
    {
        public string CultureName { get; set; }

        public string CultureEnglishName { get; set; }

        public bool Success { get; set; }

        public string ExceptionDetail { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
