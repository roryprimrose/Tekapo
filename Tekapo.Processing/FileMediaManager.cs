namespace Tekapo.Processing
{
    using System;
    using System.IO;

    public abstract class FileMediaManager : IMediaManager
    {
        public virtual DateTime ReadMediaCreatedDate(string filePath)
        {
            DateTime? mediaCreatedDate = null;

            try
            {
                mediaCreatedDate = ResolveMediaCreatedDate(filePath);
            }
            catch (Exception)
            {
                // Ignore any failure to read the time
            }

            if (mediaCreatedDate.HasValue)
            {
                return mediaCreatedDate.Value;
            }

            // We didn't get a media created date. Use the generic file based information instead.
            // Take the file created or last modified date, whichever is earlier
            var fileDetails = new FileInfo(filePath);
            var creationTime = fileDetails.CreationTime;
            var lastWriteTime = fileDetails.LastWriteTimeUtc;

            if (creationTime < lastWriteTime)
            {
                return creationTime;
            }

            return lastWriteTime;
        }

        protected abstract DateTime? ResolveMediaCreatedDate(string filePath);

        public abstract void SetMediaCreatedDate(string filePath, DateTime createdAt);
    }
}