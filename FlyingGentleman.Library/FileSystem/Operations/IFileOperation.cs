using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.FileSystem.Operations
{
	/// <summary>
	/// The common interface for all "Mirror" file operations.
	/// </summary>
	public interface IFileOperation
	{
		/// <summary>
		/// Executes the operation.
		/// </summary>
		void Execute();

		/// <summary>
		/// The path affected by the operation.
		/// </summary>
		string TargetPath { get; set; }
	}
}
