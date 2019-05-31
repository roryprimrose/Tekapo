namespace Tekapo.IntegrationTests
{
    using System.Configuration;
    using FluentAssertions;
    using Xunit;

    public class ConfigTests
    {
        [Theory]
        [InlineData(null, 1000)]
        [InlineData("", 1000)]
        [InlineData("  ", 1000)]
        [InlineData("stuff", 1000)]
        [InlineData("123", 123)]
        public void MaxCollisionIncrementReturnsExpectedValue(string value, int expected)
        {
            ConfigurationManager.AppSettings[nameof(IConfig.MaxCollisionIncrement)] = value;

            var sut = new Config();

            sut.MaxCollisionIncrement.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, 5)]
        [InlineData("", 5)]
        [InlineData("  ", 5)]
        [InlineData("stuff", 5)]
        [InlineData("123", 123)]
        public void MaxNameFormatItemsReturnsExpectedValue(string value, int expected)
        {
            ConfigurationManager.AppSettings[nameof(IConfig.MaxNameFormatItems)] = value;

            var sut = new Config();

            sut.MaxNameFormatItems.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, 5)]
        [InlineData("", 5)]
        [InlineData("  ", 5)]
        [InlineData("stuff", 5)]
        [InlineData("123", 123)]
        public void MaxSearchDirectoryItemsReturnsExpectedValue(string value, int expected)
        {
            ConfigurationManager.AppSettings[nameof(IConfig.MaxSearchDirectoryItems)] = value;

            var sut = new Config();

            sut.MaxSearchDirectoryItems.Should().Be(expected);
        }
    }
}