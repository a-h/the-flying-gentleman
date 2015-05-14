using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FlyingGentleman.Library.WindowsService
{
	/// <summary>
	/// Allows the instllation and control of Windows Services.
	/// </summary>
	[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILogEventCreator))]
	public interface IWindowsService
	{
        /// <summary>
        /// Starts a Windows Service.
        /// </summary>
        /// <param name="name">The name of the Windows Service to Start.</param>
        /// <param name="server">The server.</param>
		[OperationContract]
		void Start(string name, string server = ".");

        /// <summary>
        /// Stops a Windows Service, if it is installed and running
        /// </summary>
        /// <param name="name">The name of the Windows Service to Stop.</param>
        /// <param name="server">The server</param>
		[OperationContract]
        void Stop(string name, string server = ".");

		/// <summary>
		/// Installs a Windows Service.
		/// </summary>
		/// <param name="settings">The settings for the Windows Service.</param>
		[OperationContract]
		void Install(WindowsServiceSettings settings);
	}
}
