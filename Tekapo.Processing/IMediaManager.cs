namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IMediaManager
    {
        IEnumerable<string> GetSupportedFileTypes();

        bool IsSupported(Stream stream);

        DateTime? ReadMediaCreatedDate(Stream stream);

        Stream SetMediaCreatedDate(Stream stream, DateTime createdAt);
    }
}