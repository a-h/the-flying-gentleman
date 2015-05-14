using System;
using System.Collections.Generic;
using System.Linq;
using FlyingGentleman.Library.FileSystem;
using FlyingGentleman.Library;
using FlyingGentleman.Library.ExtensionMethods;
using System.IO;
using FlyingGentleman.Library.FileSystem.Operations;
using System.Threading;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace FlyingGentleman.RemoteServerLibrary.FileSystem
{
	/// <summary>
	/// Provides methods to interact with the File System of the server target.
	/// </summary>
	[Serializable]
	public class FileSystem : LogEventCreatorBase, IFileSystem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystem"/> class.
		/// </summary>
		public FileSystem()
		{
		}

		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		public void Delete(string fileName)
		{
			LogEventHelper.CreateEvent(this, "Delete", fileName);
			File.Delete(fileName);
		}

		/// <summary>
		/// Renames the specified file.
		/// </summary>
		/// <param name="originalName">The original file name.</param>
		/// <param name="newName">The new name.</param>
		public void Rename(string originalName, string newName)
		{
			LogEventHelper.CreateEvent(this, "Rename", string.Format("From {0} to {1}", originalName, newName));
			CopyFileUsingStreams(originalName, newName);
			File.Delete(originalName);
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
			LogEventHelper.CreateEvent(this, "Copy", string.Format("From {0} to {1}", sourcePath, targetPath));

            if (!overwrite && File.Exists(targetPath))
            {
                throw new InvalidOperationException(string.Format("You can't overwrite {0} with {1}", targetPath, sourcePath));
            }

            CopyFileUsingStreams(sourcePath, targetPath);
		}

        private static void CopyFileUsingStreams(string sourcePath, string targetPath)
        {
            if (sourcePath != targetPath)
            {
                using (var target = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
                {
                    using (var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                    {
                        source.CopyTo(target);
                    }
                }
            }
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
			LogEventHelper.CreateEvent(this, "Mirror", string.Format("From {0} to {1} with settings {2}", sourcePath, targetPath, settings.ToJson()));

			List<FileSystemEntry> sourceFiles = Directory.GetFileSystemEntries(sourcePath, "*.*", SearchOption.AllDirectories).Select(fse => new FileSystemEntry(sourcePath, fse)).ToList();

			// Create the target directory if it doesn't exist.
			if (!Directory.Exists(targetPath))
			{
				Directory.CreateDirectory(targetPath);
			}

            if (settings.RetryCount == 0)
            {
                settings.RetryCount = 5;
                settings.RetrySleepSeconds = 2;
            }
            var attempts = 0;
            while (attempts < settings.RetryCount)
            {
                try
                {
                    List<FileSystemEntry> targetFiles = Directory.GetFileSystemEntries(targetPath, "*.*", SearchOption.AllDirectories).Select(fse => new FileSystemEntry(targetPath, fse)).ToList();

                    List<IFileOperation> operations = CalculateOperations(sourcePath, sourceFiles, targetPath, targetFiles, settings);

                    // Delete files first.
                    foreach (var operation in operations.OfType<DeleteFileOperation>())
                    {
                        LogEventHelper.CreateEvent(this, "Mirror:" + operation.GetType().Name, operation.ToJson());
                        operation.Execute();
                    }

                    // Then delete directories.
                    foreach (var operation in operations.OfType<DeleteDirectoryOperation>().OrderByDescending(o => o.TargetPath.Length))
                    {
                        LogEventHelper.CreateEvent(this, "Mirror:" + operation.GetType().Name, operation.ToJson());
                        operation.Execute();
                    }

                    // Then create new directories.
                    foreach (var operation in operations.OfType<CreateDirectoryOperation>().OrderBy(o => o.TargetPath))
                    {
                        LogEventHelper.CreateEvent(this, "Mirror:" + operation.GetType().Name, operation.ToJson());
                        operation.Execute();
                    }

                    // Then create new files.
                    foreach (var operation in operations.OfType<CopyOperation>().OrderBy(o => o.TargetPath))
                    {
                        LogEventHelper.CreateEvent(this, "Mirror:" + operation.GetType().Name, operation.ToJson());
                        operation.Execute();
                    }

                    // Update existing files.
                    foreach (var operation in operations.OfType<OverwriteOperation>().OrderBy(o => o.TargetPath))
                    {
                        LogEventHelper.CreateEvent(this, "Mirror:" + operation.GetType().Name, operation.ToJson());
                        operation.Execute();
                    }

                    // Log ignore operations.
                    foreach (var operation in operations.OfType<IgnoreOperation>())
                    {
                        LogEventHelper.CreateEvent(this, "Mirror:" + operation.GetType().Name, operation.ToJson());
                        operation.Execute();
                    }

                    break;
                }
                catch (Exception e)
                {
                    if (++attempts == settings.RetryCount)
                    {   
                        throw;
                    }
                    LogEventHelper.CreateEvent(this, "Mirror", string.Format("{0}\r\nWill sleep for {1} seconds and then retry {2} time(s).", e, settings.RetrySleepSeconds, settings.RetryCount - attempts));
                    Thread.Sleep(1000 * settings.RetrySleepSeconds);                    
                }
            }

            LogEventHelper.CreateEvent(this, "Mirror", string.Format("Mirror from {0} to {1} complete after {2} retries.", sourcePath, targetPath, attempts));
		}

		/// <summary>
		/// Calculates the operations required to mirror two directories.
		/// </summary>
		/// <param name="sourcePath">The base source path e.g. \\server1\c$\inetpub\wwwroot\.</param>
		/// <param name="sourceEntries">The source entries.</param>
		/// <param name="targetPath">The target path e.g. C:\inetpub\wwwroot\</param>
		/// <param name="targetEntries">The target entries.</param>
		/// <param name="settings">The settings.</param>
		/// <returns></returns>
		public static List<IFileOperation> CalculateOperations(string sourcePath, List<FileSystemEntry> sourceEntries, string targetPath, List<FileSystemEntry> targetEntries, MirrorSettings settings)
		{
			var returnValue = new List<IFileOperation>();

			sourceEntries = sourceEntries.OrderBy(s => s.Path).ToList();
			targetEntries = targetEntries.OrderBy(s => s.Path).ToList();

			// Ignore files which belong to ignored source and target directories.
			// These will result in "IgnoreOperations".
			returnValue.AddRange(IgnoreFileSystemEntries(sourceEntries, settings.IgnoreSourceDirectoryPatterns, settings.IgnoreSourceFileNamePatterns));
			returnValue.AddRange(IgnoreFileSystemEntries(targetEntries, settings.IgnoreTargetDirectoryPatterns, settings.IgnoreTargetFileNamePatterns));

			foreach (var sourceEntry in sourceEntries)
			{
				if (sourceEntry.IsDirectory)
				{
					// Find directories which exist in the source but don't exist in the target.  These directories will be created.
					if (!targetEntries.Where(te => te.IsDirectory && string.Equals(te.RelativePath, sourceEntry.RelativePath, StringComparison.InvariantCultureIgnoreCase)).Any())
					{
						returnValue.Add(new CreateDirectoryOperation(Path.Combine(targetPath, sourceEntry.RelativePath)));
					}
				}
				else
				{
					// Find files which exist in the source and the target.  These will be updated in the target.
					if (targetEntries.Where(te => !te.IsDirectory && string.Equals(te.RelativePath, sourceEntry.RelativePath, StringComparison.InvariantCultureIgnoreCase)).Any())
					{
						returnValue.Add(new OverwriteOperation(sourceEntry.Path, Path.Combine(targetPath, sourceEntry.RelativePath)));
					}
					else
					{
						// Copy the new file to target.
						returnValue.Add(new CopyOperation(sourceEntry.Path, Path.Combine(targetPath, sourceEntry.RelativePath)));
					}
				}
			}

			// Find directories which exist in the target, but don't exist in the source.  These will be removed from the target.	
			var directoriesToRemove = targetEntries.Where(te => te.IsDirectory && !sourceEntries.Any(se => se.RelativePath.Equals(te.RelativePath) && se.IsDirectory)).ToList();
			returnValue.AddRange(directoriesToRemove.Select(d => new DeleteDirectoryOperation(d.Path)));

			// Find files which exist in the target, but don't exist in the source.  These will be removed from the target.
			var filesToRemove = targetEntries.Where(te => !te.IsDirectory && !sourceEntries.Any(se => se.RelativePath.Equals(te.RelativePath) && !se.IsDirectory)).ToList();
			returnValue.AddRange(filesToRemove.Select(f => new DeleteFileOperation(f.Path)));

			return returnValue;
		}

		/// <summary>
		/// Removes file system entries which should not be processed by the mirror operation from the inpust list
		/// of File System Entries and returns ignore operations for them.
		/// </summary>
		/// <param name="sourceEntries">The source entries.</param>
		/// <param name="ignoreDirectoryPatterns">The ignore directory patterns.</param>
		/// <param name="ignoreFileNamePatterns">The ignore file name patterns.</param>
		/// <returns>A list of file operations to use for the ignored files.</returns>
		private static List<IFileOperation> IgnoreFileSystemEntries(List<FileSystemEntry> sourceEntries, List<IgnoreExpression> ignoreDirectoryPatterns, List<IgnoreExpression> ignoreFileNamePatterns)
		{
			var operations = new List<IFileOperation>();

			// Make a list of ignored directories.
			var directoriesToIgnore = sourceEntries.Where(se => se.IsDirectory && ignoreDirectoryPatterns.Any(r => r.IsMatch(se.Path))).Select(se => se.Path).ToList();

			// Remove files and directories which are present in the ignored directories.
			var filesToRemove = sourceEntries.Where(se => directoriesToIgnore.Any(d => se.Path.StartsWith(d)));
			operations.AddRange(sourceEntries.Where(r => filesToRemove.Contains(r)).Select(se => new IgnoreOperation(se.Path)));
			sourceEntries.RemoveAll(r => filesToRemove.Contains(r));

			// Remove files which should be ignored.
			var filesToIgnore = sourceEntries.Where(se => !se.IsDirectory && ignoreFileNamePatterns.Any(r => r.IsMatch(se.Path))).ToList();
			operations.AddRange(filesToIgnore.Select(se => new IgnoreOperation(se.Path)));
			foreach (var toIgnore in filesToIgnore)
			{
				sourceEntries.Remove(toIgnore);
			}

			return operations;
		}

		/// <summary>
		/// Creates a file on disk.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="targetPath">The target path.</param>
		public void CreateFile(byte[] data, string targetPath)
		{
			LogEventHelper.CreateEvent(this, "Create File", string.Format("Copying a stream of {0} bytes to {1}.", data.Length, targetPath));
			using (var fs = new FileStream(targetPath, FileMode.Create))
			{
				fs.Write(data, 0, data.Length);
			}
		}

        /// <summary>
        /// Peforms a config transformation for the web/app config at the base path 
        /// </summary>
        /// <param name="baseConfigPath">Path to base config</param>
        /// <param name="transformConfigPath">Path to config transformation file</param>
        /// <param name="targetPath">Path to copy resultant config to</param>
        public void TransformConfigFile(string baseConfigPath, string transformConfigPath, string targetPath)
        {
            using (var baseStream = File.OpenRead(baseConfigPath))
            {
                using (var transformStream = File.OpenRead(transformConfigPath))
                {
                    XmlDocument doc = TransformConfigFile(baseStream, transformStream);
                    doc.Save(targetPath);
                }
            }
        }

        /// <summary>
        /// Transforms a configuration file
        /// </summary>
        /// <param name="baseConfig">Base config file stream</param>
        /// <param name="transformConfig">Config transformation file stream</param>
        /// <returns>Result XDocument</returns>
        internal XmlDocument TransformConfigFile(Stream baseConfig, Stream transformConfig)
        {
            var doc = new XmlDocument();
            doc.Load(baseConfig);

            Action<string> logAction = s => LogEventHelper.CreateEvent(this, "TransformConfigFile", s);
            var xmlTransformLogger = new XmlTransformLogger(logAction);

            using (var transformation = new XmlTransformation(transformConfig, xmlTransformLogger))
            {
                transformation.Apply(doc);
            }

            return doc;
        }
    }
}
