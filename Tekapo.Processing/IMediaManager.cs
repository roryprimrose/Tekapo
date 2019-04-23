namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;

    public interface IMediaManager
    {
        IEnumerable<string> GetSupportedFileTypes();

        DateTime ReadMediaCreatedDate(string filePath);

        void SetMediaCreatedDate(string filePath, DateTime createdAt);
    }
}