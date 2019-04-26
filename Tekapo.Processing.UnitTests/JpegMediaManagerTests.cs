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
        public void CanProcessReturnsFalseForPngFile()
        {
            var sut = new JpegMediaManager();

            using (var stream = new MemoryStream(Resources.example_png))
            {
                var actual = sut.CanProcess(stream);

                actual.Should().BeFalse();
            }
        }

        [Fact]
        public void CanProcessReturnsFalseForUnsupportedImageFile()
        {
            var sut = new JpegMediaManager();

            using (var stream = new MemoryStream(Resources.example_bmp))
            {
                var actual = sut.CanProcess(stream);

                actual.Should().BeFalse();
            }
        }

        [Fact]
        public void CanProcessReturnsTrueForJpgFile()
        {
            var sut = new JpegMediaManager();

            using (var stream = new MemoryStream(Resources.nopicturetaken_jpg))
            {
                var actual = sut.CanProcess(stream);

                actual.Should().BeTrue();
            }
        }

        [Fact]
        public void CanProcessThrowsExceptionWithNullStream()
        {
            var sut = new JpegMediaManager();

            Action action = () => sut.CanProcess(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(MediaOperationType.Read)]
        [InlineData(MediaOperationType.ReadWrite)]
        public void GetSupportedFileTypesReturnsCorrectFileExtensions(MediaOperationType operationType)
        {
            var sut = new JpegMediaManager();

            var actual = sut.GetSupportedFileTypes(operationType).ToList();

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
        public void ReadMediaCreatedDateThrowsExceptionWithNullStream()
        {
            var sut = new JpegMediaManager();

            Action action = () => sut.ReadMediaCreatedDate(null);

            action.Should().Throw<ArgumentNullException>();
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