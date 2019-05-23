namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;

    public interface IFileSearcher
    {
        event EventHandler<PathEventArgs> EvaluatingPath;

        IEnumerable<string> FindSupportedFiles(IEnumerable<string> paths, MediaOperationType operationType);
    }
}