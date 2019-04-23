namespace Tekapo.Processing
{
    using System;

    public interface IMediaManager
    {
        DateTime ReadMediaCreatedDate(string filePath);

        void SetMediaCreatedDate(string filePath, DateTime createdAt);
    }
}