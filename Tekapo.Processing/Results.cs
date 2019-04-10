namespace Tekapo.Processing
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    ///     The <see cref="Tekapo.Processing.Results" /> class is used to store the results from processing files.
    /// </summary>
    [Serializable]
    public class Results
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Tekapo.Processing.Results" /> class.
        /// </summary>
        public Results()
        {
            FileResults = new Collection<FileResult>();
        }

        /// <summary>
        ///     Adds the failed result.
        /// </summary>
        /// <param name="result">
        ///     The result.
        /// </param>
        /// <param name="errorMessage">
        ///     The error message.
        /// </param>
        public void AddFailedResult(FileResult result, string errorMessage)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = errorMessage;
            FilesProcessed++;
            FilesFailed++;

            FileResults.Add(result);
        }

        /// <summary>
        ///     Adds the successful result.
        /// </summary>
        /// <param name="result">
        ///     The result.
        /// </param>
        public void AddSuccessfulResult(FileResult result)
        {
            result.IsSuccessful = true;
            FilesProcessed++;
            FilesSucceeded++;

            FileResults.Add(result);
        }

        /// <summary>
        ///     Gets the file results.
        /// </summary>
        /// <value>
        ///     The file results.
        /// </value>
        public Collection<FileResult> FileResults { get; }

        /// <summary>
        ///     Gets or sets the files failed.
        /// </summary>
        /// <value>
        ///     The files failed.
        /// </value>
        public int FilesFailed { get; set; }

        /// <summary>
        ///     Gets or sets the files processed.
        /// </summary>
        /// <value>
        ///     The files processed.
        /// </value>
        public int FilesProcessed { get; set; }

        /// <summary>
        ///     Gets or sets the files succeeded.
        /// </summary>
        /// <value>
        ///     The files succeeded.
        /// </value>
        public int FilesSucceeded { get; set; }

        /// <summary>
        ///     Gets or sets the process run date.
        /// </summary>
        /// <value>
        ///     The process run date.
        /// </value>
        public string ProcessRunDate { get; set; }

        /// <summary>
        ///     Gets or sets the type of the process.
        /// </summary>
        /// <value>
        ///     The type of the process.
        /// </value>
        public string ProcessType { get; set; }
    }
}