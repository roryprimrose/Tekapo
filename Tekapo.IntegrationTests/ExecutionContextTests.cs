namespace Tekapo.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class ExecutionContextTests
    {
        [Fact]
        public void CreatesWithDefaultValuesWhenArgsIsEmpty()
        {
            var args = new string[0];

            var sut = new ExecutionContext(args);

            sut.SearchPath.Should().BeNull();
        }

        [Fact]
        public void CreatesWithDefaultValuesWhenArgsIsNull()
        {
            var sut = new ExecutionContext(null);

            sut.SearchPath.Should().BeNull();
        }

        [Fact]
        public void CreatesWithDefaultValuesWhenArgsNotContainingDirectory()
        {
            var args = new List<string> {Guid.NewGuid().ToString()};

            var sut = new ExecutionContext(args);

            sut.SearchPath.Should().BeNull();
        }

        [Fact]
        public void SearchPathDoesNotReturnFilePath()
        {
            var args = new List<string> {GetType().Assembly.Location};

            var sut = new ExecutionContext(args);

            sut.SearchPath.Should().BeNull();
        }

        [Fact]
        public void SearchPathReturnsNullWhenMultipleDirectoryPathsFound()
        {
            var path = Path.GetDirectoryName(GetType().Assembly.Location);
            var args = new List<string> {path, path};

            var sut = new ExecutionContext(args);

            sut.SearchPath.Should().BeNull();
        }

        [Fact]
        public void SearchPathReturnsSingleDirectoryArgument()
        {
            var path = Path.GetDirectoryName(GetType().Assembly.Location);
            var args = new List<string> {path};

            var sut = new ExecutionContext(args);

            sut.SearchPath.Should().Be(path);
        }

        [Fact]
        public void SearchPathReturnsSingleDirectoryArgumentWhenIgnoringOtherValues()
        {
            var value = Guid.NewGuid().ToString();
            var filePath = GetType().Assembly.Location;
            var expected = Path.GetDirectoryName(filePath);
            var args = new List<string> {filePath, expected, value};

            var sut = new ExecutionContext(args);

            sut.SearchPath.Should().Be(expected);
        }
    }
}