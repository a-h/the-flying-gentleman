using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace FlyingGentleman.RemoteServerLibrary.Tests
{
	[TestFixture(Category="Integration")]
	public class FileSystemIntegrationTests
	{
		[Test]
		public void ItIsPossibleToDeleteFiles()
		{
			// Arrange.
			var temporaryFile = Path.GetTempFileName();

			Assert.That(File.Exists(temporaryFile));

			// Act.
			var fs = new FileSystem.FileSystem();
			fs.Delete(temporaryFile);

			// Assert.
			Assert.That(!File.Exists(temporaryFile));
		}

		[Test]
		public void ItIsPossibleToCopyFiles()
		{
			// Arrange.
			var source = Path.GetTempFileName();
			var target = Path.GetTempFileName();

			try
			{
				Assert.That(File.Exists(source));
				File.Delete(target);
				Assert.That(!File.Exists(target));

				// Act.
				var fs = new FileSystem.FileSystem();
				fs.CopyFile(source, target, true);

				// Assert.
				Assert.That(File.Exists(source));
				Assert.That(File.Exists(target));
			}
			finally
			{
				try
				{
					File.Delete(source);
				}
				finally
				{
					File.Delete(target);
				}
			}
		}

		[Test]
		public void ItIsPossibleToCreateFiles()
		{
			// Arrange.
			var target = Path.GetTempFileName();
			File.Delete(target);
			Assert.That(!File.Exists(target));

			var rnd = new Random();
			var data = new byte[1024 * 50 + 3];
			rnd.NextBytes(data);

			// Act.
			var fs = new FileSystem.FileSystem();
			fs.CreateFile(data, target);

			// Assert.
			Assert.That(File.Exists(target));

			using(var fileStream = new FileStream(target, FileMode.Open))
			{
				Assert.That(fileStream.Length, Is.EqualTo(data.Length));

				var readData = new byte[data.Length];
				fileStream.Read(readData, 0, readData.Length);

				Assert.That(readData, Is.EqualTo(data));
			}

			// Cleanup.
			File.Delete(target);
		}

		[Test]
		public void ItIsPossibleToRenameFiles()
		{
			// Arrange.
			var source = Path.GetTempFileName();
			var target = Path.GetTempFileName();

			try
			{
				Assert.That(File.Exists(source));
				File.Delete(target);
				Assert.That(!File.Exists(target));

				// Act.
				var fs = new FileSystem.FileSystem();
				fs.Rename(source, target);

				// Assert.
				Assert.That(!File.Exists(source));
				Assert.That(File.Exists(target));
			}
			finally
			{
				try
				{
					File.Delete(source);
				}
				finally
				{
					File.Delete(target);
				}
			}
		}
	}
}
