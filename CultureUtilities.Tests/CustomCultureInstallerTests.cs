using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

namespace CultureUtilities.Tests
{
    [TestFixture]
    public class CustomCultureInstallerTests
    {
        ICustomCultureBuilder _builder;

        private CustomCultureInstaller Installer()
        {
            return new CustomCultureInstaller(_builder);
        }

        [SetUp]
        public void SetUp()
        {
            _builder = Substitute.For<ICustomCultureBuilder>();
        }

        [Test]
        public void Install_InstallsCulture()
        {
            _builder.BuildCustomCulture(Arg.Any<CustomCultureSettings>()).Returns(TestData.IntegrationTestBuilder());

            var settings = new CustomCultureSettings()
            {
                CultureName = "x-test",
                CultureDisplayName = "Culture",
            };

            var result = Installer().Install(settings, true, true);

            result.Success.Should().Be(true);
        }

        [Test]
        public void Install_GeneratesInstallationReport_IfCultureFailsToBuild()
        {
            _builder.BuildCustomCulture(Arg.Any<CustomCultureSettings>()).Returns(x => { throw new Exception("It went wrong"); });

            var settings = new CustomCultureSettings()
            {
                CultureName = "x-test",
                CultureDisplayName = "Culture",
            };

            var result = Installer().Install(settings, true, true);

            result.Success.Should().Be(false);
            result.ExceptionMessage.Should().Contain("It went wrong");
        }
    }
}
