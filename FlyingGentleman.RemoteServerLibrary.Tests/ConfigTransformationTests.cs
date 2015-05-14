using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;
using FluentAssertions;

namespace FlyingGentleman.RemoteServerLibrary.Tests
{
    [TestFixture]
    public class ConfigTransformationTests
    {
        private FileSystem.FileSystem GetSut()
        {
            return new FileSystem.FileSystem();
        }

        [Test]
        public void TransformConfigFile_PerformsExpectedTransformation()
        {
            // Arrange
            var baseConfig = LoadEmbeddedTestFile("BaseConfig.xml");
            var transformConfig = LoadEmbeddedTestFile("DevTransform.xml");
            var expectedConfig = LoadEmbeddedTestFile("DevExpected.xml");

            XmlDocument expected = new XmlDocument();
            expected.Load(expectedConfig);

            // Act
            XmlDocument result = GetSut().TransformConfigFile(baseConfig, transformConfig);

            // Assert
            result.InnerXml.ShouldBeEquivalentTo(expected.InnerXml);
        }

        [Test]
        public void TransformConfigFile_WritesToExpectedOutputPath()
        {

        }

        [Test]
        public void TransformConfigFile_LogsErrorEvents()
        {

        }

        private static Stream LoadEmbeddedTestFile(string name)
        {
            string fullName = "FlyingGentleman.RemoteServerLibrary.Tests.TestDocuments.ConfigTransformation." + name;

            try
            {
                var stream = typeof(ConfigTransformationTests).Assembly.GetManifestResourceStream(fullName);

                if(stream == null)
                {
                    throw new Exception("Embedded resource not found");
                }

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to load script '{0}'", name), ex);
            }
        }

    }
}
