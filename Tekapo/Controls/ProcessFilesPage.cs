namespace Tekapo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using Tekapo.Processing;
    using Tekapo.Properties;

    /// <summary>
    ///     The <see cref="ProcessFilesPage" /> is a wizard page used to display the progress of processing the files
    ///     specified by the user.
    /// </summary>
    public partial class ProcessFilesPage : ProgressPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessFilesPage" /> class.
        /// </summary>
        public ProcessFilesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Processes the task.
        /// </summary>
        protected override void ProcessTask()
        {
            var items = (BindingList<string>) State[Constants.FileListStateKey];
            var totalItems = items.Count;
            var isRenameTask = (string) State[Constants.TaskStateKey] == Constants.RenameTask;

            // Create the process results
            var processResults = new Results();

            // Store the results information
            processResults.ProcessRunDate = DateTime.Now.ToLongDateString() + ", " + DateTime.Now.ToShortTimeString();
            processResults.ProcessType = (string) State[Constants.TaskStateKey];

            // Store the process results in state
            State[Constants.ProcessResultsStateKey] = processResults;

            for (var index = 0; index < items.Count; index++)
            {
                var path = items[index];

                // Update the status
                SetProgressPercentage(index + 1, totalItems);

                // Check if the task is a rename
                if (isRenameTask)
                {
                    // Rename the file
                    ProcessRename(path);
                }
                else
                {
                    // The task is a time shift
                    // Shift the time of the file
                    ProcessTimeShift(path);
                }
            }
        }

        /// <summary>
        ///     Processes the rename.
        /// </summary>
        /// <param name="path">
        ///     The path to process.
        /// </param>
        private void ProcessRename(string path)
        {
            // Calculate the new path
            var renameFormat = (string) State[Constants.NameFormatStateKey];
            var incrementOnCollision = (bool) State[Constants.IncrementOnCollisionStateKey];
            var maxCollisionIncrement = Settings.Default.MaxCollisionIncrement;
            var currentTime = JpegInformation.GetPictureTaken(path);
            var newPath = ImageRenaming.GetRenamedPath(renameFormat,
                currentTime,
                path,
                incrementOnCollision,
                maxCollisionIncrement);
            var displayPath = newPath;

            // Store the results
            var result = new FileResult();
            result.OriginalPath = path;
            result.NewPath = newPath;

            // Check if the new path is in the same directory
            if (Path.GetDirectoryName(path).ToUpperInvariant() == Path.GetDirectoryName(newPath).ToUpperInvariant())
            {
                // Strip off the path to leave just the file name
                displayPath = Path.GetFileName(newPath);
            }

            // Set the progress status
            var progressMessage = string.Format(CultureInfo.CurrentUICulture,
                Resources.RenameProcessFormat,
                path,
                displayPath);
            SetProgressStatus(progressMessage);

            try
            {
                // Check that the file paths are different
                if (path != newPath)
                {
                    // Get the parent directory path
                    var parentDirectory = Path.GetDirectoryName(newPath);

                    // Check if the directory exists
                    if (Directory.Exists(parentDirectory) == false)
                    {
                        // Create the directory
                        Directory.CreateDirectory(parentDirectory);
                    }

                    // Check if collision checks are in place
                    if (incrementOnCollision)
                    {
                        // Get the file named without an increment and the path with the first increment
                        var newPathWithoutIncrement = ImageRenaming.GetRenamedPath(renameFormat,
                            currentTime,
                            path,
                            false,
                            maxCollisionIncrement);
                        var firstIncrementPath = ImageRenaming.CreateFilePathWithIncrement(
                            newPathWithoutIncrement,
                            1,
                            maxCollisionIncrement);

                        // Check if the first file needs to be renamed
                        if (File.Exists(newPathWithoutIncrement)
                            && File.Exists(firstIncrementPath) == false)
                        {
                            // Rename the original file as the first increment
                            File.Move(newPathWithoutIncrement, firstIncrementPath);
                        }
                    }

                    // Rename the file
                    File.Move(path, newPath);
                }

                ProcessResults.AddSuccessfulResult(result);
            }
            catch (IOException ex)
            {
                // Store the results
                ProcessResults.AddFailedResult(result, ex.Message);
            }
        }

        /// <summary>
        ///     Processes the time shift.
        /// </summary>
        /// <param name="path">
        ///     The path to process.
        /// </param>
        private void ProcessTimeShift(string path)
        {
            // Set the progress status
            var progressMessage =
                string.Format(CultureInfo.CurrentUICulture, Resources.TimeShiftProcessFormat, path);
            SetProgressStatus(progressMessage);

            // Get the current time of the file
            var currentTime = JpegInformation.GetPictureTaken(path);

            // Shift the time
            var newTime =
                currentTime.AddHours(Convert.ToDouble(State[Constants.ShiftHoursStateKey], CultureInfo.CurrentCulture));
            newTime = newTime.AddMinutes(Convert.ToDouble(State[Constants.ShiftMinutesStateKey],
                CultureInfo.CurrentCulture));
            newTime = newTime.AddSeconds(Convert.ToDouble(State[Constants.ShiftSecondsStateKey],
                CultureInfo.CurrentCulture));
            newTime = newTime.AddYears(Convert.ToInt32(State[Constants.ShiftYearsStateKey],
                CultureInfo.CurrentCulture));
            newTime = newTime.AddMonths(Convert.ToInt32(State[Constants.ShiftMonthsStateKey],
                CultureInfo.CurrentCulture));
            newTime = newTime.AddDays(Convert.ToInt32(State[Constants.ShiftDaysStateKey], CultureInfo.CurrentCulture));

            // Store the results
            var result = new FileResult();
            result.OriginalPath = path;
            result.OriginalPictureTakenDate = string.Concat(
                currentTime.ToLongDateString(),
                ", ",
                currentTime.ToShortTimeString());
            result.NewPictureTakenDate = string.Concat(newTime.ToLongDateString(), ", ", newTime.ToShortTimeString());

            try
            {
                // Store the new time in the file
                JpegInformation.SetPictureTaken(path, newTime);

                ProcessResults.AddSuccessfulResult(result);
            }
            catch (IOException ex)
            {
                // Store the results
                ProcessResults.AddFailedResult(result, ex.Message);
            }
        }

        /// <summary>
        ///     Gets the process results.
        /// </summary>
        /// <value>
        ///     The process results.
        /// </value>
        private Results ProcessResults { get { return (Results) State[Constants.ProcessResultsStateKey]; } }
    }
}