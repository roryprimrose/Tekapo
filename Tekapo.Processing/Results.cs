namespace Tekapo.Processing
{
    using System;
    using System.Collections.ObjectModel;
    using EnsureThat;

    [Serializable]
    public class Results
    {
        public Results()
        {
            FileResults = new Collection<FileResult>();
        }

        public void Add(FileResult result)
        {
            Ensure.Any.IsNotNull(result, nameof(result));

            FileResults.Add(result);

            FilesProcessed++;

            if (result.IsSuccessful)
            {
                FilesSucceeded++;
            }
            else
            {
                FilesFailed++;
            }
        }

        public void AddFailedResult(FileResult result, string errorMessage)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            result.IsSuccessful = false;
            result.ErrorMessage = errorMessage;
            FilesProcessed++;
            FilesFailed++;

            FileResults.Add(result);
        }

        public void AddSuccessfulResult(FileResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            result.IsSuccessful = true;
            FilesProcessed++;
            FilesSucceeded++;

            FileResults.Add(result);
        }

        public Collection<FileResult> FileResults { get; }

        public int FilesFailed { get; set; }

        public int FilesProcessed { get; set; }

        public int FilesSucceeded { get; set; }

        public string ProcessRunDate { get; set; }
    }
}