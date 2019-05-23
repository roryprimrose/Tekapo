namespace Tekapo.Controls
{
    using System.Diagnostics;
    using System.Windows.Forms;

    /// <summary>
    ///     The <see cref="ProgressPage" /> class is a Wizard page that displays progress to the user.
    /// </summary>
    public partial class ProgressPage : ProcessingPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressPage" /> class.
        /// </summary>
        public ProgressPage()
        {
            InitializeComponent();
        }

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

            Debug.WriteLine(value);

            // Assign the value
            ProgressStatus.Text = value;
        }

        protected override void StartProcess()
        {
            // Set up the progress bar and search status
            SetProgressPercentage(-1, -1);
            SetProgressStatus(string.Empty);
        }
    }
}