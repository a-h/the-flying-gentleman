using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.WindowsService
{
	/// <summary>
	/// Settings related to installing a new Windows Service.
	/// </summary>
	public class WindowsServiceSettings
	{
        /// <summary>
        /// Initializes a new <see cref="WindowsServiceSettings"/>, with ServerName set to "."
        /// </summary>
        public WindowsServiceSettings()
        {
            this.ServerName = ".";
        }

		/// <summary>
		/// The Windows Username to use.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// The Windows Password to use.
		/// </summary>
		public string Password { get; set; }
                
		/// <summary>
		/// The installation path of the service.
		/// </summary>
		public string ServicePath { get; set; }

        /// <summary>
        /// The server to install the service on, defaults to local
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The name of the service
        /// </summary>
        public string ServiceName { get; set; }
    }
}
