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

    public partial class SelectPathPage : WizardBannerPage
    {
        private readonly ISettings _settings;
        private readonly IExecutionContext _executionContext;

        public SelectPathPage(ISettings settings, IExecutionContext executionContext)
        {
            Ensure.Any.IsNotNull(settings, nameof(settings));
            Ensure.Any.IsNotNull(executionContext, nameof(executionContext));

            _settings = settings;
            _executionContext = executionContext;

            InitializeComponent();
        }

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

        private bool IsPageValid()
        {
            var result = true;

            // Clear the error provider
            errProvider.Clear();

            if (_executionContext.SearchPaths.Count == 0
                && string.IsNullOrEmpty(SearchPath.Text))
            {
                // There are no command line parameters and no search path text has been defined
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

        private void SelectPathPage_Opening(object sender, EventArgs e)
        {
            var noCommandLineArgs = _executionContext.SearchPaths.Count == 0;

            PathSearchLabel.Enabled = noCommandLineArgs;
            SearchPath.Enabled = noCommandLineArgs;
            Browse.Enabled = noCommandLineArgs;

            // Populate the wizard
            SearchPath.DataSource = _settings.SearchDirectoryList;

            var filterType = _settings.SearchFilterType;

            UseNoFilter.Checked = filterType == SearchFilterType.None;
            UseRegularExpression.Checked = filterType == SearchFilterType.RegularExpression;
            UseWildcard.Checked = filterType == SearchFilterType.Wildcard;
        }

        private void UseRegularExpression_CheckedChanged(object sender, EventArgs e)
        {
            Expression.Enabled = UseRegularExpression.Checked;

            // Check if the checkbox is checked
            if (UseRegularExpression.Checked)
            {
                Expression.Focus();
            }
        }

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