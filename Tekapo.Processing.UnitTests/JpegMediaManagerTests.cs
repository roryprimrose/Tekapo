namespace Tekapo.Processing.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Tekapo.Processing.UnitTests.Properties;
    using Xunit;

    public class JpegMediaManagerTests
    {
        [Fact]
        public void GetSupportedFileTypesReturnsCorrectFileExtensions()
        {
            var sut = new JpegMediaManager();

            var actual = sut.GetSupportedFileTypes().ToList();

            actual.Should().HaveCount(2);
            actual.Should().Contain(".jpg");
            actual.Should().Contain(".jpeg");
        }

        [Fact]
        public void ReadMediaCreatedDateReturnsValueForStreamContainingExifData()
        {
            var expected = new DateTime(2019, 4, 25, 16, 58, 17);

            var sut = new JpegMediaManager();

            using (var stream = new MemoryStream(Resources.exif_jpg))
            {
                var actual = sut.ReadMediaCreatedDate(stream);

                actual.Should().HaveValue();
                actual.Should().Be(expected);
            }
        }
    }
}