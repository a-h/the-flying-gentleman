using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// Deletes a file from the disk.
	/// </summary>
	public class DeleteFileOperation : IFileOperation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteFileOperation"/> class.
		/// </summary>
		public DeleteFileOperation()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteFileOperation"/> class.
		/// </summary>
		/// <param name="fileName">The name of the file to be deleted.</param>
		public DeleteFileOperation(string fileName)
		{
			this.TargetPath = fileName;
		}

		/// <summary>
		/// Gets or sets the name of the file which will be deleted.
		/// </summary>
		/// <value>
		/// The name of the file which will be deleted.
		/// </value>
		public string TargetPath { get; set; }

		/// <summary>
		/// Executes this instance.
		/// </summary>
		public void Execute()
		{
			File.Delete(this.TargetPath);
		}
	}
}
