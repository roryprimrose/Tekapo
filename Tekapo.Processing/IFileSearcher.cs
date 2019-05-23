namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;

    public interface IFileSearcher
    {
        event EventHandler<PathEventArgs> DirectoryFound;

        event EventHandler<PathProgressEventArgs> FilteringFile;

        event EventHandler<PathProgressEventArgs> SearchingDirectory;

        event EventHandler<SearchStageEventArgs> SearchStageChange;

        IEnumerable<string> FindSupportedFiles(IEnumerable<string> paths, MediaOperationType operationType);
    }
}