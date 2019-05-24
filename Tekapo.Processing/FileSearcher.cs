namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using EnsureThat;

    public class FileSearcher : IFileSearcher
    {
        private readonly IMediaManager _mediaManager;
        private readonly ISettings _settings;

        public FileSearcher(IMediaManager mediaManager, ISettings settings)
        {
            Ensure.Any.IsNotNull(mediaManager, nameof(mediaManager));
            Ensure.Any.IsNotNull(settings, nameof(settings));

            _mediaManager = mediaManager;
            _settings = settings;
        }

        public event EventHandler<PathEventArgs> EvaluatingPath;

        public IEnumerable<string> FindSupportedFiles(IEnumerable<string> paths, MediaOperationType operationType)
        {
            var context = BuildSearchContext(operationType);

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    if (IsSupportedFile(path, context))
                    {
                        yield return path;
                    }
                }
                else if (Directory.Exists(path))
                {
                    var files = SearchDirectory(path, context);

                    foreach (var file in files)
                    {
                        yield return file;
                    }
                }
            }
        }

        private SearchContext BuildSearchContext(MediaOperationType operationType)
        {
            var context = new SearchContext
            {
                RecursiveSearch = _settings.RecursiveSearch,
                FilterType = _settings.SearchFilterType,
                WildcardPattern = "*",
                OperationType = operationType
            };

            if (context.FilterType == SearchFilterType.Wildcard)
            {
                context.WildcardPattern = _settings.WildcardFilter ?? "*";

                // Create a regex of the wildcard to apply against files in the paths variable
                var wildcardExpressionPattern = "^" + Regex.Escape(context.WildcardPattern).Replace("\\*", ".*") + "$";
                var wildcardExpression = new Regex(wildcardExpressionPattern);

                context.WildcardExpression = wildcardExpression;
            }

            if (string.IsNullOrWhiteSpace(_settings.RegularExpressionFilter) == false)
            {
                context.RegularExpression = new Regex(_settings.RegularExpressionFilter);
            }

            return context;
        }

        private bool IsSupportedFile(string path, SearchContext context)
        {
            if (context.FilesProcessed.Contains(path))
            {
                return false;
            }

            // We don't care if the file is supported and could be processed, we care whether we need to evaluate it again
            context.FilesProcessed.Add(path);

            var filename = Path.GetFileName(path);

            if (context.FilterType == SearchFilterType.RegularExpression
                && string.IsNullOrWhiteSpace(filename) == false
                && context.RegularExpression.IsMatch(filename) == false)
            {
                // This is a regex check and neither the file path or file name  matches the expression
                return false;
            }

            if (context.FilterType == SearchFilterType.Wildcard
                && string.IsNullOrWhiteSpace(filename) == false
                && context.WildcardExpression.IsMatch(filename) == false)
            {
                // This is a wildcard check and neither the file path or file name  matches the expression
                return false;
            }

            // Check if the file is a supported type
            if (_mediaManager.IsSupported(path, context.OperationType))
            {
                return true;
            }

            return false;
        }

        private IEnumerable<string> SearchDirectory(string path, SearchContext context)
        {
            var fullPath = Path.GetFullPath(path);

            if (fullPath.EndsWith("\\") == false)
            {
                fullPath += "\\";
            }

            if (context.DirectoriesProcessed.Contains(fullPath))
            {
                yield break;
            }

            context.DirectoriesProcessed.Add(fullPath);

            EvaluatingPath?.Invoke(this, PathEventArgs.For(fullPath));

            IEnumerable<string> files;

            try
            {
                files = Directory.EnumerateFiles(fullPath, context.WildcardPattern, SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                // We can't read this folder
                files = new List<string>();
            }

            foreach (var file in files)
            {
                if (IsSupportedFile(file, context))
                {
                    yield return file;
                }
            }

            if (context.RecursiveSearch == false)
            {
                yield break;
            }

            IEnumerable<string> directories;

            try
            {
                directories = Directory.EnumerateDirectories(fullPath, "*", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                // We can't read this folder
                directories = new List<string>();
            }

            foreach (var directory in directories)
            {
                var childFiles = SearchDirectory(directory, context);

                foreach (var childFile in childFiles)
                {
                    yield return childFile;
                }
            }
        }

        private class SearchContext
        {
            public List<string> DirectoriesProcessed { get; } = new List<string>();

            public List<string> FilesProcessed { get; } = new List<string>();

            public SearchFilterType FilterType { get; set; }

            public MediaOperationType OperationType { get; set; }

            public bool RecursiveSearch { get; set; }

            public Regex RegularExpression { get; set; }

            public Regex WildcardExpression { get; set; }

            public string WildcardPattern { get; set; }
        }
    }
}