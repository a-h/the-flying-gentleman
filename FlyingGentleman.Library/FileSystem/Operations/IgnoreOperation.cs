using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// An operation which ignores the file due to an ignore pattern match.
	/// </summary>
	public class IgnoreOperation : IFileOperation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IgnoreOperation"/> class.
		/// </summary>
		public IgnoreOperation()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IgnoreOperation"/> class.
		/// </summary>
		/// <param name="targetPath">The path to ignore.</param>
		public IgnoreOperation(string targetPath)
		{
			this.TargetPath = targetPath;
		}

		/// <summary>
		/// Gets or sets the path which will be ignored.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string TargetPath { get; set; }

		/// <summary>
		/// Executes this instance.
		/// </summary>
		public void Execute()
		{
			// Do nothing.
		}
	}
}
