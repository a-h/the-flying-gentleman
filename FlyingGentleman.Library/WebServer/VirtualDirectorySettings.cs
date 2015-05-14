using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WebServer
{
	/// <summary>
	/// Settings to use when creating a Virtual Directory within IIS.
	/// </summary>
	[DataContract]
	public class VirtualDirectorySettings
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualDirectorySettings"/> class.
		/// </summary>
		public VirtualDirectorySettings()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualDirectorySettings"/> class.
        /// </summary>
        /// <param name="websiteName">The name of the website.</param>
        /// <param name="name">The name of the Virtual Directory, e.g. "/".</param>
        /// <param name="virtualDirectoryPath">The physical path of the virtual directory .</param>
        /// <param name="applicationPoolName">The name of the application pool which the virtual directory should belong to.</param>
        /// <param name="authenticationAllowAnonymousAuthentication">Whether or not to allow anonymous authentication in IIS.</param>
        /// <param name="authenticationMethod">The authentication method.</param>
        public VirtualDirectorySettings(string websiteName, string name, string virtualDirectoryPath, string applicationPoolName, bool authenticationAllowAnonymousAuthentication, AuthenticationMethod authenticationMethod)
		{
			this.Name = name;
			this.Path = virtualDirectoryPath;
			this.ApplicationPool = applicationPoolName;
			this.WebsiteName = websiteName;
            this.AuthenticationAllowAnonymousAuthentication = authenticationAllowAnonymousAuthentication;
            this.AuthenticationMethod = authenticationMethod;
		}

		/// <summary>
		/// The name of the Website to setup the virtual directory in, e.g. "Default Web Site".
		/// </summary>
		[DataMember]
		public string WebsiteName { get; set; }

		/// <summary>
		/// The name of the virtual directory, e.g. "WebServiceV4" or "WebServiceV4/Elmah"
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// The physical path to the Virtual Directory, e.g. C:\\inetpub\\wwwroot\Vdir\\
		/// </summary>
		[DataMember]
		public string Path { get; set; }

		/// <summary>
		/// The name of the Application Pool to use.
		/// </summary>
		[DataMember]
		public string ApplicationPool { get; set; }

        /// <summary>
        /// Whether or not Anonymous Authentication is enabled for the web application.
        /// Anonymous Authentication is independent to the AuthenticationMethod.
        /// </summary>
        [DataMember]
        public bool AuthenticationAllowAnonymousAuthentication { get; set; }

        /// <summary>
        /// The authentication method for the web application.
        /// It can be either Forms Authentication, Windows Authentication or None.
        /// It can not be both (Challenge based auth and login redirect based cannot be used simultaneously).
        /// </summary>
        [DataMember]
        public AuthenticationMethod AuthenticationMethod { get; set; }
	}
}
