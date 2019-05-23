namespace Tekapo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using EnsureThat;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;
    using Newtonsoft.Json;
    using Tekapo.Controls;
    using Tekapo.Properties;

    /// <summary>
    ///     The <see cref="MainForm" /> class is the main application window for Tekapo. It hosts the pages in the wizard
    ///     process that allows
    ///     the user to either rename or time-shift image files.
    /// </summary>
    public partial class MainForm : WizardForm
    {
        private readonly IConfiguration _configuration;
        private readonly IExecutionContext _executionContext;
        private readonly ISettings _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm(
            IExecutionContext executionContext,
            IList<WizardPage> pages,
            ISettings settings,
            IConfiguration configuration)
        {
            Ensure.Any.IsNotNull(executionContext, nameof(executionContext));
            Ensure.Any.IsNotNull(pages, nameof(pages));
            Ensure.Any.IsNotNull(settings, nameof(settings));
            Ensure.Any.IsNotNull(configuration, nameof(configuration));

            _executionContext = executionContext;
            _settings = settings;
            _configuration = configuration;

            InitializeComponent();

            // Populate the state
            PopulateState();

            // Populate pages
            PopulateWizardPages(pages);
        }

        /// <summary>
        ///     Raises the <see cref="WizardForm.Navigating" /> and <see cref="WizardForm.Navigated" /> events.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected override void OnNavigate(WizardFormNavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.NavigationType == WizardFormNavigationType.Help)
            {
                // Shell the help page
                Process.Start(Resources.HelpAddress);
            }
            else
            {
                base.OnNavigate(e);
            }
        }

        /// <summary>
        ///     Adds the item to MRU.
        /// </summary>
        /// <param name="list">
        ///     The list to add the item to.
        /// </param>
        /// <param name="newItem">
        ///     The new item.
        /// </param>
        /// <param name="maxListSize">
        ///     Size of the max list.
        /// </param>
        private static void AddItemToMru(IList<string> list, string newItem, int maxListSize)
        {
            if (list.Contains(newItem))
            {
                // Remove the item
                list.Remove(newItem);
            }

            // Insert the item at the start of the list
            list.Insert(0, newItem);

            // Loop while there are too many items
            while (list.Count > maxListSize)
            {
                // Remove the last item
                list.RemoveAt(list.Count - 1);
            }
        }

        /// <summary>
        ///     Handles the FormClosing event of the MainForm control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.FormClosingEventArgs" /> instance containing the event data.
        /// </param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Store state
            StoreStateValues();

            // Attempt to delete the log path if it exists
            var path = (string) State[Tekapo.State.ResultsLogPathKey];

            if (string.IsNullOrEmpty(path) == false
                && File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        ///     Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Assign the window title using the application name and version
            Text = Application.ProductName;
        }

        /// <summary>
        ///     Populates the state.
        /// </summary>
        private void PopulateState()
        {
            State[Tekapo.State.TaskKey] = Task.RenameTask;

            // Determine whether there is a directory path in the commandline arguments
            var searchPath = _executionContext.SearchPath;

            // Check if there is a search path
            if (string.IsNullOrEmpty(searchPath))
            {
                // There is no single directory specified on the command line
                searchPath = _settings.SearchPath;

                // Check if there is a search path
                if (string.IsNullOrEmpty(searchPath))
                {
                    // Set the search path to the personal directory
                    searchPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                }
            }

            _settings.SearchPath = searchPath;

            var lastNameFormat = _settings.NameFormat;

            // Check if there is a name format
            if (string.IsNullOrEmpty(lastNameFormat))
            {
                // Set a default name format
                lastNameFormat = Resources.DefaultRenameFormat;

                _settings.NameFormat = lastNameFormat;
            }
        }

        /// <summary>
        ///     Populates the wizard pages.
        /// </summary>
        /// <param name="pages"></param>
        [SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Pages are disposed when the form is disposed.")]
        private void PopulateWizardPages(IList<WizardPage> pages)
        {
            // Create the Choose Task page
            var chooseTaskPage = pages.OfType<ChooseTaskPage>().Single();

            Pages.Add(NavigationKey.ChoosePage, chooseTaskPage);

            // Create the Select Path page
            var selectPathPage = pages.OfType<SelectPathPage>().Single();
            var selectPathSkipButtonSettings = new WizardButtonSettings(Resources.SkipButtonText);
            var selectPathPageSettings = new WizardPageSettings(null, null, null, null, selectPathSkipButtonSettings);
            var selectPathNavigationSettings = new WizardPageNavigationSettings(
                null,
                null,
                null,
                null,
                NavigationKey.SelectFilesPage);

            Pages.Add(NavigationKey.SelectPathPage,
                selectPathPage,
                selectPathPageSettings,
                selectPathNavigationSettings);

            // Create the File Search page
            var fileSearchPage = pages.OfType<FileSearchPage>().Single();

            Pages.Add(NavigationKey.FileSearchPage, fileSearchPage);

            // Create the Select Files page
            var selectFilesPage = pages.OfType<SelectFilesPage>().Single();
            var selectFilesNavigationSettings = new WizardPageNavigationSettings(null, NavigationKey.SelectPathPage);

            Pages.Add(NavigationKey.SelectFilesPage, selectFilesPage, null, selectFilesNavigationSettings);

            // Declare the finish page settings
            var nameFormatPage = pages.OfType<NameFormatPage>().Single();
            var finishButton = new WizardButtonSettings(Resources.Finish);
            var finishPageSettings = new WizardPageSettings(finishButton);
            var finishNavigationSettings = new WizardPageNavigationSettings(NavigationKey.ProcessFilesPage);

            // Create the Naming Format page
            Pages.Add(NavigationKey.NameFormatPage, nameFormatPage, finishPageSettings, finishNavigationSettings);

            // Create the Time Shift page
            var timeShiftPage = pages.OfType<TimeShiftPage>().Single();

            Pages.Add(NavigationKey.TimeShiftPage, timeShiftPage, finishPageSettings, finishNavigationSettings);

            // Create the Progress page
            var processFilesPage = pages.OfType<ProcessFilesPage>().Single();

            Pages.Add(NavigationKey.ProcessFilesPage, processFilesPage);

            var completedPage = pages.OfType<CompletedPage>().Single();

            completedPage.PageSettings.BackButtonSettings.Visible = false;
            completedPage.PageSettings.NextButtonSettings.Text = Resources.Close;
            completedPage.PageSettings.CancelButtonSettings.Visible = false;
            completedPage.PageSettings.CustomButtonSettings.Text = Resources.Restart;
            completedPage.PageSettings.CustomButtonSettings.Visible = true;
            completedPage.NavigationSettings.CustomPageKey = NavigationKey.ChoosePage;

            Pages.Add(NavigationKey.CompletedPage, completedPage);
        }

        /// <summary>
        ///     Stores the state values.
        /// </summary>
        private void StoreStateValues()
        {
            // Store the search directory MRU
            var searchDirectoryMru = _settings.SearchDirectoryList;
            AddItemToMru(searchDirectoryMru,
                _settings.SearchPath,
                _configuration.MaxSearchDirectoryItems);
            Properties.Settings.Default.SearchDirectoryMRU = JsonConvert.SerializeObject(searchDirectoryMru);

            // Store the name format MRU
            var nameFormatMru = _settings.NameFormatList;
            AddItemToMru(nameFormatMru,
                _settings.NameFormat,
                _configuration.MaxNameFormatItems);
            Properties.Settings.Default.NameFormatMRU = JsonConvert.SerializeObject(nameFormatMru);

            // Save the properties
            Properties.Settings.Default.Save();
        }
    }
}