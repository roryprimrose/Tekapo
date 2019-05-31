namespace Tekapo
{
    using System.Collections.Generic;

    public interface IExecutionContext
    {
        string SearchDirectory { get; }

        IReadOnlyCollection<string> SearchPaths { get; }
    }
}