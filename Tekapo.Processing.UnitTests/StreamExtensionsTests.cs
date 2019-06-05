namespace Tekapo.Processing.UnitTests
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Tekapo.Processing.UnitTests.Properties;
    using Xunit;

    public class StreamExtensionsTests
    {
        [Fact]
        public void CalculateHashReturnsUppercaseHexValue()
        {
            using (var stream = new MemoryStream(Resources.example_png))
            {
                var actual = stream.CalculateHash();

                actual.Should().Be(actual.ToUpperInvariant());
            }
        }

        [Fact]
        public void CalculateHashReturnsValueForStream()
        {
            using (var stream = new MemoryStream(Resources.example_png))
            {
                var actual = stream.CalculateHash();

                actual.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Fact]
        public void CalculateHashReturnsValueForStreamWithPositionNotAtStart()
        {
            using (var stream = new MemoryStream(Resources.example_png))
            {
                var first = stream.CalculateHash();
                var second = stream.CalculateHash();

                first.Should().Be(second);
            }
        }

        [Fact]
        public void CalculateHashThrowsExceptionWithNullStream()
        {
            Stream sut = null;

            Action action = () => sut.CalculateHash();

            action.Should().Throw<ArgumentNullException>();
        }
    }
}