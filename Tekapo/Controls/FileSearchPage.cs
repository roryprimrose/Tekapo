using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using Tekapo.Properties;

namespace Tekapo.Controls
{
    /// <summary>
    /// The <see cref="FileSearchPage"/> class is a Wizard page that displays progress to the user as files are being searched.
    /// </summary>
    public partial class FileSearchPage : ProgressPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchPage"/> class.
        /// </summary>
        public FileSearchPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Processes the task.
        /// </summary>
        protected override void ProcessTask()
        {
            // Get the search criteria
            String basePath = (String)State[Constants.SearchPathStateKey];
            Boolean recurseDirectories = (Boolean)State[Constants.SearchSubDirectoriesStateKey];
            SearchFilterType filterType = (SearchFilterType)State[Constants.SearchFilterTypeStateKey];
            String wildcardPattern = (String)State[Constants.SearchFilterWildcardStateKey];
            String regularExpressionPattern = (String)State[Constants.SearchFilterRegularExpressionStateKey];

            // Run the first search stage
            // Get the list of directories involved
            List<String> directories = new List<String>();
            SetStepTitle(Resources.FileSearchDirectoryTitle);
            FindDirectories(basePath, recurseDirectories, directories);

            // Run the second search stage
            // Get list of files involved
            List<String> files = new List<String>();
            SetStepTitle(Resources.FileSearchFileTitle);
            FindFiles(directories, files, filterType, wildcardPattern);

            // Clean up memory
            directories.Clear();

            // Run the third search stage
            // Filter the files involved
            BindingList<String> filteredFiles = new BindingList<String>();
            SetStepTitle(Resources.FileSearchFileFilterTitle);
            FilterFiles(files, filteredFiles, filterType, regularExpressionPattern);

            // Clean up memory
            files.Clear();

            // Store the list of files found
            State[Constants.FileListStateKey] = filteredFiles;
        }

        /// <summary>
        /// Filters the files.
        /// </summary>
        /// <param name="files">
        /// The files.
        /// </param>
        /// <param name="filteredFiles">
        /// The filtered files.
        /// </param>
        /// <param name="filterType">
        /// Type of the filter.
        /// </param>
        /// <param name="regularExpressionPattern">
        /// The regular expression pattern.
        /// </param>
        private void FilterFiles(
            IList<String> files,
            ICollection<String> filteredFiles,
            SearchFilterType filterType,
            String regularExpressionPattern)
        {
            Regex expressionTest = new Regex(regularExpressionPattern, RegexOptions.Singleline);
            Int32 totalCount = files.Count;

            // Loop through each file
            for (Int32 index = 0; index < totalCount; index++)
            {
                // Get the path
                String path = files[index];

                // Update the progress percentage
                SetProgressPercentage(index + 1, totalCount);

                // Update the search status
                SetProgressStatus(path);

                // Check if the file is a supported type
                if (Helper.IsFileSupported(path))
                {
                    if ((filterType != SearchFilterType.RegularExpression)
                        || expressionTest.IsMatch(path))
                    {
                        // Add the item
                        filteredFiles.Add(path);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the directories.
        /// </summary>
        /// <param name="path">
        /// The path to search.
        /// </param>
        /// <param name="recurseDirectories">
        /// If set to <c>true</c>, the method will be called recursively.
        /// </param>
        /// <param name="directories">
        /// The directories.
        /// </param>
        private void FindDirectories(String path, Boolean recurseDirectories, ICollection<String> directories)
        {
            // Add the current path to the list
            directories.Add(path);

            // Get the subdirectory paths
            String[] newPaths = Directory.GetDirectories(path);

            // Loop through each directory path
            for (Int32 index = 0; index < newPaths.Length; index++)
            {
                String newPath = newPaths[index];

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
        /// Finds the files.
        /// </summary>
        /// <param name="directories">
        /// The directories.
        /// </param>
        /// <param name="files">
        /// The files.
        /// </param>
        /// <param name="filterType">
        /// Type of the filter.
        /// </param>
        /// <param name="wildcardPattern">
        /// The wildcard pattern.
        /// </param>
        private void FindFiles(
            IList<String> directories, List<String> files, SearchFilterType filterType, String wildcardPattern)
        {
            Int32 totalCount = directories.Count;

            // Loop through each directory
            for (Int32 index = 0; index < totalCount; index++)
            {
                // Get the directory
                String directory = directories[index];

                // Update the progress percentage
                SetProgressPercentage(index + 1, totalCount);

                // Update the status
                SetProgressStatus(directory);

                // Get the files for the directory
                String[] newFiles;

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
            if ((Boolean)State[Constants.CommandLineArgumentsProcessedStateKey] == false)
            {
                // Update the status
                SetProgressStatus(Resources.ParseCommandLineArguments);

                String[] arguments = Environment.GetCommandLineArgs();

                // Loop through each argument
                for (Int32 index = 0; index < arguments.Length; index++)
                {
                    String argument = arguments[index];

                    // Check if the item is a file path
                    if (File.Exists(argument)
                        && (files.Contains(argument) == false))
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
        /// Sets the step title.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void SetStepTitle(String value)
        {
            // Check if a thread switch is required
            if (StepTitle.InvokeRequired)
            {
                Object[] args = new Object[]
                                    {
                                        value
                                    };

                StepTitle.Invoke(new StringThreadSwitch(SetStepTitle), args);

                return;
            }

            // Assign the value
            StepTitle.Text = value;
        }
    }
}