namespace Tekapo.Processing.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using NSubstitute;
    using Xunit;

    public class FileSearcherTests
    {
        private readonly string _assemblyLocation;
        private readonly string _directoryPath;

        public FileSearcherTests()
        {
            _assemblyLocation = GetType().Assembly.Location;
            _directoryPath = Path.GetDirectoryName(_assemblyLocation);
        }

        [Fact]
        public void FindSupportedPathsIgnoresDirectoryPathNotFound()
        {
            var directoryPath = Path.Combine(_directoryPath, Guid.NewGuid().ToString("N") + "\\");

            var paths = new List<string> {directoryPath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            var sut = new FileSearcher(mediaManager, settings);

            var actual = sut.FindSupportedFiles(paths, MediaOperationType.Read);

            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void FindSupportedPathsIgnoresEmptyValues(string value)
        {
            var paths = new List<string> {value};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            var sut = new FileSearcher(mediaManager, settings);

            var actual = sut.FindSupportedFiles(paths, MediaOperationType.Read);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void FindSupportedPathsIgnoresFilePathNotFound()
        {
            var filePath = Path.Combine(_directoryPath, Guid.NewGuid().ToString("N") + ".txt");

            var paths = new List<string> {filePath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            var sut = new FileSearcher(mediaManager, settings);

            var actual = sut.FindSupportedFiles(paths, MediaOperationType.Read);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void FindSupportedPathsReturnsEmptyWhenPathsIsEmpty()
        {
            var paths = new List<string>();

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            var sut = new FileSearcher(mediaManager, settings);

            var actual = sut.FindSupportedFiles(paths, MediaOperationType.Read);

            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineData(MediaOperationType.ReadWrite, false)]
        [InlineData(MediaOperationType.Read, false)]
        [InlineData(MediaOperationType.ReadWrite, true)]
        [InlineData(MediaOperationType.Read, true)]
        public void FindSupportedPathsReturnsExistingFilePathBasedOnMediaSupported(
            MediaOperationType mediaOperationType,
            bool mediaSupported)
        {
            var filePath = Path.Combine(_directoryPath, Guid.NewGuid().ToString("N") + ".txt");
            var supportedExtensions = new List<string> {".jpg"};
            var paths = new List<string> {filePath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            if (mediaSupported)
            {
                supportedExtensions.Add(".txt");
            }

            settings.SearchFilterType = SearchFilterType.None;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            try
            {
                File.WriteAllText(filePath, Guid.NewGuid().ToString());

                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(paths, mediaOperationType);

                if (mediaSupported)
                {
                    actual.Should().Contain(filePath);
                }
                else
                {
                    actual.Should().BeEmpty();
                }
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Theory]
        [InlineData("test.txt", "*", true)]
        [InlineData("test.txt", "*.txt", true)]
        [InlineData("test.txt", "*.sky", false)]
        [InlineData("test.txt", "*z.txt", false)]
        [InlineData("test.txt", "z*.txt", false)]
        [InlineData("test.txt", "te*.txt", true)]
        [InlineData("test.txt", "TE*.txt", true)]
        [InlineData("Test.txt", "te*.txt", true)]
        [InlineData("test.txt", "test.*", true)]
        [InlineData("test.txt", "te*xt", true)]
        [InlineData("2019\\test.txt", "*\\2019\\test.txt", true)]
        [InlineData("2019\\test.txt", "*\\2019\\*", true)]
        [InlineData("2019\\test.txt", "*2019*", true)]
        [InlineData("2019\\test.txt", "*2019", false)]
        [InlineData("2019\\test.txt", "2019*", false)]
        public void FindSupportedPathsReturnsExistingFilePathMatchingWildcardFilter(
            string fileSegment,
            string wildcardPattern,
            bool isMatch)
        {
            var filePath = Path.Combine(_directoryPath, fileSegment);
            var directoryPath = Path.GetDirectoryName(filePath);
            var createDirectory = Directory.Exists(directoryPath) == false;
            var extension = Path.GetExtension(filePath);
            var supportedExtensions = new List<string> {extension};
            var mediaOperationType = MediaOperationType.ReadWrite;
            var paths = new List<string> {filePath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.Wildcard;
            settings.WildcardFilter = wildcardPattern;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            try
            {
                if (createDirectory)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(filePath, Guid.NewGuid().ToString());

                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(paths, mediaOperationType);

                if (isMatch)
                {
                    actual.Should().Contain(filePath);
                }
                else
                {
                    actual.Should().BeEmpty();
                }
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                if (createDirectory && Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath);
                }
            }
        }
        
        [Theory]
        [InlineData("test.txt", ".*", true)]
        [InlineData("test.txt", ".*\\.txt", true)]
        [InlineData("test.txt", ".*\\.txt$", true)]
        [InlineData("test.txt", "^.*\\.txt", true)]
        [InlineData("test.txt", "^.*\\.txt$", true)]
        [InlineData("test.txt", ".*\\.sky", false)]
        [InlineData("test.txt", ".*z\\.txt", false)]
        [InlineData("test.txt", "z.*\\.txt", false)]
        [InlineData("test.txt", "te.*\\.txt", true)]
        [InlineData("test.txt", "TE.*\\.txt", false)]
        [InlineData("Test.txt", "te.*\\.txt", false)]
        [InlineData("test.txt", "test\\..*", true)]
        [InlineData("test.txt", "te.*xt", true)]
        [InlineData("2019\\test.txt", ".*\\\\2019\\\\test\\.txt", true)]
        [InlineData("2019\\test.txt", ".*\\\\2019\\\\test\\.txt$", true)]
        [InlineData("2019\\test.txt", ".*\\\\2019\\\\.*", true)]
        [InlineData("2019\\test.txt", "\\d{4}", true)]
        [InlineData("2019\\test.txt", "\\\\\\d{4}\\\\", true)]
        [InlineData("2019\\test.txt", ".*2019.*", true)]
        [InlineData("2019\\test.txt", "^.*2019.*$", true)]
        [InlineData("2019\\test.txt", ".*2019", true)]
        [InlineData("2019\\test.txt", ".*2019$", false)]
        [InlineData("2019\\test.txt", "2019.*", true)]
        [InlineData("2019\\test.txt", "^2019.*", false)]
        public void FindSupportedPathsReturnsExistingFilePathMatchingRegularExpressionFilter(
            string fileSegment,
            string regularExpression,
            bool isMatch)
        {
            var filePath = Path.Combine(_directoryPath, fileSegment);
            var directoryPath = Path.GetDirectoryName(filePath);
            var createDirectory = Directory.Exists(directoryPath) == false;
            var extension = Path.GetExtension(filePath);
            var supportedExtensions = new List<string> {extension};
            var mediaOperationType = MediaOperationType.ReadWrite;
            var paths = new List<string> {filePath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.RegularExpression;
            settings.RegularExpressionFilter = regularExpression;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            try
            {
                if (createDirectory)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(filePath, Guid.NewGuid().ToString());

                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(paths, mediaOperationType);

                if (isMatch)
                {
                    actual.Should().Contain(filePath);
                }
                else
                {
                    actual.Should().BeEmpty();
                }
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                if (createDirectory && Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath);
                }
            }
        }

        [Theory]
        [InlineData(".txt")]
        [InlineData(".TXT")]
        public void FindSupportedPathsReturnsExistingFilePathWithCaseInsensitiveExtensionMatch(string extension)
        {
            var filePath = Path.Combine(_directoryPath, Guid.NewGuid().ToString("N") + extension);
            var supportedExtensions = new List<string> {".jpg", ".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;
            var paths = new List<string> {filePath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            try
            {
                File.WriteAllText(filePath, Guid.NewGuid().ToString());

                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(paths, mediaOperationType);

                actual.Should().Contain(filePath);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
        
        [Fact]
        public void FindSupportedPathsReturnsPathOnlyOnce()
        {
            var filePath = Path.Combine(_directoryPath, Guid.NewGuid().ToString("N") + ".txt");
            var supportedExtensions = new List<string> {".jpg", ".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;
            var paths = new List<string> {filePath, filePath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            try
            {
                File.WriteAllText(filePath, Guid.NewGuid().ToString());

                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(paths, mediaOperationType).ToList();

                actual.Should().HaveCount(1);
                actual.Should().Contain(filePath);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Fact]
        public void FindSupportedPathsThrowsExceptionWithNullPaths()
        {
            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            var sut = new FileSearcher(mediaManager, settings);

            Action action = () => sut.FindSupportedFiles(null, MediaOperationType.Read).ToList();

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FindSupportedReturnsEmptyWhenSpecifiedDirectoryIsEmpty()
        {
            var directoryPath = Path.Combine(_directoryPath, Guid.NewGuid().ToString("N") + "\\");

            var paths = new List<string> {directoryPath};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            var sut = new FileSearcher(mediaManager, settings);

            try
            {
                Directory.CreateDirectory(directoryPath);

                var actual = sut.FindSupportedFiles(paths, MediaOperationType.Read);

                actual.Should().BeEmpty();
            }
            finally
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath);
                }
            }
        }

        [Fact]
        public void ThrowsExceptionWhenCreatedWithNullMediaManager()
        {
            var settings = Substitute.For<ISettings>();

            Action action = () => new FileSearcher(null, settings);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsExceptionWhenCreatedWithNullSettings()
        {
            var mediaManager = Substitute.For<IMediaManager>();

            Action action = () => new FileSearcher(mediaManager, null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}