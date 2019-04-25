namespace Tekapo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class ProcessFilesPage : ProgressPage
    {
        private readonly IMediaManager _mediaManager;
        private readonly IPathManager _pathManager;

        public ProcessFilesPage(IMediaManager mediaManager, IPathManager pathManager)
        {
            _mediaManager = mediaManager;
            _pathManager = pathManager;

            InitializeComponent();
        }

        protected override void ProcessTask()
        {
            var items = (BindingList<string>)State[Constants.FileListStateKey];
            var totalItems = items.Count;
            var isRenameTask = (string)State[Constants.TaskStateKey] == Constants.RenameTask;

            // Create the process results
            var processResults = new Results();

            // Store the results information
            processResults.ProcessRunDate = DateTime.Now.ToLongDateString() + ", " + DateTime.Now.ToShortTimeString();
            processResults.ProcessType = (string)State[Constants.TaskStateKey];

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

        private void ProcessRename(string path)
        {
            // Calculate the new path
            var renameFormat = (string)State[Constants.NameFormatStateKey];
            var incrementOnCollision = (bool)State[Constants.IncrementOnCollisionStateKey];
            var maxCollisionIncrement = Settings.Default.MaxCollisionIncrement;
            var result = new FileResult { OriginalPath = path };

            DateTime? currentTime;

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                currentTime = _mediaManager.ReadMediaCreatedDate(stream);
            }

            if (currentTime == null)
            {
                ProcessResults.AddFailedResult(result, "The date and time the media was created could not be determined, skipping this file.");

                return;
            }

            var newPath = _pathManager.GetRenamedPath(renameFormat,
                currentTime.Value,
                path,
                incrementOnCollision,
                maxCollisionIncrement);

            result.NewPath = newPath;

            var displayPath = newPath;

            // Store the results

            // Check if the new path is in the same directory
            if (Path.GetDirectoryName(path).ToUpperInvariant() == Path.GetDirectoryName(newPath).ToUpperInvariant())
            {
                // Strip off the path to leave just the file name
                displayPath = Path.GetFileName(newPath);
            }

            // Set the progress status
            var progressMessage = string.Format(CultureInfo.CurrentCulture,
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
                        var newPathWithoutIncrement = _pathManager.GetRenamedPath(renameFormat,
                            currentTime.Value,
                            path,
                            false,
                            maxCollisionIncrement);
                        var firstIncrementPath = _pathManager.CreateFilePathWithIncrement(
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

        private void ProcessTimeShift(string path)
        {
            // Set the progress status
            var progressMessage = string.Format(CultureInfo.CurrentCulture, Resources.TimeShiftProcessFormat, path);
            SetProgressStatus(progressMessage);

            var result = new FileResult { OriginalPath = path };

            // Get the current time of the file
            Stream updatedStream;

            using (var originalStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                var currentTime = _mediaManager.ReadMediaCreatedDate(originalStream);

                if (currentTime == null)
                {
                    ProcessResults.AddFailedResult(result, "The date and time the media was created could not be determined, skipping this file.");

                    return;
                }

                // Shift the time
                var newTime =
                    currentTime.Value.AddHours(Convert.ToDouble(State[Constants.ShiftHoursStateKey], CultureInfo.CurrentCulture));
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
                result.OriginalMediaCreatedDate = string.Concat(
                    currentTime.Value.ToLongDateString(),
                    ", ",
                    currentTime.Value.ToShortTimeString());
                result.NewMediaCreatedDate = string.Concat(newTime.ToLongDateString(), ", ", newTime.ToShortTimeString());

                try
                {
                    // Store the new time in the file
                    updatedStream = _mediaManager.SetMediaCreatedDate(originalStream, newTime);

                }
                catch (IOException ex)
                {
                    // Store the results
                    ProcessResults.AddFailedResult(result, ex.Message);

                    return;
                }
            }

            updatedStream.Position = 0;

            using (var outputStream = File.Open(path, FileMode.Open, FileAccess.Write))
            {
                updatedStream.CopyTo(outputStream);

                outputStream.Flush();
            }

            ProcessResults.AddSuccessfulResult(result);
        }

        private Results ProcessResults => (Results)State[Constants.ProcessResultsStateKey];
    }
}