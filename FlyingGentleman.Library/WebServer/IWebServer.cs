using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FlyingGentleman.Library.WebServer
{
	/// <summary>
	/// Allows the setup and configuration of IIS 7.0.
	/// </summary>
	[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILogEventCreator))]
	public interface IWebServer
	{
		/// <summary>
		/// Installs IIS on the Server.
		/// </summary>
		[OperationContract]
		void Install();
		
        /// <summary>
        /// Installs MVC3 on the server
        /// </summary>
        [OperationContract]
        void InstallMvc3();
        
        /// <summary>
        /// Installs support for hosting WCF service applications
        /// </summary>
        [OperationContract]
        void InstallWcfSupport();

		/// <summary>
		/// Sets up an IIS Website and Appropriate bindings for the Server.
		/// </summary>
		/// <param name="settings">The Website settings to use.</param>
		[OperationContract]
		void SetupWebsite(WebsiteSettings settings);

		/// <summary>
		/// Sets up an Application Pool within IIS.
		/// </summary>
		/// <param name="settings"></param>
		[OperationContract]
		void SetupApplicationPool(ApplicationPoolSettings settings);

		/// <summary>
		/// Sets up a Virtual Directory within an existing Website.
		/// </summary>
		/// <param name="settings">The Virtual Directory Settings.</param>
		[OperationContract]
		void SetupVirtualDirectory(VirtualDirectorySettings settings);

        /// <summary>
        /// Restarts an app pool on the remote server
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns>Wheher it succeeded</returns>
	    [OperationContract]
        [Obsolete("Use Start and Stop methods instead")]
	    bool RestartAppPool(string appPoolName);

        /// <summary>
        /// Attemts to Stop the specified Application Pool within a timeout duration
        /// </summary>
        /// <param name="appPoolName">ApplicationPool to Stop</param>
        /// <param name="maximumWaitSeconds">Timeout duration</param>
        [OperationContract]
        void StopAppPool(string appPoolName, int maximumWaitSeconds);

        /// <summary>
        /// Attemts to Start the specified Application Pool within a timeout duration
        /// </summary>
        /// <param name="appPoolName">ApplicationPool to Start</param>
        /// <param name="maximumWaitSeconds">Timeout duration</param>
        [OperationContract]
        void StartAppPool(string appPoolName, int maximumWaitSeconds);
	}
}
