namespace Tekapo.Controls
{
    using System;
    using System.Threading;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;

    /// <summary>
    ///     The <see cref="ProcessingPage" /> class is a Wizard page that runs a processing task.
    /// </summary>
    public partial class ProcessingPage : WizardBannerPage
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
        public ProcessingPage()
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
        ///     Finishes the process.
        /// </summary>
        protected virtual void FinishProcess()
        {
            // Mark the search as complete and update the UI
            _processComplete = true;
            OnUpdateWizardSettingsRequired(new EventArgs());

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
        ///     Starts the process.
        /// </summary>
        protected virtual void StartProcess()
        {
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