using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Neovolve.Windows.Forms;
using Neovolve.Windows.Forms.Controls;

namespace Tekapo.Controls
{
    /// <summary>
    /// The <see cref="Tekapo.Controls.SelectPathPage"/> class is a Wizard page that allows the user to select a path to search for
    /// images to process.
    /// </summary>
    public partial class SelectPathPage : WizardBannerPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tekapo.Controls.SelectPathPage"/> class.
        /// </summary>
        public SelectPathPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Determines whether this instance can navigate the specified e.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:Neovolve.Windows.Forms.WizardFormNavigationEventArgs"/> instance containing the event data.
        /// </param>
        /// <returns>
        /// <c>true</c>if this instance can navigate the specified e; otherwise, <c>false</c>.
        /// </returns>
        public override Boolean CanNavigate(WizardFormNavigationEventArgs e)
        {
            // Check if the user is clicking next
            if ((e.NavigationType == WizardFormNavigationType.Next)
                && (IsPageValid() == false))
            {
                // The page isn't valid
                return false;
            }

            return base.CanNavigate(e);
        }

        /// <summary>
        /// Handles the Click event of the Browse control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Browse_Click(Object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = (String)State[Constants.SearchPathStateKey];
                dialog.Description = Properties.Resources.SelectPathDescription;
                dialog.ShowNewFolderButton = false;

                // Check if the user clicked OK
                if (dialog.ShowDialog(this)
                    == DialogResult.OK)
                {
                    // Store the path
                    SearchPath.Text = dialog.SelectedPath;
                }
            }
        }

        /// <summary>
        /// Determines whether the page is valid.
        /// </summary>
        /// <returns>
        /// <c>true</c>if the page is valid; otherwise, <c>false</c>.
        /// </returns>
        private Boolean IsPageValid()
        {
            Boolean result = true;

            // Clear the error provider
            errProvider.Clear();

            if (String.IsNullOrEmpty(SearchPath.Text))
            {
                // Set the error provider
                errProvider.SetError(SearchPath, Properties.Resources.ErrorNoSearchPathProvided);

                // There is no path
                result = false;
            }
            else if (Directory.Exists(SearchPath.Text) == false)
            {
                // Set the error provider
                errProvider.SetError(SearchPath, Properties.Resources.ErrorSearchPathDoesNotExist);

                // The directory specified doesn't exist
                result = false;
            }

            if (UseWildcard.Checked
                && String.IsNullOrEmpty(Wildcard.Text))
            {
                // Set the error provider
                errProvider.SetError(Wildcard, Properties.Resources.ErrorNoWildcardProvided);

                // There is no wildcard value
                result = false;
            }

            if (UseRegularExpression.Checked)
            {
                if (String.IsNullOrEmpty(Expression.Text))
                {
                    // Set the error provider
                    errProvider.SetError(Expression, Properties.Resources.ErrorNoRegularExpressionProvided);

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
                        String format = String.Format(
                            CultureInfo.CurrentCulture, Properties.Resources.ErrorRegularExpressionInvalid, ex.Message);

                        errProvider.SetError(Expression, format);
                    }
                }
            }

            // Return the result
            return result;
        }

        /// <summary>
        /// Handles the Closing event of the SelectPathPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void SelectPathPage_Closing(Object sender, EventArgs e)
        {
            // Check if the path should have \ appended to it
            if ((String.IsNullOrEmpty(SearchPath.Text) == false)
                &&
                (SearchPath.Text.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.CurrentCulture)
                 == false))
            {
                // Update the path
                SearchPath.Text += Path.DirectorySeparatorChar;
            }

            // Store a new file list in state
            State[Constants.FileListStateKey] = new BindingList<String>();

            // Store the settings in state
            State[Constants.SearchPathStateKey] = SearchPath.Text;
            State[Constants.SearchSubDirectoriesStateKey] = Recurse.Checked;

            if (UseRegularExpression.Checked)
            {
                State[Constants.SearchFilterTypeStateKey] = SearchFilterType.RegularExpression;
            }
            else if (UseWildcard.Checked)
            {
                State[Constants.SearchFilterTypeStateKey] = SearchFilterType.Wildcard;
            }
            else
            {
                State[Constants.SearchFilterTypeStateKey] = SearchFilterType.None;
            }

            State[Constants.SearchFilterWildcardStateKey] = Wildcard.Text;
            State[Constants.SearchFilterRegularExpressionStateKey] = Expression.Text;
        }

        /// <summary>
        /// Handles the Opening event of the SelectPathPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void SelectPathPage_Opening(Object sender, EventArgs e)
        {
            // Populate the wizard
            SearchPath.Text = (String)State[Constants.SearchPathStateKey];
            SearchPath.DataSource = State[Constants.SearchDirectoryMRUStateKey];
            Recurse.Checked = (Boolean)State[Constants.SearchSubDirectoriesStateKey];

            SearchFilterType filterType = (SearchFilterType)State[Constants.SearchFilterTypeStateKey];

            UseNoFilter.Checked = filterType == SearchFilterType.None;
            UseRegularExpression.Checked = filterType == SearchFilterType.RegularExpression;
            Expression.Text = (String)State[Constants.SearchFilterRegularExpressionStateKey];
            UseWildcard.Checked = filterType == SearchFilterType.Wildcard;
            Wildcard.Text = (String)State[Constants.SearchFilterWildcardStateKey];
        }

        /// <summary>
        /// Handles the CheckedChanged event of the UseRegularExpression control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void UseRegularExpression_CheckedChanged(Object sender, EventArgs e)
        {
            Expression.Enabled = UseRegularExpression.Checked;

            // Check if the checkbox is checked
            if (UseRegularExpression.Checked)
            {
                Expression.Focus();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the UseWildcard control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void UseWildcard_CheckedChanged(Object sender, EventArgs e)
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