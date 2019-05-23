namespace Tekapo.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using EnsureThat;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class FileSearchPage : ProgressPage
    {
        private readonly IFileSearcher _fileSearcher;
        private readonly ISettings _settings;

        public FileSearchPage(IFileSearcher fileSearcher, ISettings settings)
        {
            Ensure.Any.IsNotNull(fileSearcher, nameof(fileSearcher));
            Ensure.Any.IsNotNull(settings, nameof(settings));

            _fileSearcher = fileSearcher;
            _settings = settings;

            components = new Container();

            InitializeComponent();

            _fileSearcher.DirectoryFound += FileSearcherOnDirectoryFound;
            _fileSearcher.SearchingDirectory += FileSearcherOnProgress;
            _fileSearcher.FilteringFile += FileSearcherOnProgress;

            var disposeTrigger = new DisposeTrigger(disposing =>
            {
                if (disposing)
                {
                    _fileSearcher.DirectoryFound -= FileSearcherOnDirectoryFound;
                    _fileSearcher.SearchingDirectory -= FileSearcherOnProgress;
                    _fileSearcher.FilteringFile -= FileSearcherOnProgress;
                }
            });

            components.Add(disposeTrigger);
        }

        protected override void ProcessTask()
        {
            var searchPaths = LoadFromCommandLine();

            var searchPath = _settings.SearchPath;

            if (searchPaths.Contains(searchPath) == false)
            {
                searchPaths.Add(searchPath);
            }

            var taskType = (TaskType) State[Tekapo.State.TaskKey];
            var operationType = taskType.AsMediaOperationType();

            var filesFound = _fileSearcher.FindSupportedFiles(searchPaths, operationType).ToList();

            var fileList = new BindingList<string>(filesFound);

            // Store the list of files found
            State[Tekapo.State.FileListKey] = fileList;
        }

        private void FileSearcherOnDirectoryFound(object sender, PathEventArgs e)
        {
            SetProgressStatus(e.Path);
        }

        private void FileSearcherOnProgress(object sender, PathProgressEventArgs e)
        {
            // Update the progress percentage
            SetProgressPercentage(e.CurrentItem, e.TotalItems);

            // Update the status
            SetProgressStatus(e.Path);
        }

        private List<string> LoadFromCommandLine()
        {
            var files = new List<string>();

            // Check if the command line arguments have been processed
            // Update the status
            SetProgressStatus(Resources.ParseCommandLineArguments);

            var arguments = Environment.GetCommandLineArgs();

            // Loop through each argument
            for (var index = 0; index < arguments.Length; index++)
            {
                var argument = arguments[index];
                string path;

                if (File.Exists(argument))
                {
                    // We will process this file
                    path = Path.GetFullPath(argument);
                }
                else if (Directory.Exists(argument))
                {
                    path = Path.GetFullPath(argument);
                }
                else
                {
                    continue;
                }

                // Check if the item is a file path
                if (files.Contains(path))
                {
                    continue;
                }

                // Add the file to the list
                files.Add(path);
            }

            return files;
        }
    }
}