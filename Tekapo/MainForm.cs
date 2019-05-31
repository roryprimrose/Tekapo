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
    using Tekapo.Controls;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class MainForm : WizardForm
    {
        private readonly IConfig _config;
        private readonly ISettings _settings;
        private readonly ISettingsWriter _settingsWriter;

        public MainForm(IList<WizardPage> pages, ISettings settings, ISettingsWriter settingsWriter, IConfig config)
        {
            Ensure.Any.IsNotNull(pages, nameof(pages));
            Ensure.Any.IsNotNull(settings, nameof(settings));
            Ensure.Any.IsNotNull(settingsWriter, nameof(settingsWriter));
            Ensure.Any.IsNotNull(config, nameof(config));

            _settings = settings;
            _settingsWriter = settingsWriter;
            _config = config;

            InitializeComponent();

            // Set the default state
            State[Tekapo.State.TaskKey] = TaskType.RenameTask;

            // Populate pages
            PopulateWizardPages(pages);
        }

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Assign the window title using the application name and version
            Text = Application.ProductName;
        }

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

        private void StoreStateValues()
        {
            // Store the search directory MRU
            var searchDirectoryMru = _settings.SearchDirectoryList;

            AddItemToMru(searchDirectoryMru, _settings.SearchPath, _config.MaxSearchDirectoryItems);

            _settingsWriter.WriteSearchDirectoryList(searchDirectoryMru);

            // Store the name format MRU
            var nameFormatMru = _settings.NameFormatList;

            AddItemToMru(nameFormatMru, _settings.NameFormat, _config.MaxNameFormatItems);

            _settingsWriter.WriteNameFormatList(nameFormatMru);

            // Save the properties
            _settingsWriter.Save();
        }
    }
}