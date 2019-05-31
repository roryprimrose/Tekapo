namespace Tekapo.IntegrationTests
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Tekapo.Processing;
    using Xunit;

    public class StartupTests
    {
        [Fact]
        public void StartupReplacesSearchPathSettingWithCommandLineValue()
        {
            var commandLine = Guid.NewGuid().ToString();
            var searchPath = Guid.NewGuid().ToString();

            var executionContext = Substitute.For<IExecutionContext>();
            var settings = Substitute.For<ISettings>();

            executionContext.SearchDirectory.Returns(commandLine);
            settings.SearchPath = searchPath;

            var sut = new Startup(executionContext, settings);

            sut.Start();

            settings.SearchPath.Should().Be(commandLine);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void StartupReplacesSearchPathSettingWithMyPicturesWhenNoSettingOrCommandLineValueFound(string value)
        {
            var expected = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            var executionContext = Substitute.For<IExecutionContext>();
            var settings = Substitute.For<ISettings>();

            executionContext.SearchDirectory.Returns(value);
            settings.SearchPath = value;

            var sut = new Startup(executionContext, settings);

            sut.Start();

            settings.SearchPath.Should().Be(expected);
        }

        [Fact]
        public void StartupRetainsSettingsSearchPathValueWhenNoCommandLineFound()
        {
            var expected = Guid.NewGuid().ToString();

            var executionContext = Substitute.For<IExecutionContext>();
            var settings = Substitute.For<ISettings>();

            settings.SearchPath = expected;

            var sut = new Startup(executionContext, settings);

            sut.Start();

            settings.SearchPath.Should().Be(expected);
        }

        [Fact]
        public void ThrowsExceptionWhenCreatedWithNullExecutionContext()
        {
            var settings = Substitute.For<ISettings>();

            Action action = () => new Startup(null, settings);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsExceptionWhenCreatedWithNullSettings()
        {
            var executionContext = Substitute.For<IExecutionContext>();

            Action action = () => new Startup(executionContext, null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}