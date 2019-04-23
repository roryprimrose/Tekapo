namespace Tekapo.Processing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    public abstract class FileMediaManager : IMediaManager
    {
        [SuppressMessage("Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This is a fallback strategy which must cater for any type of failure.")]
        public virtual DateTime ReadMediaCreatedDate(string filePath)
        {
            // Check if the file exists
            if (File.Exists(filePath) == false)
            {
                return DateTime.Now;
            }

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

        public abstract void SetMediaCreatedDate(string filePath, DateTime createdAt);

        protected abstract DateTime? ResolveMediaCreatedDate(string filePath);
    }
}