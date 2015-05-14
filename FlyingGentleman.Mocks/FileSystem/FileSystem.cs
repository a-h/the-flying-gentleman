using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.FileSystem;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;
using System.IO;

namespace FlyingGentleman.Mocks.FileSystem
{
	/// <summary>
	/// Provides mock File System interactions for testing deployments.
	/// </summary>
	public class FileSystem : MockBase, IFileSystem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystem"/> class.
		/// </summary>
		/// <param name="a">The action to execute when a log event is created (e.g. write it to the console).</param>
		public FileSystem(Action<LogEvent> a)
		{
			this.Action = a;
		}

		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		public void Delete(string fileName)
		{
			LogEventHelper.CreateEvent(this.Action, this, "Delete", fileName);
		}

		/// <summary>
		/// Renames the specified file.
		/// </summary>
		/// <param name="originalName">The original file name.</param>
		/// <param name="newName">The new name.</param>
		public void Rename(string originalName, string newName)
		{
			LogEventHelper.CreateEvent(this.Action, this, "Rename", string.Format("From {0} to {1}", originalName, newName));
		}

		/// <summary>
		/// Copies a single file from the source location to the target location.
		/// Overwriting happens by default.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="targetPath">The target path.</param>
		/// <param name="overwrite">Set to true to allow overwriting an existing target file.</param>
		public void CopyFile(string sourcePath, string targetPath, bool overwrite)
		{
			LogEventHelper.CreateEvent(this.Action, this, "Copy", string.Format("From {0} to {1} (overwriting existing file).", sourcePath, targetPath, overwrite));
		}

		/// <summary>
		/// Mirrors a directory structure.  This operation is able to delete files on the
		/// target path.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="targetPath">The target path.</param>
		/// <param name="settings">Any mirroring settings.</param>
		public void Mirror(string sourcePath, string targetPath, MirrorSettings settings)
		{
			LogEventHelper.CreateEvent(this.Action, this, "Mirror", string.Format("From {0} to {1}.  {2}", sourcePath, targetPath, settings.ToJson()));
		}

		/// <summary>
		/// Creates a file on disk.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="targetPath">The target path.</param>
		public void CreateFile(byte[] data, string targetPath)
		{
			LogEventHelper.CreateEvent(this.Action, this, "Create File", string.Format("Copying a stream of {0} bytes to {1}.", data.Length, targetPath));
		}


        public void TransformConfigFile(string baseConfigPath, string transformConfigPath, string targetPath)
        {
            LogEventHelper.CreateEvent(this.Action, this, "Transform Config File", string.Format("Transforming config at '{0}' with transform '{1}' to path {2}'.", baseConfigPath, transformConfigPath, targetPath));
        }
    }
}
