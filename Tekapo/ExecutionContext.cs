namespace Tekapo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    public class ExecutionContext : IExecutionContext
    {
        public ExecutionContext(IEnumerable<string> args)
        {
            if (args == null)
            {
                var emptyList = new List<string>();

                SearchPaths = new ReadOnlyCollection<string>(emptyList);

                // There are no arguments to evaluate
                return;
            }

            // Determine the search path
            var arguments = args.ToList();

            var directories = arguments.Where(Directory.Exists).ToList();
            var files = arguments.Where(File.Exists).ToList();

            if (files.Count == 0
                && directories.Count == 1)
            {
                // There is only one directory, we will use it as the single search directory
                SearchDirectory = directories[0];
                SearchPaths = new List<string>();
            }
            else
            {
                var entries = directories.Union(files).ToList();

                SearchPaths = new ReadOnlyCollection<string>(entries);
            }
        }

        public string SearchDirectory { get; }

        public IReadOnlyCollection<string> SearchPaths { get; }
    }
}