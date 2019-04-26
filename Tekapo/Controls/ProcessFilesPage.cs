namespace Tekapo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using EnsureThat;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class ProcessFilesPage : ProgressPage
    {
        private readonly IMediaManager _mediaManager;
        private readonly IRenameProcessor _renameProcessor;

        public ProcessFilesPage(IMediaManager mediaManager, IRenameProcessor renameProcessor)
        {
            Ensure.Any.IsNotNull(mediaManager, nameof(mediaManager));
            Ensure.Any.IsNotNull(renameProcessor, nameof(renameProcessor));

            _mediaManager = mediaManager;
            _renameProcessor = renameProcessor;

            InitializeComponent();
        }

        protected override void ProcessTask()
        {
            var items = (BindingList<string>)State[Constants.FileListStateKey];
            var totalItems = items.Count;
            var isRenameTask = (string)State[Constants.TaskStateKey] == Constants.RenameTask;
            var renameConfig = new RenameConfiguration
            {
                RenameFormat = (string) State[Constants.NameFormatStateKey],
                IncrementOnCollision = (bool) State[Constants.IncrementOnCollisionStateKey],
                MaxCollisionIncrement = Settings.Default.MaxCollisionIncrement
            };
            var processResults = new Results
            {
                ProcessRunDate = DateTime.Now.ToLongDateString() + ", " + DateTime.Now.ToShortTimeString(),
                ProcessType = (string) State[Constants.TaskStateKey]
            };

            // Store the process results in state
            State[Constants.ProcessResultsStateKey] = processResults;

            for (var index = 0; index < items.Count; index++)
            {
                var path = items[index];

                // Check if the task is a rename
                if (isRenameTask)
                {
                    // Rename the file
                    ProcessRename(path, renameConfig);
                }
                else
                {
                    // The task is a time shift
                    // Shift the time of the file
                    ProcessTimeShift(path);
                }

                SetProgressPercentage(index + 1, totalItems);
            }
        }

        private void ProcessRename(string path, RenameConfiguration config)
        {
            var result = _renameProcessor.RenameFile(path, config);

            ProcessResults.Add(result);

            if (result.IsSuccessful == false)
            {
                return;
            }

            var displayPath = result.NewPath;

            // Check if the new path is in the same directory
            if (Path.GetDirectoryName(result.OriginalPath).ToUpperInvariant() == Path.GetDirectoryName(result.NewPath).ToUpperInvariant())
            {
                // Strip off the path to leave just the file name
                displayPath = Path.GetFileName(result.NewPath);
            }

            var progressMessage = string.Format(CultureInfo.CurrentCulture,
                Resources.RenameProcessFormat,
                path,
                displayPath);
            SetProgressStatus(progressMessage);
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