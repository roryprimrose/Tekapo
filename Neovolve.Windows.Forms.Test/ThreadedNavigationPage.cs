namespace Neovolve.Windows.Forms.Test
{
    using System;
    using System.Threading;
    using Neovolve.Windows.Forms.Controls;

    /// <summary>
    ///     Threaded navigation page.
    /// </summary>
    public partial class ThreadedNavigationPage : WizardPage
    {
        /// <summary>
        ///     Stores the worker thread.
        /// </summary>
        private Thread _workerThread;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThreadedNavigationPage" /> class.
        /// </summary>
        public ThreadedNavigationPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Run process.
        /// </summary>
        private void RunProcess()
        {
            Thread.Sleep(2000);

            InvokeNavigation(WizardFormNavigationType.Next);
        }

        /// <summary>
        ///     Handles the Closing event of the ThreadedNavigation control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ThreadedNavigation_Closing(object sender, EventArgs e)
        {
            if (_workerThread != null)
            {
                if ((_workerThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    _workerThread.Abort();
                }

                _workerThread = null;
            }
        }

        /// <summary>
        ///     Handles the Opening event of the ThreadedNavigation control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ThreadedNavigation_Opening(object sender, EventArgs e)
        {
            // Create the thread and start it
            _workerThread = new Thread(RunProcess);
            _workerThread.IsBackground = true;
            _workerThread.Start();
        }
    }
}