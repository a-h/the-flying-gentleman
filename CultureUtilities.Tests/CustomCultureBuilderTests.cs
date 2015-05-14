using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CultureUtilities;
using FluentAssertions;
using AutoMapper;

namespace CultureUtilities.Tests
{
    [TestFixture]
    public class CustomCultureBuilderTests
    {
        private CustomCultureBuilder Builder()
        {
            return new CustomCultureBuilder();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {

        }

        [Test]
        public void BuildCustomCulture_PopulatesExpectedProperties()
        {
            // Arrange
            var settings = TestData.FullyPopulatedSpecificCultureSettings();

            // Act

            var result = Builder().BuildCustomCulture(settings);

            // Assert
            result.CultureName.Should().Be("x-test");
            result.CultureEnglishName.Should().Be("Test Culture (Test Region)");
            result.CultureNativeName.Should().Be("Test Culture (Test Region)");
            result.ThreeLetterISOLanguageName.Should().Be("tst");
            result.ThreeLetterWindowsLanguageName.Should().Be("tst");
            result.TwoLetterISOLanguageName.Should().Be("ts");
            result.RegionEnglishName.Should().Be("Test Region");
            result.RegionNativeName.Should().Be("Test Region");
            result.ThreeLetterISORegionName.Should().Be("tmi");
            result.TwoLetterISORegionName.Should().Be("tm");
            result.ThreeLetterWindowsRegionName.Should().Be("tmi");
            result.IsRightToLeft.Should().Be(true);
            result.GeoId.Should().Be(123);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "ParentCultureName", MatchType = MessageMatch.Contains)]
        public void BuildCustomCulture_ThrowsExceptionForInvalidParentCultureName()
        {
            // Arrange
            var settings = TestData.FullyPopulatedSpecificCultureSettings();
            settings.ParentCultureName = "x-invalid";
    
            // Act
            var result = Builder().BuildCustomCulture(settings);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Parent culture must be a Neutral culture", MatchType = MessageMatch.Contains)]
        public void BuildCustomn_Culture_ThrowsExceptionForNonNeutralParentCulture()
        {
            // Arrange
            var settings = TestData.FullyPopulatedSpecificCultureSettings();
            settings.ParentCultureName = "en-GB";

            // Act
            var result = Builder().BuildCustomCulture(settings);
        }

    }
}
