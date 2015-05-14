using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WebServer
{
	/// <summary>
	/// Settings used when creating an IIS Website.
	/// </summary>
	[DataContract]
	public class WebsiteSettings
	{
		/// <summary>
		/// Creates an instance of WebsiteSettings.
		/// </summary>
		public WebsiteSettings()
			: this("Default Web Site", @"C:\inetpub\wwwroot\", @"C:\inetpub\logs\LogFiles\")
		{
		}

		/// <summary>
		/// Creates an instance of Website settings.
		/// </summary>
		/// <param name="name">The name of the Website, e.g. "Default Website".</param>
		/// <param name="path">The path to the Website on the server e.g. C:\inetpub\wwwroot\.</param>
		/// <param name="logFileDirectory">The name of the directory which will contain the IIS logs, e.g.  C:\inetpub\logs\LogFiles\</param>
		public WebsiteSettings(string name, string path, string logFileDirectory)
		{
			this.WebsiteName = name;
			this.Path = path;
			this.LogFileDirectory = logFileDirectory;
		}

		/// <summary>
		/// The name of the Website, e.g. "Default Web Site".
		/// </summary>
		[DataMember]
		public string WebsiteName { get; set; }

		/// <summary>
		/// The path of the Website root, e.g. "C:\\inetpub\wwwroot\".
		/// </summary>
		[DataMember]
		public string Path { get; set; }

		/// <summary>
		/// The directory where log files are stored, e.g. "E:\Logs\";
		/// </summary>
		[DataMember]
		public string LogFileDirectory { get; set; }

		/// <summary>
		/// The thumbprint of the SSL certificate to get from the local SSL certificate store.
		/// </summary>
		[DataMember]
		public string SslThumbprint { get; set; }

		/// <summary>
		/// The port for the Website. Will use default of 80 if null.
		/// </summary>
		[DataMember]
	    public int? Port { get; set; }
	}
}
