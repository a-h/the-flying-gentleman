using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// Overwrites a file on the target server with a file from another location.
	/// </summary>
	public class OverwriteOperation : IFileOperation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverwriteOperation"/> class.
		/// </summary>
		public OverwriteOperation()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverwriteOperation"/> class.
		/// </summary>
		/// <param name="from">The source file location.</param>
		/// <param name="to">The target file location.</param>
		public OverwriteOperation(string from, string to)
		{
			this.From = from;
			this.TargetPath = to;
		}

		/// <summary>
		/// Gets or sets the source file location.
		/// </summary>
		/// <value>
		/// From.
		/// </value>
		public string From { get; set; }

		/// <summary>
		/// Gets or sets the target  file location.
		/// </summary>
		/// <value>
		/// To.
		/// </value>
		public string TargetPath { get; set; }

		/// <summary>
		/// Executes the operation.
		/// </summary>
		public void Execute()
		{
            using (var source = new FileStream(this.From, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var target = new FileStream(this.TargetPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    source.CopyTo(target);
                }
            }
		}
	}
}