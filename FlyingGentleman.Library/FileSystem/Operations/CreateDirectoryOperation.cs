using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// Creates a directory on the target machine.
	/// </summary>
	public class CreateDirectoryOperation : IFileOperation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateDirectoryOperation"/> class.
		/// </summary>
		public CreateDirectoryOperation()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateDirectoryOperation"/> class.
		/// </summary>
		/// <param name="targetPath">Name of the target directory which will be created.</param>
		public CreateDirectoryOperation(string targetPath)
		{
			this.TargetPath = targetPath;
		}

		/// <summary>
		/// Executes the operation.
		/// </summary>
		public void Execute()
		{
			Directory.CreateDirectory(this.TargetPath);	
		}

		/// <summary>
		/// Gets or sets the name of the directory which will be created on the server.
		/// </summary>
		/// <value>
		/// The name of the target directory.
		/// </value>
		public string TargetPath { get; set; }
	}
}
