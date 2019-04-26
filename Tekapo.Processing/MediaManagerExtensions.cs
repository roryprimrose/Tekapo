namespace Tekapo.Processing
{
    using System;
    using System.IO;
    using System.Linq;

    public static class MediaManagerExtensions
    {
        public static bool IsSupported(this IMediaManager mediaManager, string filePath)
        {
            var fileType = Path.GetExtension(filePath);

            if (string.IsNullOrWhiteSpace(fileType))
            {
                return false;
            }

            var fileTypes = mediaManager.GetSupportedFileTypes();

            return fileTypes.Any(x => string.Equals(x, fileType, StringComparison.OrdinalIgnoreCase));
        }

        public static MediaInfo ReadMediaInfo(IMediaManager mediaManager, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var mediaCreated = mediaManager.ReadMediaCreatedDate(stream);
                var hash = stream.CalculateHash();

                return new MediaInfo {FilePath = filePath, Hash = hash, MediaCreated = mediaCreated};
            }
        }
    }
}