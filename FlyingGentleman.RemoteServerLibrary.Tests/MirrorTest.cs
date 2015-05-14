using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FlyingGentleman.Library.FileSystem.Operations;
using FlyingGentleman.Library.ExtensionMethods;
using FlyingGentleman.Library.FileSystem;
using System.Text.RegularExpressions;

namespace FlyingGentleman.RemoteServerLibrary.Tests
{
	[TestFixture]
	public class MirrorTest
	{
		[Test]
		public void TheFileSystemEntryIsSerializableToJson()
		{
			var fse = new FileSystemEntry() { IsDirectory = false, Path = "C:\\Test\\Test.html" };
			Assert.That(fse.ToJson(), Is.Not.Empty);
		}

		[Test]
		public void SourceDirectoriesAndFilesAreProperlyIgnored()
		{
			// Arrange.
			var settings = new MirrorSettings();

			var sourceFiles = new List<FileSystemEntry>();
			var targetFiles = new List<FileSystemEntry>();

			string sourcePath = "C:\\inetpub\\wwwroot\\";
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Web.Config", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Default.aspx", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\", true));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\Icon.png", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\Update.png", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\EditUser.aspx", false));

			settings.IgnoreSourceDirectoryPatterns.Add(new IgnoreExpression(@"^.+\\Images\\$", false));
			settings.IgnoreSourceFileNamePatterns.Add(new IgnoreExpression(@"^.+\\web.config$", false));

			string targetPath = "F:\\inetpub\\wwwroot\\";
			targetFiles.Add(new FileSystemEntry("F:\\inetpub\\wwwroot\\", "F:\\inetpub\\wwwroot\\App_Offline.htm", false));
			targetFiles.Add(new FileSystemEntry("F:\\inetpub\\wwwroot\\", "F:\\inetpub\\wwwroot\\App_Data\\", true));

			settings.IgnoreTargetDirectoryPatterns.Add(new IgnoreExpression(@"^.+\\app_data\\$", false));
			settings.IgnoreTargetFileNamePatterns.Add(new IgnoreExpression(@"^.+\\app_offline.htm$", false));

			// Act.
			List<IFileOperation> fileOperations = FileSystem.FileSystem.CalculateOperations(sourcePath, sourceFiles, targetPath, targetFiles, settings);

			// Assert.

			// Because I've ignored the Images directory, the following files shouldn't be copied across:
			// C:\\inetpub\\wwwroot\\Images\\
			// C:\\inetpub\\wwwroot\\Images\\Icon.png
			// C:\\inetpub\\wwwroot\\Images\\Update.png
			// Because I've ignored the web.config, that shouldn't be copied either.

			// In this case, I should only copy the Default.aspx file and the EditUser.aspx file.

			// However, normally, the Mirror would remove the App_Offline.htm file and App_Data folders.
			// but I've added target exclusions, so they should be left alone.

			Assert.That(fileOperations.Count, Is.EqualTo(8));
			Assert.That(fileOperations.OfType<CopyOperation>().Count(), Is.EqualTo(2));
			Assert.That(fileOperations.OfType<IgnoreOperation>().Count(), Is.EqualTo(6));
			
			// Check that the copy options are as expected.
			var copyOperations = fileOperations.OfType<CopyOperation>().ToList();
			Assert.That(copyOperations[0], Is.InstanceOf<CopyOperation>());
			Assert.That((copyOperations[0] as CopyOperation).From, Is.EqualTo("C:\\inetpub\\wwwroot\\Default.aspx"));
			Assert.That((copyOperations[0] as CopyOperation).TargetPath, Is.EqualTo("F:\\inetpub\\wwwroot\\Default.aspx"));

			Assert.That(copyOperations[1], Is.InstanceOf<CopyOperation>());
			Assert.That((copyOperations[1] as CopyOperation).From, Is.EqualTo("C:\\inetpub\\wwwroot\\EditUser.aspx"));
			Assert.That((copyOperations[1] as CopyOperation).TargetPath, Is.EqualTo("F:\\inetpub\\wwwroot\\EditUser.aspx"));

