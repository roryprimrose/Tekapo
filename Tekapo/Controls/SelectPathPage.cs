namespace Tekapo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using EnsureThat;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Processing;
    using Tekapo.Properties;

    /// <summary>
    ///     The <see cref="Tekapo.Controls.SelectPathPage" /> class is a Wizard page that allows the user to select a path to
    ///     search for
    ///     images to process.
    /// </summary>
    public partial class SelectPathPage : WizardBannerPage
    {
        private readonly ISettings _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tekapo.Controls.SelectPathPage" /> class.
        /// </summary>
        public SelectPathPage(ISettings settings)
        {
            Ensure.Any.IsNotNull(settings, nameof(settings));

            _settings = settings;

            InitializeComponent();
        }

        /// <summary>
        ///     Determines whether this instance can navigate the specified e.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="T:Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
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
        ///     Handles the Click event of the Browse control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Browse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = _settings.SearchPath;
                dialog.Description = Resources.SelectPathDescription;
                dialog.ShowNewFolderButton = false;

                // Check if the user clicked OK
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Store the path
                    SearchPath.Text = dialog.SelectedPath;
                }
            }
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
            errProvider.Clear();

            if (string.IsNullOrEmpty(SearchPath.Text))
            {
                // Set the error provider
                errProvider.SetError(SearchPath, Resources.ErrorNoSearchPathProvided);

                // There is no path
                result = false;
            }
            else if (Directory.Exists(SearchPath.Text) == false)
            {
                // Set the error provider
                errProvider.SetError(SearchPath, Resources.ErrorSearchPathDoesNotExist);

                // The directory specified doesn't exist
                result = false;
            }

            if (UseWildcard.Checked
                && string.IsNullOrEmpty(Wildcard.Text))
            {
                // Set the error provider
                errProvider.SetError(Wildcard, Resources.ErrorNoWildcardProvided);

                // There is no wildcard value
                result = false;
            }

            if (UseRegularExpression.Checked)
            {
                if (string.IsNullOrEmpty(Expression.Text))
                {
                    // Set the error provider
                    errProvider.SetError(Expression, Resources.ErrorNoRegularExpressionProvided);

                    // There is no regexp value
                    result = false;
                }
                else
                {
                    // Check that the regular expression is valid
                    try
                    {
                        // Code analysis complains that this Regex instance is never used however this is the only way to check 
                        // whether the expression is valid
                        new Regex(Expression.Text, RegexOptions.Singleline);
                    }
                    catch (ArgumentException ex)
                    {
                        // Set the error provider
                        var format = string.Format(CultureInfo.CurrentCulture,
                            Resources.ErrorRegularExpressionInvalid,
                            ex.Message);

                        errProvider.SetError(Expression, format);
                    }
                }
            }

            // Return the result
            return result;
        }

        /// <summary>
        ///     Handles the Closing event of the SelectPathPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void SelectPathPage_Closing(object sender, EventArgs e)
        {
            // Check if the path should have \ appended to it
            if (string.IsNullOrEmpty(SearchPath.Text) == false
                && SearchPath.Text.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.CurrentCulture)
                == false)
            {
                // Update the path
                SearchPath.Text += Path.DirectorySeparatorChar;
            }

            // Store a new file list in state
            State[Tekapo.State.FileListKey] = new BindingList<string>();

            // Store the settings in state
            if (UseRegularExpression.Checked)
            {
                _settings.SearchFilterType = SearchFilterType.RegularExpression;
            }
            else if (UseWildcard.Checked)
            {
                _settings.SearchFilterType = SearchFilterType.Wildcard;
            }
            else
            {
                _settings.SearchFilterType = SearchFilterType.None;
            }
        }

        /// <summary>
        ///     Handles the Opening event of the SelectPathPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void SelectPathPage_Opening(object sender, EventArgs e)
        {
            // Populate the wizard
            SearchPath.DataSource = _settings.SearchDirectoryList;

            var filterType = _settings.SearchFilterType;

            UseNoFilter.Checked = filterType == SearchFilterType.None;
            UseRegularExpression.Checked = filterType == SearchFilterType.RegularExpression;
            UseWildcard.Checked = filterType == SearchFilterType.Wildcard;
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the UseRegularExpression control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void UseRegularExpression_CheckedChanged(object sender, EventArgs e)
        {
            Expression.Enabled = UseRegularExpression.Checked;

            // Check if the checkbox is checked
            if (UseRegularExpression.Checked)
            {
                Expression.Focus();
            }
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the UseWildcard control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void UseWildcard_CheckedChanged(object sender, EventArgs e)
        {
            Wildcard.Enabled = UseWildcard.Checked;

            // Check if the checkbox is checked
            if (UseWildcard.Checked)
            {
                Wildcard.Focus();
            }
        }
    }
}