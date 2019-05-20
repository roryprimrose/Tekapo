namespace Tekapo
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ExecutionContext : IExecutionContext
    {
        public ExecutionContext(IEnumerable<string> args)
        {
            if (args == null)
            {
                // There are no arguments to evaluate
                return;
            }

            // Determine the search path
            var directories = args.Where(Directory.Exists).Select(x => x).ToList();

            if (directories.Count == 1)
            {
                SearchPath = directories[0];
            }
        }

        public string SearchPath { get; }
    }
}