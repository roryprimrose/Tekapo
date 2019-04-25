namespace Tekapo.Processing
{
    using System.IO;

    public static class MediaManagerExtensions
    {
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