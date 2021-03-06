namespace Tekapo.Controls
{
    using System;
    using Neovolve.Windows.Forms.Controls;

    /// <summary>
    ///     The <see cref="ChooseTaskPage" /> class is a Wizard page that allows the user to choose which task they want to
    ///     process.
    /// </summary>
    public partial class ChooseTaskPage : WizardBannerPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChooseTaskPage" /> class.
        /// </summary>
        public ChooseTaskPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the Closing event of the ChooseTaskPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ChooseTaskPage_Closing(object sender, EventArgs e)
        {
            if (Rename.Checked)
            {
                State[Tekapo.State.TaskKey] = TaskType.RenameTask;
            }
            else
            {
                State[Tekapo.State.TaskKey] = TaskType.ShiftTask;
            }
        }

        /// <summary>
        ///     Handles the Opening event of the ChooseTaskPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void ChooseTaskPage_Opening(object sender, EventArgs e)
        {
            Rename.Checked = (TaskType) State[Tekapo.State.TaskKey] == TaskType.RenameTask;
            Shift.Checked = (TaskType) State[Tekapo.State.TaskKey] == TaskType.ShiftTask;
        }
    }
}