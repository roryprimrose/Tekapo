namespace Tekapo.Controls
{
    using System;
    using System.Globalization;
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
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

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

            if (stateValue is decimal shiftValue)
            {
                return shiftValue;
            }

            var value = Convert.ToString(State[stateKey], CultureInfo.CurrentCulture);

            // Check if the value is a decimal
            if (string.IsNullOrEmpty(value) == false
                && decimal.TryParse(value, out var returnValue))
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
            State[Tekapo.State.ShiftHoursKey] = txtHours.Value;
            State[Tekapo.State.ShiftMinutesKey] = txtMinutes.Value;
            State[Tekapo.State.ShiftSecondsKey] = txtSeconds.Value;
            State[Tekapo.State.ShiftYearsKey] = txtYears.Value;
            State[Tekapo.State.ShiftMonthsKey] = txtMonths.Value;
            State[Tekapo.State.ShiftDaysKey] = txtDays.Value;
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
            txtHours.Value = GetShiftValue(Tekapo.State.ShiftHoursKey);
            txtMinutes.Value = GetShiftValue(Tekapo.State.ShiftMinutesKey);
            txtSeconds.Value = GetShiftValue(Tekapo.State.ShiftSecondsKey);
            txtYears.Value = GetShiftValue(Tekapo.State.ShiftYearsKey);
            txtMonths.Value = GetShiftValue(Tekapo.State.ShiftMonthsKey);
            txtDays.Value = GetShiftValue(Tekapo.State.ShiftDaysKey);
        }
    }
}