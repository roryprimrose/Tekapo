using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Tekapo.Processing;
using Tekapo.Properties;

namespace Tekapo.Controls
{
    /// <summary>
    /// The <see cref="ProcessFilesPage"/> is a wizard page used to display the progress of processing the files
    /// specified by the user.
    /// </summary>
    public partial class ProcessFilesPage : ProgressPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessFilesPage"/> class.
        /// </summary>
        public ProcessFilesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Processes the task.
        /// </summary>
        protected override void ProcessTask()
        {
            BindingList<String> items = (BindingList<String>)State[Constants.FileListStateKey];
            Int32 totalItems = items.Count;
            Boolean isRenameTask = (String)State[Constants.TaskStateKey] == Constants.RenameTask;

            // Create the process results
            Results processResults = new Results();

            // Store the results information
            processResults.ProcessRunDate = DateTime.Now.ToLongDateString() + ", " + DateTime.Now.ToShortTimeString();
            processResults.ProcessType = (String)State[Constants.TaskStateKey];

            // Store the process results in state
            State[Constants.ProcessResultsStateKey] = processResults;

            for (Int32 index = 0; index < items.Count; index++)
            {
                String path = items[index];

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
        /// Processes the rename.
        /// </summary>
        /// <param name="path">
        /// The path to process.
        /// </param>
        private void ProcessRename(String path)
        {
            // Calculate the new path
            String renameFormat = (String)State[Constants.NameFormatStateKey];
            Boolean incrementOnCollision = (Boolean)State[Constants.IncrementOnCollisionStateKey];
            Int32 maxCollisionIncrement = Settings.Default.MaxCollisionIncrement;
            DateTime currentTime = JpegInformation.GetPictureTaken(path);
            String newPath = ImageRenaming.GetRenamedPath(
                renameFormat, currentTime, path, incrementOnCollision, maxCollisionIncrement);
            String displayPath = newPath;

            // Store the results
            FileResult result = new FileResult();
            result.OriginalPath = path;
            result.NewPath = newPath;

            // Check if the new path is in the same directory
            if (Path.GetDirectoryName(path).ToUpperInvariant()
                == Path.GetDirectoryName(newPath).ToUpperInvariant())
            {
                // Strip off the path to leave just the file name
                displayPath = Path.GetFileName(newPath);
            }

            // Set the progress status
            String progressMessage = String.Format(
                CultureInfo.CurrentUICulture, Resources.RenameProcessFormat, path, displayPath);
            SetProgressStatus(progressMessage);

            try
            {
                // Check that the file paths are different
                if (path != newPath)
                {
                    // Get the parent directory path
                    String parentDirectory = Path.GetDirectoryName(newPath);

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
                        String newPathWithoutIncrement = ImageRenaming.GetRenamedPath(
                            renameFormat, currentTime, path, false, maxCollisionIncrement);
                        String firstIncrementPath = ImageRenaming.CreateFilePathWithIncrement(
                            newPathWithoutIncrement, 1, maxCollisionIncrement);

                        // Check if the first file needs to be renamed
                        if (File.Exists(newPathWithoutIncrement)
                            && (File.Exists(firstIncrementPath) == false))
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
        /// Processes the time shift.
        /// </summary>
        /// <param name="path">
        /// The path to process.
        /// </param>
        private void ProcessTimeShift(String path)
        {
            // Set the progress status
            String progressMessage = String.Format(CultureInfo.CurrentUICulture, Resources.TimeShiftProcessFormat, path);
            SetProgressStatus(progressMessage);

            // Get the current time of the file
            DateTime currentTime = JpegInformation.GetPictureTaken(path);

            // Shift the time
            DateTime newTime =
                currentTime.AddHours(Convert.ToDouble(State[Constants.ShiftHoursStateKey], CultureInfo.CurrentCulture));
            newTime =
                newTime.AddMinutes(Convert.ToDouble(State[Constants.ShiftMinutesStateKey], CultureInfo.CurrentCulture));
            newTime =
                newTime.AddSeconds(Convert.ToDouble(State[Constants.ShiftSecondsStateKey], CultureInfo.CurrentCulture));
            newTime = newTime.AddYears(Convert.ToInt32(State[Constants.ShiftYearsStateKey], CultureInfo.CurrentCulture));
            newTime =
                newTime.AddMonths(Convert.ToInt32(State[Constants.ShiftMonthsStateKey], CultureInfo.CurrentCulture));
            newTime = newTime.AddDays(Convert.ToInt32(State[Constants.ShiftDaysStateKey], CultureInfo.CurrentCulture));

            // Store the results
            FileResult result = new FileResult();
            result.OriginalPath = path;
            result.OriginalPictureTakenDate = String.Concat(
                currentTime.ToLongDateString(), ", ", currentTime.ToShortTimeString());
            result.NewPictureTakenDate = String.Concat(newTime.ToLongDateString(), ", ", newTime.ToShortTimeString());

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
        /// Gets the process results.
        /// </summary>
        /// <value>
        /// The process results.
        /// </value>
        private Results ProcessResults
        {
            get
            {
                return (Results)State[Constants.ProcessResultsStateKey];
            }
        }
    }
}