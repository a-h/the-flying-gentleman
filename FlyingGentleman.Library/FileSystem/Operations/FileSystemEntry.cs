using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// An entry within the file system.
	/// </summary>
	public class FileSystemEntry
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystemEntry"/> class.
		/// </summary>
		public FileSystemEntry()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystemEntry"/> class.
		/// </summary>
		/// <param name="basePath">The base path of the file system entry.</param>
		/// <param name="path">The path of the File System entry..</param>
		public FileSystemEntry(string basePath, string path)
		{
			this.Path = path;
			this.RelativePath = path.Substring(basePath.Length).TrimStart('\\'); //remove any leading strokes and this breaks Path.Combine
		
			// Work out whether the file is a directory or not.
			FileAttributes attributes = File.GetAttributes(path);
			this.IsDirectory = (attributes & FileAttributes.Directory) == FileAttributes.Directory;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystemEntry"/> class.
		/// </summary>
		/// <param name="basePath">The base path of the file system entry.</param>
		/// <param name="path">The path.</param>
		/// <param name="isDirectory">if set to <c>true</c> [is directory].</param>
		public FileSystemEntry(string basePath, string path, bool isDirectory)
		{
			this.Path = path;
			this.RelativePath = path.Substring(basePath.Length);
			this.IsDirectory = isDirectory;
		}

		/// <summary>
		/// Gets or sets the path of the File System entry.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is directory.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is directory; otherwise, <c>false</c>.
		/// </value>
		public bool IsDirectory { get; set; }

		/// <summary>
		/// Gets or sets the path relative to the base path.
		/// </summary>
		/// <value>
		/// The relative path.
		/// </value>
		public string RelativePath { get; set; }
	}
}
