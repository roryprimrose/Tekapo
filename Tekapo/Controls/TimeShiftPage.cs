namespace Tekapo.Controls
{
    using System;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Properties;

    /// <summary>
    ///     Time shift page.
    /// </summary>
    public partial class TimeShiftPage : WizardBannerPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TimeShiftPage" /> class.
        /// </summary>
        public TimeShiftPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Determines whether this instance can navigate the specified e.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        /// <returns>
        ///     <c>true</c>if this instance can navigate the specified e; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanNavigate(WizardFormNavigationEventArgs e)
        {
            // Check if the user is clicking next
            if (e.NavigationType == WizardFormNavigationType.Next
                && IsPageValid() == false)
            {
                // The page isn't valid
                return false;
            }

            return base.CanNavigate(e);
        }

        /// <summary>
        ///     Gets the shift value.
        /// </summary>
        /// <param name="stateKey">
        ///     The state key.
        /// </param>
        /// <returns>
        ///     The shift value.
        /// </returns>
        private decimal GetShiftValue(string stateKey)
        {
            var stateValue = State[stateKey];

            if (stateValue is decimal)
            {
                return (decimal) stateValue;
            }

            var value = Convert.ToString(State[stateKey]);

            decimal returnValue;

            // Check if the value is a decimal
            if (string.IsNullOrEmpty(value) == false
                && decimal.TryParse(value, out returnValue))
            {
                return returnValue;
            }

            return 0;
        }

        /// <summary>
        ///     Determines whether the page is valid.
        /// </summary>
        /// <returns>
        ///     <c>true</c>if the page is valid; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPageValid()
        {
            var result = true;

            // Clear the error provider
            ErrorDisplay.Clear();

            if (txtHours.Value == 0
                && txtMinutes.Value == 0
                && txtSeconds.Value == 0
                && txtYears.Value == 0
                && txtMonths.Value == 0
                && txtDays.Value == 0)
            {
                // Set the error provider
                ErrorDisplay.SetError(txtHours, Resources.ErrorNoTimeShiftProvided);
                ErrorDisplay.SetError(txtMinutes, Resources.ErrorNoTimeShiftProvided);
                ErrorDisplay.SetError(txtSeconds, Resources.ErrorNoTimeShiftProvided);
                ErrorDisplay.SetError(txtYears, Resources.ErrorNoTimeShiftProvided);
                ErrorDisplay.SetError(txtMonths, Resources.ErrorNoTimeShiftProvided);
                ErrorDisplay.SetError(txtDays, Resources.ErrorNoTimeShiftProvided);

                // There is no name format
                result = false;
            }

            // Return the result
            return result;
        }

        /// <summary>
        ///     Handles the Closing event of the TimeShiftPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void TimeShiftPage_Closing(object sender, EventArgs e)
        {
            State[Constants.ShiftHoursStateKey] = txtHours.Value;
            State[Constants.ShiftMinutesStateKey] = txtMinutes.Value;
            State[Constants.ShiftSecondsStateKey] = txtSeconds.Value;
            State[Constants.ShiftYearsStateKey] = txtYears.Value;
            State[Constants.ShiftMonthsStateKey] = txtMonths.Value;
            State[Constants.ShiftDaysStateKey] = txtDays.Value;
        }

        /// <summary>
        ///     Handles the Opening event of the TimeShiftPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void TimeShiftPage_Opening(object sender, EventArgs e)
        {
            txtHours.Value = GetShiftValue(Constants.ShiftHoursStateKey);
            txtMinutes.Value = GetShiftValue(Constants.ShiftMinutesStateKey);
            txtSeconds.Value = GetShiftValue(Constants.ShiftSecondsStateKey);
            txtYears.Value = GetShiftValue(Constants.ShiftYearsStateKey);
            txtMonths.Value = GetShiftValue(Constants.ShiftMonthsStateKey);
            txtDays.Value = GetShiftValue(Constants.ShiftDaysStateKey);
        }
    }
}