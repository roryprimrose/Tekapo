namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;

    public interface IFileSearcher
    {
        event EventHandler<PathEventArgs> DirectoryFound;

        event EventHandler<PathProgressEventArgs> FilteringFile;

        event EventHandler<PathProgressEventArgs> SearchingDirectory;

        IEnumerable<string> FindSupportedFiles(IEnumerable<string> paths, MediaOperationType operationType);
    }
}