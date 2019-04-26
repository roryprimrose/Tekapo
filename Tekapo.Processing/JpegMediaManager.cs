namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using EnsureThat;
    using ExifLibrary;

    public class JpegMediaManager : IMediaManager
    {
        public bool CanProcess(Stream stream)
        {
            Ensure.Any.IsNotNull(stream, nameof(stream));

            try
            {
                var image = ImageFile.FromStream(stream);

                if (image.Format == ImageFileFormat.JPEG)
                {
                    return true;
                }

                return false;
            }
            catch (NotValidImageFileException)
            {
                return false;
            }
        }

        public IEnumerable<string> GetSupportedFileTypes(MediaOperationType operationType)
        {
            yield return ".jpg";
            yield return ".jpeg";
        }

        public DateTime? ReadMediaCreatedDate(Stream stream)
        {
            Ensure.Any.IsNotNull(stream, nameof(stream));

            var image = ImageFile.FromStream(stream);

            var exifProperty = image.Properties?.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal) as ExifDateTime;

            return exifProperty?.Value;
        }

        [SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "The stream is the value returned by the function and cannot be disposed here.")]
        public Stream SetMediaCreatedDate(Stream stream, DateTime createdAt)
        {
            Ensure.Any.IsNotNull(stream, nameof(stream));

            var image = ImageFile.FromStream(stream);

            Debug.Assert(image.Properties != null,
                "The internal implementation of ImageFile has changed and Properties is no longer created by default.");

            var exifProperty = image.Properties?.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal) as ExifDateTime;

            if (exifProperty == null)
            {
                image.Properties.Add(ExifTag.DateTimeOriginal, createdAt);
            }
            else
            {
                exifProperty.Value = createdAt;
            }

            var newStream = new MemoryStream();

            image.Save(newStream);

            return newStream;
        }
    }
}