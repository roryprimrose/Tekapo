namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using MetadataExtractor;
    using MetadataExtractor.Formats.QuickTime;
    using MetadataExtractor.Util;
    using Directory = MetadataExtractor.Directory;

    public class MovMediaManager : IMediaManager
    {
        public bool CanProcess(Stream stream)
        {
            stream.Position = 0;

            var fileType = FileTypeDetector.DetectFileType(stream);

            if (fileType == FileType.QuickTime)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetSupportedFileTypes(MediaOperationType operationType)
        {
            if (operationType == MediaOperationType.Read)
            {
                yield return ".mov";
            }
        }

        public DateTime? ReadMediaCreatedDate(Stream stream)
        {
            stream.Position = 0;

            var metadata = ImageMetadataReader.ReadMetadata(stream);

            var tag = SearchForTag(metadata);

            if (tag == null)
            {
                return null;
            }

            var value = tag.Description;

            const string Format = "ddd MMM dd HH:mm:ss yyyy";

            // Thu Sep 02 07:00:53 2010
            if (DateTime.TryParseExact(value,
                Format,
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeUniversal,
                out var created))
            {
                return created.ToLocalTime();
            }

            return null;
        }

        public Stream SetMediaCreatedDate(Stream stream, DateTime createdAt)
        {
            throw new NotSupportedException("Updates can not be made to mov files.");
        }

        private Tag SearchForTag(IReadOnlyList<Directory> directories)
        {
            var movieHeader = directories.FirstOrDefault(x => x.Name == "QuickTime Movie Header");

            if (movieHeader == null)
            {
                return null;
            }

            if (movieHeader.ContainsTag(QuickTimeTrackHeaderDirectory.TagCreated))
            {
                return movieHeader.Tags.FirstOrDefault(x => x.Type == QuickTimeMovieHeaderDirectory.TagCreated);
            }

            return null;
        }
    }
}