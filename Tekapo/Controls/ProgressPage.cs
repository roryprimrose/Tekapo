namespace Tekapo.Controls
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;

    /// <summary>
    ///     The <see cref="ProgressPage" /> class is a Wizard page that displays progress to the user.
    /// </summary>
    public partial class ProgressPage : WizardBannerPage
    {
        /// <summary>
        ///     Stores whether the processing is complete.
        /// </summary>
        private bool _processComplete;

        /// <summary>
        ///     Stores the thread that runs the process.
        /// </summary>
        private Thread _processThread;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressPage" /> class.
        /// </summary>
        public ProgressPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Defines the thread switching delegate for string parameter methods.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="System.String" /> value.
        /// </param>
        protected delegate void StringThreadSwitch(string value);

        /// <summary>
        ///     Defines the thread switching delegate for updating the progress bar percentage.
        /// </summary>
        /// <param name="current">
        ///     The current count.
        /// </param>
        /// <param name="total">
        ///     The total count.
        /// </param>
        private delegate void ProgressThreadSwitch(int current, int total);

        /// <summary>
        ///     Finishes the process.
        /// </summary>
        protected virtual void FinishProcess()
        {
            // Mark the search as complete and update the UI
            _processComplete = true;
            OnUpdateWizardSettingsRequiredRequired(new EventArgs());

            // Request a navigation to the next page
            InvokeNavigation(WizardFormNavigationType.Next);
        }

        /// <summary>
        ///     Processes the task.
        /// </summary>
        protected virtual void ProcessTask()
        {
        }

        /// <summary>
        ///     Runs the process.
        /// </summary>
        protected virtual void RunProcess()
        {
            StartProcess();
            ProcessTask();
            FinishProcess();
        }

        /// <summary>
        ///     Sets the progress percentage.
        /// </summary>
        /// <param name="current">
        ///     The current.
        /// </param>
        /// <param name="total">
        ///     The total.
        /// </param>
        protected void SetProgressPercentage(int current, int total)
        {
            // Check if a thread switch is required
            if (SearchProgress.InvokeRequired)
            {
                object[] args = {current, total};

                SearchProgress.Invoke(new ProgressThreadSwitch(SetProgressPercentage), args);

                return;
            }

            // Check if the style is for progress
            if (SearchProgress.Style == ProgressBarStyle.Marquee
                && total > -1)
            {
                // Set the style
                SearchProgress.Style = ProgressBarStyle.Blocks;
                SearchProgress.Maximum = total;
            }
            else if (SearchProgress.Style != ProgressBarStyle.Marquee
                     && total == -1)
            {
                SearchProgress.Style = ProgressBarStyle.Marquee;

                return;
            }

            // Check if the maximum needs to change
            if (total > -1
                && SearchProgress.Maximum != total)
            {
                // Set the new total
                SearchProgress.Maximum = total;
            }

            // Check if there is a valid value
            if (current > -1
                && current <= total)
            {
                // Set the new value
                SearchProgress.Value = current;
            }
        }

        /// <summary>
        ///     Sets the search status.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        protected void SetProgressStatus(string value)
        {
            // Check if a thread switch is required
            if (ProgressStatus.InvokeRequired)
            {
                object[] args = {value};

                ProgressStatus.Invoke(new StringThreadSwitch(SetProgressStatus), args);

                return;
            }

            // Assign the value
            ProgressStatus.Text = value;
        }

        /// <summary>
        ///     Starts the process.
        /// </summary>
        protected virtual void StartProcess()
        {
            // Set up the progress bar and search status
            SetProgressPercentage(-1, -1);
            SetProgressStatus(string.Empty);
        }

        /// <summary>
        ///     Handles the Closing event of the ProgressPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ProgressPage_Closing(object sender, EventArgs e)
        {
            // Stop the file search
            if (_processThread != null)
            {
                // Check if the thread is running
                if ((_processThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    // Abort the thread
                    _processThread.Abort();
                }

                // Destroy the Object
                _processThread = null;
            }
        }

        /// <summary>
        ///     Handles the Opened event of the ProgressPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ProgressPage_Opened(object sender, EventArgs e)
        {
            // Create a start the thread and start the file search
            _processThread = new Thread(RunProcess);
            _processThread.IsBackground = true;
            _processThread.Start();
        }

        /// <summary>
        ///     Handles the Opening event of the ProgressPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ProgressPage_Opening(object sender, EventArgs e)
        {
            // Mark the process as not complete so that the next button will be disabled when the UI displays the page
            _processComplete = false;
        }

        /// <summary>
        ///     Gets the current settings.
        /// </summary>
        /// <value>
        ///     The current settings.
        /// </value>
        public override WizardPageSettings CurrentSettings
        {
            get
            {
                var settings = base.CurrentSettings;

                // Set the enabled state of the next button according to whether the search has completed
                settings.NextButtonSettings.Enabled = _processComplete;

                return settings;
            }
        }
    }
}