			// Check that the appropriate files and directories have been ignored.
			var ignoreOperations = fileOperations.OfType<IgnoreOperation>().ToList();
			Assert.That(ignoreOperations.First().TargetPath, Is.EqualTo("C:\\inetpub\\wwwroot\\Images\\"));
			Assert.That(ignoreOperations.Skip(1).First().TargetPath, Is.EqualTo("C:\\inetpub\\wwwroot\\Images\\Icon.png"));
			Assert.That(ignoreOperations.Skip(2).First().TargetPath, Is.EqualTo("C:\\inetpub\\wwwroot\\Images\\Update.png"));
			Assert.That(ignoreOperations.Skip(3).First().TargetPath, Is.EqualTo("C:\\inetpub\\wwwroot\\Web.Config"));
			Assert.That(ignoreOperations.Skip(4).First().TargetPath, Is.EqualTo("F:\\inetpub\\wwwroot\\App_Data\\"));
			Assert.That(ignoreOperations.Skip(5).First().TargetPath, Is.EqualTo("F:\\inetpub\\wwwroot\\App_Offline.htm"));
		}

		[Test]
		public void FilesAndDirectoriesPresentInTheSourceButNotPresentInTheTargetWillBeCopiedToTheTarget()
		{
			// Arrange.
			var settings = new MirrorSettings();

			var sourceFiles = new List<FileSystemEntry>();
			var targetFiles = new List<FileSystemEntry>();

			string sourcePath = "C:\\inetpub\\wwwroot\\";
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Web.Config", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Default.aspx", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\", true));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\Icon.png", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\Update.png", false));
			sourceFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\EditUser.aspx", false));

			sourceFiles = sourceFiles.OrderBy(sf => sf.Path).ToList();

			string targetPath = "F:\\inetpub\\wwwroot\\";

			// Act.
			List<IFileOperation> fileOperations = FileSystem.FileSystem.CalculateOperations(sourcePath, sourceFiles, targetPath, targetFiles, settings);

			// Assert.
			Assert.That(fileOperations.Count, Is.EqualTo(sourceFiles.Count));
			Assert.That(fileOperations[0], Is.InstanceOf<CopyOperation>());
			Assert.That((fileOperations[0] as CopyOperation).TargetPath, Is.EqualTo("F:\\inetpub\\wwwroot\\Default.aspx"));
		}

		[Test]
		public void FilesAndDirectoriesPresentInTheTargetButNotPresentInTheSourceWillBeDeleted()
		{
			// Arrange.
			var settings = new MirrorSettings();

			var sourceFiles = new List<FileSystemEntry>();
			var targetFiles = new List<FileSystemEntry>();

			string sourcePath = "F:\\inetpub\\wwwroot\\";

			string targetPath = "C:\\inetpub\\wwwroot\\";
			targetFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Web.Config", false));
			targetFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Default.aspx", false));
			targetFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\", true));
			targetFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\Icon.png", false));
			targetFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\Images\\Update.png", false));
			targetFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\EditUser.aspx", false));
			targetFiles.Add(new FileSystemEntry(sourcePath, "C:\\inetpub\\wwwroot\\App_Offline.htm", false));

			settings.IgnoreTargetFileNamePatterns.Add(new IgnoreExpression(@"^.+\\app_offline.htm$", false));

			// Act.
			List<IFileOperation> fileOperations = FileSystem.FileSystem.CalculateOperations(sourcePath, sourceFiles, targetPath, targetFiles, settings);

			// All of the files in the F:\\inetpub\wwwroot\ directory should be deleted.  Apart from App_Offline.htm.
			// Assert.
			Assert.That(fileOperations.Count, Is.EqualTo(7));
			// Ignore the App_Offline.htm in the target directory.
			Assert.That(fileOperations.OfType<IgnoreOperation>().Count(), Is.EqualTo(1));
			// Delete the Images directory.
			Assert.That(fileOperations.OfType<DeleteDirectoryOperation>().Count(), Is.EqualTo(1));
			// Delete the other files.
			Assert.That(fileOperations.OfType<DeleteFileOperation>().Count(), Is.EqualTo(5));
		}
	}
}