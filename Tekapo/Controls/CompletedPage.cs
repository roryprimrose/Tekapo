namespace Tekapo.Controls
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Processing;
    using Tekapo.Properties;

    /// <summary>
    ///     The <see cref="CompletedPage" /> class is used to display the final page in the wizard process.
    /// </summary>
    public partial class CompletedPage : WizardBannerPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CompletedPage" /> class.
        /// </summary>
        public CompletedPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the Opening event of the CompletedPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void CompletedPage_Opening(object sender, EventArgs e)
        {
            // Display the results
            var processingResults = (Results) State[Tekapo.State.ProcessResultsKey];

            var message = string.Format(CultureInfo.CurrentCulture,
                Resources.SuccessfulProcessedResultsFormat,
                processingResults.FilesSucceeded);

            // Check if there are failed results
            if (processingResults.FilesFailed > 0)
            {
                message += Environment.NewLine + string.Format(CultureInfo.CurrentCulture,
                               Resources.FailedProcessedResultsFormat,
                               processingResults.FilesFailed);
            }

            Results.Text = message;
        }

        /// <summary>
        ///     Handles the Click event of the ViewLog control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ViewLog_Click(object sender, EventArgs e)
        {
            // Get a temporary file path
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".htm");

            // Store the path in state
            State[Tekapo.State.ResultsLogPathKey] = filePath;

            // Write the log
            LogWriter.SaveResultsLog((Results) State[Tekapo.State.ProcessResultsKey], filePath);

            // Shell the file path
            Process.Start(filePath);
        }
    }
}