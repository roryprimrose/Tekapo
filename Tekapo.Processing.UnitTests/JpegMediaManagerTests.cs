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
        public void ReadMediaCreatedDateReturnsNullWhenStreamDoesNotContainPictureTakenDate()
        {
            var sut = new JpegMediaManager();

            using (var stream = new MemoryStream(Resources.nopicturetaken_jpg))
            {
                var actual = sut.ReadMediaCreatedDate(stream);

                actual.Should().NotHaveValue();
            }
        }

        [Fact]
        public void ReadMediaCreatedDateReturnsValueWhenStreamContainsPictureTakenDate()
        {
            var expected = new DateTime(2019, 4, 25, 16, 58, 17);

            var sut = new JpegMediaManager();

            using (var stream = new MemoryStream(Resources.picturetaken_jpg))
            {
                var actual = sut.ReadMediaCreatedDate(stream);

                actual.Should().HaveValue();
                actual.Should().Be(expected);
            }
        }

        [Fact]
        public void SetMediaCreatedDateCanUpdateStreamAlreadyContainingPictureTakenDate()
        {
            var point = DateTime.Now;
            var expected = new DateTime(point.Year, point.Month, point.Day, point.Hour, point.Minute, point.Second);

            var sut = new JpegMediaManager();

            using (var inputStream = new MemoryStream(Resources.picturetaken_jpg))
            {
                using (var outputStream = sut.SetMediaCreatedDate(inputStream, expected))
                {
                    var actual = sut.ReadMediaCreatedDate(outputStream);

                    actual.Should().HaveValue();
                    actual.Should().Be(expected);
                }
            }
        }

        [Fact]
        public void SetMediaCreatedDateCanUpdateStreamNotContainingPictureTakenDate()
        {
            var point = DateTime.Now;
            var expected = new DateTime(point.Year, point.Month, point.Day, point.Hour, point.Minute, point.Second);

            var sut = new JpegMediaManager();

            using (var inputStream = new MemoryStream(Resources.nopicturetaken_jpg))
            {
                using (var outputStream = sut.SetMediaCreatedDate(inputStream, expected))
                {
                    var actual = sut.ReadMediaCreatedDate(outputStream);

                    actual.Should().HaveValue();
                    actual.Should().Be(expected);
                }
            }
        }
    }
}