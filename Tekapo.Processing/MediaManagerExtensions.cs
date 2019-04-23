namespace Tekapo.Processing
{
    using System;
    using System.IO;
    using System.Linq;

    public static class MediaManagerExtensions
    {
        public static bool IsSupported(this IMediaManager mediaManager, string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            var supportedTypes = mediaManager.GetSupportedFileTypes();

            return supportedTypes.Any(x => x.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}