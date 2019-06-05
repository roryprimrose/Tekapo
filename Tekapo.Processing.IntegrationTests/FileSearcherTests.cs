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
        private readonly string _directoryPath;

        public FileSearcherTests()
        {
            var assemblyLocation = GetType().Assembly.Location;

            _directoryPath = Path.GetDirectoryName(assemblyLocation);
        }

        [Fact]
        public void FindSupportedPathsDoesNotReturnFilesInChildDirectoriesWhenRecursiveSearchDisabled()
        {
            var searchPaths = new List<string> {_directoryPath};
            var supportedExtensions = new List<string> {".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            settings.RecursiveSearch = false;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (new TestScenario(_directoryPath,
                Guid.NewGuid().ToString("N") + ".txt",
                "Parent\\" + Guid.NewGuid().ToString("N") + ".txt",
                "2019\\Parent\\" + Guid.NewGuid().ToString("N") + ".txt"))
            {
                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(searchPaths, mediaOperationType);

                actual.Should().BeEmpty();
            }
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
        public void FindSupportedPathsProcessesDirectoryOnlyOnce()
        {
            var supportedExtensions = new List<string> {".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            settings.RecursiveSearch = true;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (var scenario = new TestScenario(_directoryPath,
                Guid.NewGuid().ToString("N") + ".txt",
                "Parent\\" + Guid.NewGuid().ToString("N") + ".txt",
                "2019\\Parent\\" + Guid.NewGuid().ToString("N") + ".txt"))
            {
                var searchPaths = new List<string> {scenario.ScenarioDirectory, scenario.ScenarioDirectory};

                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(searchPaths, mediaOperationType);

                actual.Should().BeEquivalentTo(scenario.Files);
            }
        }

        [Fact]
        public void FindSupportedPathsReturnsEmptyWhenAccessDenied()
        {
            var mediaOperationType = MediaOperationType.ReadWrite;
            var paths = new List<string>
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "Tasks")
            };

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            settings.RecursiveSearch = true;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(new[] {"*.txt"});

            var sut = new FileSearcher(mediaManager, settings);

            var actual = sut.FindSupportedFiles(paths, mediaOperationType).ToList();

            actual.Should().HaveCount(0);
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
            var supportedExtensions = new List<string> {".jpg"};

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            if (mediaSupported)
            {
                supportedExtensions.Add(".txt");
            }

            settings.SearchFilterType = SearchFilterType.None;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (var scenario = new TestScenario(_directoryPath, Guid.NewGuid().ToString("N") + ".txt"))
            {
                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(scenario.Files, mediaOperationType);

                if (mediaSupported)
                {
                    actual.Should().Contain(scenario.Files[0]);
                }
                else
                {
                    actual.Should().BeEmpty();
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
        [InlineData("Stuff.txt", "stu.*\\.txt", false)]
        [InlineData("test.txt", "test\\..*", true)]
        [InlineData("test.txt", "te.*xt", true)]
        [InlineData("2019\\More Here\\test.txt", ".*\\\\2019\\\\More Here\\\\test\\.txt", true)]
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
            var extension = Path.GetExtension(fileSegment);
            var supportedExtensions = new List<string> {extension};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.RegularExpression;
            settings.RegularExpressionFilter = regularExpression;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (var scenario = new TestScenario(_directoryPath, fileSegment))
            {
                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(scenario.Files, mediaOperationType);

                if (isMatch)
                {
                    actual.Should().Contain(scenario.Files[0]);
                }
                else
                {
                    actual.Should().BeEmpty();
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
            var extension = Path.GetExtension(fileSegment);
            var supportedExtensions = new List<string> {extension};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.Wildcard;
            settings.WildcardFilter = wildcardPattern;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (var scenario = new TestScenario(_directoryPath, fileSegment))
            {
                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(scenario.Files, mediaOperationType);

                if (isMatch)
                {
                    actual.Should().Contain(scenario.Files[0]);
                }
                else
                {
                    actual.Should().BeEmpty();
                }
            }
        }

        [Theory]
        [InlineData(".txt")]
        [InlineData(".TXT")]
        public void FindSupportedPathsReturnsExistingFilePathWithCaseInsensitiveExtensionMatch(string extension)
        {
            var supportedExtensions = new List<string> {".jpg", ".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (var scenario = new TestScenario(_directoryPath, Guid.NewGuid().ToString("N") + extension))
            {
                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(scenario.Files, mediaOperationType);

                actual.Should().Contain(scenario.Files[0]);
            }
        }

        [Fact]
        public void FindSupportedPathsReturnsFileInSearchPathOnlyWhenRecursiveSearchDisabled()
        {
            var supportedExtensions = new List<string> {".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            settings.RecursiveSearch = false;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (var scenario = new TestScenario(_directoryPath,
                Guid.NewGuid().ToString("N") + ".txt",
                "Parent\\" + Guid.NewGuid().ToString("N") + ".txt",
                "2019\\Parent\\" + Guid.NewGuid().ToString("N") + ".txt"))
            {
                var searchPaths = new List<string> {scenario.ScenarioDirectory};

                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(searchPaths, mediaOperationType).ToList();

                actual.Should().HaveCount(1);
                actual[0].Should().Be(scenario.Files[0]);
            }
        }

        [Fact]
        public void FindSupportedPathsReturnsMatchingFilesInRecursiveDirectories()
        {
            var searchPaths = new List<string> {_directoryPath};
            var supportedExtensions = new List<string> {".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            settings.RecursiveSearch = true;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            using (var scenario = new TestScenario(_directoryPath,
                Guid.NewGuid().ToString("N") + ".txt",
                "Parent\\" + Guid.NewGuid().ToString("N") + ".txt",
                "2019\\Parent\\" + Guid.NewGuid().ToString("N") + ".txt"))
            {
                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(searchPaths, mediaOperationType);

                actual.Should().BeEquivalentTo(scenario.Files);
            }
        }

        [Fact]
        public void FindSupportedPathsReturnsPathOnlyOnce()
        {
            var supportedExtensions = new List<string> {".jpg", ".txt"};
            var mediaOperationType = MediaOperationType.ReadWrite;

            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            settings.SearchFilterType = SearchFilterType.None;
            mediaManager.GetSupportedFileTypes(mediaOperationType).Returns(supportedExtensions);

            var fileName = Guid.NewGuid().ToString("N") + ".txt";

            using (var scenario = new TestScenario(_directoryPath, fileName, fileName))
            {
                var sut = new FileSearcher(mediaManager, settings);

                var actual = sut.FindSupportedFiles(scenario.Files, mediaOperationType).ToList();

                actual.Should().HaveCount(1);
                actual.Should().Contain(scenario.Files[0]);
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
            var mediaManager = Substitute.For<IMediaManager>();
            var settings = Substitute.For<ISettings>();

            var sut = new FileSearcher(mediaManager, settings);
            
            using (var scenario = new TestScenario(_directoryPath))
            {
                var actual = sut.FindSupportedFiles(new []{scenario.ScenarioDirectory}, MediaOperationType.Read);

                actual.Should().BeEmpty();
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