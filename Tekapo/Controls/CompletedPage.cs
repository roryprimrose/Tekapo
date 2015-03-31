using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Neovolve.Windows.Forms.Controls;
using Tekapo.Processing;
using Tekapo.Properties;

namespace Tekapo.Controls
{
    /// <summary>
    /// The <see cref="CompletedPage"/> class is used to display the final page in the wizard process.
    /// </summary>
    public partial class CompletedPage : WizardBannerPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompletedPage"/> class.
        /// </summary>
        public CompletedPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Opening event of the CompletedPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void CompletedPage_Opening(Object sender, EventArgs e)
        {
            // Display the results
            Results processingResults = (Results)State[Constants.ProcessResultsStateKey];

            String message = String.Format(
                CultureInfo.CurrentUICulture,
                Resources.SuccessfulProcessedResultsFormat,
                processingResults.FilesSucceeded);

            // Check if there are failed results
            if (processingResults.FilesFailed > 0)
            {
                message += Environment.NewLine
                           +
                           String.Format(
                               CultureInfo.CurrentUICulture,
                               Resources.FailedProcessedResultsFormat,
                               processingResults.FilesFailed);
            }

            Results.Text = message;
        }

        /// <summary>
        /// Handles the Click event of the ViewLog control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void ViewLog_Click(Object sender, EventArgs e)
        {
            // Get a temporary file path
            String filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".htm");

            // Store the path in state
            State[Constants.ResultsLogPathStateKey] = filePath;

            // Write the log
            LogWriter.SaveResultsLog((Results)State[Constants.ProcessResultsStateKey], filePath);

            // Shell the file path
            Process.Start(filePath);
        }
    }
}