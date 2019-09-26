namespace Tekapo.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class ExecutionContextTests
    {
        private readonly string _assemblyLocation;
        private readonly string _directoryPath;

        public ExecutionContextTests()
        {
            _assemblyLocation = GetType().Assembly.Location;
            _directoryPath = Path.GetDirectoryName(_assemblyLocation);
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenArgumentContainsFileOnly()
        {
            var value = Guid.NewGuid().ToString();
            var args = new List<string> {_assemblyLocation, value};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenArgumentContainsFilesAndDirectories()
        {
            var value = Guid.NewGuid().ToString();
            var args = new List<string> {_assemblyLocation, _directoryPath, value};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenArgumentContainsMultipleDirectories()
        {
            var value = Guid.NewGuid().ToString();
            var secondDirectory = Directory.GetParent(_directoryPath).FullName;
            var args = new List<string> {_directoryPath, secondDirectory, value};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenArgumentsDoesNotContainDirectoryOrFilePaths()
        {
            var args = new List<string> {Guid.NewGuid().ToString()};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenArgumentsIsEmpty()
        {
            var args = Array.Empty<string>();

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenArgumentsIsNull()
        {
            var sut = new ExecutionContext(null);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenArgumentsOnlyContainsFilePath()
        {
            var args = new List<string> {_assemblyLocation};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsNullWhenMultipleDirectoryPathsFound()
        {
            var path = _directoryPath;
            var args = new List<string> {path, path};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().BeNull();
        }

        [Fact]
        public void SearchDirectoryReturnsSingleDirectoryArgument()
        {
            var path = _directoryPath;
            var args = new List<string> {path};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().Be(path);
        }

        [Fact]
        public void SearchDirectoryReturnsSingleDirectoryArgumentWhenIgnoringOtherNonFileValues()
        {
            var value = Guid.NewGuid().ToString();
            var args = new List<string> {_directoryPath, value};

            var sut = new ExecutionContext(args);

            sut.SearchDirectory.Should().Be(_directoryPath);
        }

        [Fact]
        public void SearchPathsDoesNotContainNonFileOrDirectoryValuesFromArgument()
        {
            var value = Guid.NewGuid().ToString();
            var args = new List<string> {_assemblyLocation, _directoryPath, value};

            var sut = new ExecutionContext(args);

            sut.SearchPaths.Should().NotContain(value);
        }

        [Fact]
        public void SearchPathsReturnsEmptyWhenArgumentsDoesNotContainDirectoryOrFilePaths()
        {
            var args = new List<string> {Guid.NewGuid().ToString()};

            var sut = new ExecutionContext(args);

            sut.SearchPaths.Should().BeEmpty();
        }

        [Fact]
        public void SearchPathsReturnsEmptyWhenArgumentsIsEmpty()
        {
            var args = Array.Empty<string>();

            var sut = new ExecutionContext(args);

            sut.SearchPaths.Should().BeEmpty();
        }

        [Fact]
        public void SearchPathsReturnsEmptyWhenArgumentsIsNull()
        {
            var sut = new ExecutionContext(null);

            sut.SearchPaths.Should().BeEmpty();
        }


        [Fact]
        public void SearchPathsReturnsFilePathFromArgument()
        {
            var value = Guid.NewGuid().ToString();
            var args = new List<string> {_assemblyLocation, value};

            var sut = new ExecutionContext(args);

            sut.SearchPaths.Should().Contain(_assemblyLocation);
            sut.SearchPaths.Should().NotContain(value);
        }

        [Fact]
        public void SearchPathsReturnsFilesAndDirectoriesFromArgument()
        {
            var value = Guid.NewGuid().ToString();
            var args = new List<string> {_assemblyLocation, _directoryPath, value};

            var sut = new ExecutionContext(args);

            sut.SearchPaths.Should().Contain(_assemblyLocation);
            sut.SearchPaths.Should().Contain(_directoryPath);
        }

        [Fact]
        public void SearchPathsReturnsMultipleDirectoriesFromArgument()
        {
            var value = Guid.NewGuid().ToString();
            var secondDirectory = Directory.GetParent(_directoryPath).FullName;
            var args = new List<string> {_directoryPath, secondDirectory, value};

            var sut = new ExecutionContext(args);

            sut.SearchPaths.Should().Contain(_directoryPath);
            sut.SearchPaths.Should().Contain(secondDirectory);
        }

        [Fact]
        public void SearchPathsReturnsEmptyForSingleDirectoryArgument()
        {
            var args = new List<string> {_directoryPath};

            var sut = new ExecutionContext(args);

            sut.SearchPaths.Should().BeEmpty();
        }

        [Fact]
        public void SearchPathsReturnsEmptyForSingleDirectoryArgumentWhileIgnoringOtherNonFileValues()
        {
            var value = Guid.NewGuid().ToString();
            var args = new List<string> {_directoryPath, value};

            var sut = new ExecutionContext(args);
            
            sut.SearchPaths.Should().BeEmpty();
        }
    }
}