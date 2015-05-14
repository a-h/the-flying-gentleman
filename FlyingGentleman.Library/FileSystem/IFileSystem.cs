using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using System.IO;
using System.ServiceModel;

namespace FlyingGentleman.Library.FileSystem
{
	/// <summary>
	/// Provides methods to interact with the File System of the server target.
	/// </summary>
	[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILogEventCreator))]
	public interface IFileSystem
	{
		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		[OperationContract]
		void Delete(string fileName);

		/// <summary>
		/// Renames the specified file.
		/// </summary>
		/// <param name="originalName">The original file name.</param>
		/// <param name="newName">The new name.</param>
		[OperationContract]
		void Rename(string originalName, string newName);

		/// <summary>
		/// Creates a file on disk.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="targetPath">The target path.</param>
		[OperationContract]
		void CreateFile(byte[] data, string targetPath);

		/// <summary>
		/// Copies a single file from the source location to the target location.  
		/// Overwriting happens by default.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="targetPath">The target path.</param>
		/// <param name="overwrite">Set to true to allow overwriting an existing target file.</param>
		[OperationContract]
		void CopyFile(string sourcePath, string targetPath, bool overwrite);

		/// <summary>
		/// Mirrors a directory structure.  This operation is able to delete files on the
		/// target path.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="targetPath">The target path.</param>
		/// <param name="settings">Any mirroring settings.</param>
		[OperationContract]
		void Mirror(string sourcePath, string targetPath, MirrorSettings settings);

        /// <summary>
        /// Peforms a config transformation for the web/app config at the base path 
        /// </summary>
        /// <param name="baseConfigPath">Path to base config</param>
        /// <param name="transformConfigPath">Path to config transformation file</param>
        /// <param name="targetPath">Path to copy resultant config to</param>
        [OperationContract]
        void TransformConfigFile(string baseConfigPath, string transformConfigPath, string targetPath);
	}
}
