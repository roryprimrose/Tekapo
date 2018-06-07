using System;

namespace Neovolve.Windows.Forms.Test
{
    /// <summary>
    /// Timer page.
    /// </summary>
    public partial class TimerPage : Controls.WizardBannerPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerPage"/> class.
        /// </summary>
        public TimerPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Tick event of the timer1 control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Timer1_Tick(Object sender, EventArgs e)
        {
            if (progressBar1.Value + progressBar1.Step
                <= progressBar1.Maximum)
            {
                progressBar1.Value += progressBar1.Step;
            }
            else
            {
                timer1.Enabled = false;
                InvokeNavigation(WizardFormNavigationType.Next);
            }
        }

        /// <summary>
        /// Handles the Closing event of the TimerPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void TimerPage_Closing(Object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        /// <summary>
        /// Handles the Opened event of the TimerPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void TimerPage_Opened(Object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            timer1.Enabled = true;
        }
    }
}