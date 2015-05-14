using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// Deletes a directory on the target machine.
	/// </summary>
	public class DeleteDirectoryOperation : IFileOperation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteDirectoryOperation"/> class.
		/// </summary>
		public DeleteDirectoryOperation()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateDirectoryOperation"/> class.
		/// </summary>
		/// <param name="targetPath">The name of the target directory which will be deleted.</param>
		public DeleteDirectoryOperation(string targetPath)
		{
			this.TargetPath = targetPath;
		}

		/// <summary>
		/// Executes the operation.
		/// </summary>
		public void Execute()
		{
			if (Directory.GetFileSystemEntries(this.TargetPath).Any())
			{
				throw new IOException(string.Format("It is not possible to delete a directory unless it is empty.  Please delete the contents of the \"{0}\" directory.", this.TargetPath));
			}

			Directory.Delete(this.TargetPath);
		}

		/// <summary>
		/// Gets or sets the name of the directory which will be deleted on the server.
		/// </summary>
		/// <value>
		/// The name of the target directory.
		/// </value>
		public string TargetPath { get; set; }
	}
}
