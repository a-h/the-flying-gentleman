using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// Represents the operation of copying a file from one location to another.  It is
	/// functionally equivalent to the copy operation.
	/// </summary>
	public class CopyOperation : OverwriteOperation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CopyOperation"/> class.
		/// </summary>
		public CopyOperation()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverwriteOperation"/> class.
		/// </summary>
		/// <param name="from">The source file location.</param>
		/// <param name="to">The target file location.</param>
		public CopyOperation(string from, string to)
		{
			this.From = from;
			this.TargetPath = to;
		}
	}
}
