using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WebServer
{
	/// <summary>
	/// The settings used to configure an IIS application pool.
	/// </summary>
	[DataContract]
	public class ApplicationPoolSettings
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationPoolSettings"/> class.
		/// </summary>
		public ApplicationPoolSettings()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationPoolSettings"/> class.
		/// </summary>
		/// <param name="applicationPoolName">Name of the application pool.</param>
		/// <param name="runtime">The runtime.</param>
		/// <param name="pipelineMode">The pipeline mode.</param>
		/// <param name="identityType">Type of the identity.</param>
		public ApplicationPoolSettings(string applicationPoolName, Runtime runtime, PipelineMode pipelineMode, IdentityType identityType)
		{
			this.ApplicationPoolName = applicationPoolName;
			this.Runtime = runtime;
			this.PipelineMode = pipelineMode;
			this.IdentityType = identityType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationPoolSettings"/> class.
		/// </summary>
		/// <param name="applicationPoolName">Name of the application pool.</param>
		/// <param name="runtime">The runtime.</param>
		/// <param name="pipelineMode">The pipeline mode.</param>
		/// <param name="identityType">Type of the identity.</param>
        /// <param name="idleTimeout">The Idle-Timeout in minutes.</param>
        public ApplicationPoolSettings(string applicationPoolName, Runtime runtime, PipelineMode pipelineMode, IdentityType identityType, int idleTimeout)
            : this(applicationPoolName, runtime, pipelineMode, identityType)
		{
            this.IdleTimeout = idleTimeout;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPoolSettings"/> class.
        /// </summary>
        /// <param name="applicationPoolName">Name of the application pool.</param>
        /// <param name="runtime">The runtime.</param>
        /// <param name="pipelineMode">The pipeline mode.</param>
		/// <param name="windowsUserName">Name of the windows user.</param>
		/// <param name="windowsPassword">The windows password.</param>
		public ApplicationPoolSettings(string applicationPoolName, Runtime runtime, PipelineMode pipelineMode, string windowsUserName, string windowsPassword)
			: this(applicationPoolName, runtime, pipelineMode, IdentityType.SpecificUser)
		{
			this.WindowsUserName = windowsUserName;
			this.WindowsPassword = windowsPassword;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationPoolSettings"/> class.
		/// </summary>
		/// <param name="applicationPoolName">Name of the application pool.</param>
		/// <param name="runtime">The runtime.</param>
		/// <param name="pipelineMode">The pipeline mode.</param>
		/// <param name="windowsUserName">Name of the windows user.</param>
		/// <param name="windowsPassword">The windows password.</param>
        /// <param name="idleTimeout">The Idle-Timeout in minutes.</param>
        public ApplicationPoolSettings(string applicationPoolName, Runtime runtime, PipelineMode pipelineMode, string windowsUserName, string windowsPassword, int idleTimeout)
            : this(applicationPoolName, runtime, pipelineMode, windowsUserName, windowsPassword)
		{
            this.IdleTimeout = idleTimeout;
		}

		/// <summary>
		/// The .Net Runtime version to use for the Application Pool.
		/// </summary>
		[DataMember]
		public Runtime Runtime { get; set; }

		/// <summary>
		/// The pipeline mode to for the Application Pool.
		/// </summary>
		[DataMember]
		public PipelineMode PipelineMode { get; set; }

		/// <summary>
		/// The name of the Application Pool.
		/// </summary>
		[DataMember]
		public string ApplicationPoolName { get; set; }

		/// <summary>
		/// The identity type of the Application Pool, e.g. LocalSystem, or Specific User.
		/// </summary>
		[DataMember]
		public IdentityType IdentityType { get; set; }

		/// <summary>
		/// The username of the Windows user who is running the Application Pool.  Used when the
		/// IdentityType is set to IdentityType.SpecificUser.
		/// </summary>
		[DataMember]
		public string WindowsUserName { get; set; }

		/// <summary>
		/// The password of the Windows User who can run the Application Pool.  Used when the
		/// IdentityType is set to IdentityType.SpecificUser.
		/// </summary>
		[DataMember]
		public string WindowsPassword { get; set; }

        /// <summary>
        /// Amount of time (in minutes) a worker process will remain idle before it shuts down.
        /// A worker process is idle if it is not processing requests and no new requests are received.
        /// </summary>
        [DataMember]
        public int IdleTimeout { get; set; }
    }
}
