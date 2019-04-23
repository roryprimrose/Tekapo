namespace Tekapo.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text.RegularExpressions;
    using Tekapo.Properties;

    /// <summary>
    ///     The <see cref="FileSearchPage" /> class is a Wizard page that displays progress to the user as files are being
    ///     searched.
    /// </summary>
    public partial class FileSearchPage : ProgressPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileSearchPage" /> class.
        /// </summary>
        public FileSearchPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Processes the task.
        /// </summary>
        protected override void ProcessTask()
        {
            // Get the search criteria
            var basePath = (string) State[Constants.SearchPathStateKey];
            var recurseDirectories = (bool) State[Constants.SearchSubDirectoriesStateKey];
            var filterType = (SearchFilterType) State[Constants.SearchFilterTypeStateKey];
            var wildcardPattern = (string) State[Constants.SearchFilterWildcardStateKey];
            var regularExpressionPattern = (string) State[Constants.SearchFilterRegularExpressionStateKey];

            // Run the first search stage
            // Get the list of directories involved
            var directories = new List<string>();
            SetStepTitle(Resources.FileSearchDirectoryTitle);
            FindDirectories(basePath, recurseDirectories, directories);

            // Run the second search stage
            // Get list of files involved
            var files = new List<string>();
            SetStepTitle(Resources.FileSearchFileTitle);
            FindFiles(directories, files, filterType, wildcardPattern);

            // Clean up memory
            directories.Clear();

            // Run the third search stage
            // Filter the files involved
            var filteredFiles = new BindingList<string>();
            SetStepTitle(Resources.FileSearchFileFilterTitle);
            FilterFiles(files, filteredFiles, filterType, regularExpressionPattern);

            // Clean up memory
            files.Clear();

            // Store the list of files found
            State[Constants.FileListStateKey] = filteredFiles;
        }

        /// <summary>
        ///     Filters the files.
        /// </summary>
        /// <param name="files">
        ///     The files.
        /// </param>
        /// <param name="filteredFiles">
        ///     The filtered files.
        /// </param>
        /// <param name="filterType">
        ///     Type of the filter.
        /// </param>
        /// <param name="regularExpressionPattern">
        ///     The regular expression pattern.
        /// </param>
        private void FilterFiles(
            IList<string> files,
            ICollection<string> filteredFiles,
            SearchFilterType filterType,
            string regularExpressionPattern)
        {
            var expressionTest = new Regex(regularExpressionPattern, RegexOptions.Singleline);
            var totalCount = files.Count;

            // Loop through each file
            for (var index = 0; index < totalCount; index++)
            {
                // Get the path
                var path = files[index];

                // Update the progress percentage
                SetProgressPercentage(index + 1, totalCount);

                // Update the search status
                SetProgressStatus(path);

                // Check if the file is a supported type
                if (Helper.IsFileSupported(path))
                {
                    if (filterType != SearchFilterType.RegularExpression
                        || expressionTest.IsMatch(path))
                    {
                        // Add the item
                        filteredFiles.Add(path);
                    }
                }
            }
        }

        /// <summary>
        ///     Finds the directories.
        /// </summary>
        /// <param name="path">
        ///     The path to search.
        /// </param>
        /// <param name="recurseDirectories">
        ///     If set to <c>true</c>, the method will be called recursively.
        /// </param>
        /// <param name="directories">
        ///     The directories.
        /// </param>
        private void FindDirectories(string path, bool recurseDirectories, ICollection<string> directories)
        {
            // Add the current path to the list
            directories.Add(path);

            // Get the subdirectory paths
            var newPaths = Directory.GetDirectories(path);

            // Loop through each directory path
            for (var index = 0; index < newPaths.Length; index++)
            {
                var newPath = newPaths[index];

                // Update the progress message
                SetProgressStatus(newPath);

                // Check if we need to recurse
                if (recurseDirectories)
                {
                    // SearchExpression the call
                    FindDirectories(newPath, true, directories);
                }
            }
        }

        /// <summary>
        ///     Finds the files.
        /// </summary>
        /// <param name="directories">
        ///     The directories.
        /// </param>
        /// <param name="files">
        ///     The files.
        /// </param>
        /// <param name="filterType">
        ///     Type of the filter.
        /// </param>
        /// <param name="wildcardPattern">
        ///     The wildcard pattern.
        /// </param>
        private void FindFiles(
            IList<string> directories,
            List<string> files,
            SearchFilterType filterType,
            string wildcardPattern)
        {
            var totalCount = directories.Count;

            // Loop through each directory
            for (var index = 0; index < totalCount; index++)
            {
                // Get the directory
                var directory = directories[index];

                // Update the progress percentage
                SetProgressPercentage(index + 1, totalCount);

                // Update the status
                SetProgressStatus(directory);

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
                files.AddRange(newFiles);
            }

            // Check if the command line arguments have been processed
            if ((bool) State[Constants.CommandLineArgumentsProcessedStateKey] == false)
            {
                // Update the status
                SetProgressStatus(Resources.ParseCommandLineArguments);

                var arguments = Environment.GetCommandLineArgs();

                // Loop through each argument
                for (var index = 0; index < arguments.Length; index++)
                {
                    var argument = arguments[index];

                    // Check if the item is a file path
                    if (File.Exists(argument)
                        && files.Contains(argument) == false)
                    {
                        // Add the file to the list
                        files.Add(argument);
                    }
                }

                // Update the state value
                State[Constants.CommandLineArgumentsProcessedStateKey] = true;
            }
        }

        /// <summary>
        ///     Sets the step title.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        private void SetStepTitle(string value)
        {
            // Check if a thread switch is required
            if (StepTitle.InvokeRequired)
            {
                object[] args = {value};

                StepTitle.Invoke(new StringThreadSwitch(SetStepTitle), args);

                return;
            }

            // Assign the value
            StepTitle.Text = value;
        }
    }
}