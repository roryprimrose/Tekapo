namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IMediaManager
    {
        bool CanProcess(Stream stream);

        IEnumerable<string> GetSupportedFileTypes();

        DateTime? ReadMediaCreatedDate(Stream stream);

        Stream SetMediaCreatedDate(Stream stream, DateTime createdAt);
    }
}