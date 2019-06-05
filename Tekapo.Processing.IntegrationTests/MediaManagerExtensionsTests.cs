namespace Tekapo.Processing.IntegrationTests
{
    using System;
    using System.IO;
    using FluentAssertions;
    using NSubstitute;
    using Xunit;

    public class MediaManagerExtensionsTests
    {
        [Fact]
        public void IsSupportedReturnsFalseForPathWithoutFileExtension()
        {
            var manager = Substitute.For<IMediaManager>();

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n"));

            var actual = manager.IsSupported(path, MediaOperationType.Read);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("stuff.jpg", true, ".jpg", ".png")]
        [InlineData("stuff.JPG", true, ".jpg", ".png")]
        [InlineData("stuff.jpg", false, ".png")]
        public void IsSupportedReturnsWhetherMediaManagerSupportsFileType(
            string path,
            bool expected,
            params string[] fileTypes)
        {
            var operationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();

            mediaManager.GetSupportedFileTypes(operationType).Returns(fileTypes);

            var actual = mediaManager.IsSupported(path, operationType);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void IsSupportedThrowsExceptionWithNullInvalidPath(string path)
        {
            var manager = Substitute.For<IMediaManager>();

            Action action = () => manager.IsSupported(path, MediaOperationType.Read);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsSupportedThrowsExceptionWithNullMediaManager()
        {
            IMediaManager manager = null;

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n"));

            Action action = () => manager.IsSupported(path, MediaOperationType.Read);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ReadMediaInfoReturnsInfoForFilePath()
        {
            var path = Path.GetTempFileName();
            var created = DateTime.UtcNow;

            var mediaManager = Substitute.For<IMediaManager>();

            mediaManager.ReadMediaCreatedDate(Arg.Any<Stream>()).Returns(created);

            try
            {
                File.WriteAllText(path, Guid.NewGuid().ToString());

                var actual = mediaManager.ReadMediaInfo(path);

                actual.FilePath.Should().Be(path);
                actual.Hash.Should().NotBeNullOrWhiteSpace();
                actual.MediaCreated.Should().HaveValue();
                actual.MediaCreated.Should().Be(created);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Fact]
        public void ReadMediaInfoThrowsExceptionWithInvalidFilePath()
        {
            var path = Guid.NewGuid().ToString();

            var mediaManager = Substitute.For<IMediaManager>();

            Action action = () => mediaManager.ReadMediaInfo(path);

            action.Should().Throw<FileNotFoundException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ReadMediaInfoThrowsExceptionWithInvalidPath(string path)
        {
            var mediaManager = Substitute.For<IMediaManager>();

            Action action = () => mediaManager.ReadMediaInfo(path);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReadMediaInfoThrowsExceptionWithNullMediaManager()
        {
            var path = Guid.NewGuid().ToString();

            IMediaManager sut = null;

            Action action = () => sut.ReadMediaInfo(path);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}