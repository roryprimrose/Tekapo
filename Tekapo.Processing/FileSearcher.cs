namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

        public event EventHandler<PathEventArgs> DirectoryFound;

        public event EventHandler<PathProgressEventArgs> FilteringFile;

        public event EventHandler<PathProgressEventArgs> SearchingDirectory;

        public event EventHandler<SearchStageEventArgs> SearchStageChange;

        public IEnumerable<string> FindSupportedFiles(IEnumerable<string> paths, MediaOperationType operationType)
        {
            var sourceData = paths.ToList();
            var sourceDirectories = sourceData.Where(Directory.Exists);
            var sourceFiles = sourceData.Where(File.Exists).ToList();

             SearchStageChange?.Invoke(this, SearchStageEventArgs.For(SearchStage.FindingDirectories));

            var directories = FindDirectories(sourceDirectories, _settings.RecursiveSearch);

            SearchStageChange?.Invoke(this, SearchStageEventArgs.For(SearchStage.FindingFiles));

            var filesFound = FindFiles(directories, _settings.SearchFilterType, _settings.WildcardFilter);

            // Clean up memory
            directories.Clear();

            foreach (var sourceFile in sourceFiles)
            {
                if (filesFound.Contains(sourceFile))
                {
                    continue;
                }
            
                filesFound.AddRange(sourceFiles);
            }
            
            SearchStageChange?.Invoke(this, SearchStageEventArgs.For(SearchStage.FilteringFiles));

            return FilterFiles(filesFound,
                operationType,
                _settings.SearchFilterType,
                _settings.RegularExpressionFilter);
        }

        private IEnumerable<string> FilterFiles(
            IList<string> files,
            MediaOperationType operationType,
            SearchFilterType filterType,
            string regularExpressionPattern)
        {
            var matchingFiles = new List<string>();
            var expressionTest = new Regex(regularExpressionPattern, RegexOptions.Singleline);
            var totalCount = files.Count;

            // Loop through each file
            for (var index = 0; index < totalCount; index++)
            {
                // Get the path
                var path = files[index];

                FilteringFile?.Invoke(this, PathProgressEventArgs.For(path, index + 1, totalCount));

                // Check if the file is a supported type
                if (_mediaManager.IsSupported(path, operationType))
                {
                    if (filterType != SearchFilterType.RegularExpression
                        || expressionTest.IsMatch(path))
                    {
                        // Add the item
                        matchingFiles.Add(path);
                    }
                }
            }

            return matchingFiles;
        }

        private IList<string> FindDirectories(IEnumerable<string> paths, bool recurseDirectories)
        {
            var itemsFound = new List<string>();

            foreach (var path in paths)
            {
                var pathToProcess = path;

                if (pathToProcess.EndsWith("\\") == false)
                {
                    pathToProcess = pathToProcess + "\\";
                }

                if (itemsFound.Contains(pathToProcess))
                {
                    continue;
                }

                itemsFound.Add(pathToProcess);

                FindDirectories(pathToProcess, itemsFound, recurseDirectories);
}

            return itemsFound;
        }

        private void FindDirectories(string path, ICollection<string> directories, bool recurseDirectories)
        {
            if (Directory.Exists(path) == false)
            {
                return;
            }

            try
            {
                // Get the sub-directory paths
                var newPaths = Directory.EnumerateDirectories(path);

                // Loop through each directory path
                foreach (var newPath in newPaths)
                {
                    var pathToProcess = newPath;

                    if (pathToProcess.EndsWith("\\") == false)
                    {
                        pathToProcess = pathToProcess + "\\";
                    }

                    if (directories.Contains(pathToProcess))
                    {
                        return;
                    }

                    directories.Add(pathToProcess);

                    // Update the progress message
                    DirectoryFound?.Invoke(this, PathEventArgs.For(pathToProcess));

                    // Check if we need to recurse
                    if (recurseDirectories)
                    {
                        // SearchExpression the call
                        FindDirectories(pathToProcess, directories, true);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // We can't read this folder
            }
        }

        private List<string> FindFiles(IList<string> directories, SearchFilterType filterType, string wildcardPattern)
        {
            var totalCount = directories.Count;
            var itemsFound = new List<string>();

            // Loop through each directory
            for (var index = 0; index < totalCount; index++)
            {
                // Get the directory
                var directory = directories[index];

                SearchingDirectory?.Invoke(this, PathProgressEventArgs.For(directory, index + 1, totalCount));

                // Get the files for the directory
                string[] newFiles;

                switch (filterType)
                {
                    case SearchFilterType.None:
                    case SearchFilterType.RegularExpression:

                        newFiles = Directory.GetFiles(directory);

                        break;

                    case SearchFilterType.Wildcard:

                        newFiles = Directory.GetFiles(directory, wildcardPattern);

                        break;

                    default:

                        throw new NotSupportedException();
                }

                // Add the new files to the collection
                itemsFound.AddRange(newFiles);
            }

            return itemsFound;
        }
    }
}