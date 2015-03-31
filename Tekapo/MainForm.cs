using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Neovolve.Windows.Forms;
using Neovolve.Windows.Forms.Controls;
using Tekapo.Controls;
using Tekapo.Properties;

namespace Tekapo
{
    /// <summary>
    /// The <see cref="MainForm"/> class is the main application window for Tekapo. It hosts the pages in the wizard process that allows
    /// the user to either rename or timeshift image files.
    /// </summary>
    public partial class MainForm : WizardForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Populate the state
            PopulateState();

            // Populate pages
            PopulateWizardPages();
        }

        /// <summary>
        /// Raises the <see cref="WizardForm.Navigating"/> and <see cref="WizardForm.Navigated"/> events.
        /// </summary>
        /// <param name="e">
        /// The <see cref="WizardFormNavigationEventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnNavigate(WizardFormNavigationEventArgs e)
        {
            if (e.NavigationType
                == WizardFormNavigationType.Help)
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
        /// Adds the item to MRU.
        /// </summary>
        /// <param name="list">
        /// The list to add the item to.
        /// </param>
        /// <param name="newItem">
        /// The new item.
        /// </param>
        /// <param name="maxListSize">
        /// Size of the max list.
        /// </param>
        private static void AddItemToMru(IList<String> list, String newItem, Int32 maxListSize)
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
        /// Gets the directory from arguments.
        /// </summary>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String GetDirectoryFromArguments(String[] arguments)
        {
            // Exit if there are no values to process
            if ((arguments == null)
                || (arguments.Length == 0))
            {
                return String.Empty;
            }

            // Loop through each string
            for (Int32 index = 0; index < arguments.Length; index++)
            {
                String item = arguments[index];

                // Check if the item is a directory path
                if (Directory.Exists(item))
                {
                    return item;
                }
            }

            // No directory was found
            return String.Empty;
        }

        /// <summary>
        /// Handles the FormClosing event of the MainForm control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.
        /// </param>
        private void MainForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            // Store state
            StoreStateValues();

            // Attempt to delete the log path if it exists
            String path = (String)State[Constants.ResultsLogPathStateKey];

            if ((String.IsNullOrEmpty(path) == false)
                && File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void MainForm_Load(Object sender, EventArgs e)
        {
            // Assign the window title using the application name and version
            Text = Application.ProductName;
        }

        /// <summary>
        /// Populates the state.
        /// </summary>
        private void PopulateState()
        {
            State[Constants.CommandLineArgumentsProcessedStateKey] = false;
            State[Constants.TaskStateKey] = Constants.RenameTask;

            // Determine whether there is a directory path in the commandline arguments
            String lastSearchPath = GetDirectoryFromArguments(Environment.GetCommandLineArgs());

            // Check if there is a search path
            if (String.IsNullOrEmpty(lastSearchPath))
            {
                lastSearchPath = Settings.Default.LastSearchDirectory;

                // Check if there is a search path
                if (String.IsNullOrEmpty(lastSearchPath))
                {
                    // Set the search path to the personal directory
                    lastSearchPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                }
            }

            State[Constants.SearchPathStateKey] = lastSearchPath;
            State[Constants.SearchSubDirectoriesStateKey] = Settings.Default.SearchSubDirectories;

            try
            {
                State[Constants.SearchFilterTypeStateKey] = Enum.Parse(
                    typeof(SearchFilterType), Settings.Default.SearchFilterType);
            }
            catch (ArgumentException)
            {
                // An invalid enum value was defined
                State[Constants.SearchFilterTypeStateKey] = Tekapo.Controls.SearchFilterType.None;
            }

            State[Constants.SearchFilterRegularExpressionStateKey] = Settings.Default.RegularExpressionSearchFilter;
            State[Constants.SearchFilterWildcardStateKey] = Settings.Default.WildcardSearchFilter;

            String lastNameFormat = Settings.Default.NameFormat;

            // Check if there is a name format
            if (String.IsNullOrEmpty(lastNameFormat))
            {
                // Set a default name format
                lastNameFormat = Resources.DefaultRenameFormat;
            }

            State[Constants.NameFormatStateKey] = lastNameFormat;
            State[Constants.IncrementOnCollisionStateKey] = Settings.Default.IncrementOnCollision;

            // Determine the MRU list for search directories
            State[Constants.SearchDirectoryMRUStateKey] = Helper.DeserializeList(Settings.Default.SearchDirectoryMRU);

            // Determine the MRU list for name formats
            State[Constants.NameFormatMRUStateKey] = Helper.DeserializeList(Settings.Default.NameFormatMRU);
        }

        /// <summary>
        /// Populates the wizard pages.
        /// </summary>
        private void PopulateWizardPages()
        {
            // Create the Choose Task page
            Pages.Add(Constants.ChooseNavigationKey, new ChooseTaskPage());

            // Create the Select Path page
            WizardButtonSettings selectPathSkipButtonSettings = new WizardButtonSettings(Resources.SkipButtonText);
            WizardPageSettings selectPathPageSettings = new WizardPageSettings(
                null, null, null, null, selectPathSkipButtonSettings);
            WizardPageNavigationSettings selectPathNavigationSettings = new WizardPageNavigationSettings(
                null, null, null, null, Constants.SelectFilesNavigationKey);
            Pages.Add(
                Constants.SelectPathNavigationKey,
                new SelectPathPage(),
                selectPathPageSettings,
                selectPathNavigationSettings);

            // Create the File Search page
            Pages.Add(Constants.FileSearchNavigationKey, new FileSearchPage());

            // Create the Select Files page
            WizardPageNavigationSettings selectFilesNavigationSettings = new WizardPageNavigationSettings(
                null, Constants.SelectPathNavigationKey);
            Pages.Add(Constants.SelectFilesNavigationKey, new SelectFilesPage(), null, selectFilesNavigationSettings);

            // Declare the finish page settings
            WizardButtonSettings finishButton = new WizardButtonSettings(Resources.Finish);
            WizardPageSettings finishPageSettings = new WizardPageSettings(finishButton);
            WizardPageNavigationSettings finishNavigationSettings =
                new WizardPageNavigationSettings(Constants.ProcessFilesNavigationKey);

            // Create the Naming Format page
            Pages.Add(
                Constants.NameFormatNavigationKey, new NameFormatPage(), finishPageSettings, finishNavigationSettings);

            // Create the Time Shift page
            Pages.Add(
                Constants.TimeShiftNavigationKey, new TimeShiftPage(), finishPageSettings, finishNavigationSettings);

            // Create the Progress page
            Pages.Add(Constants.ProcessFilesNavigationKey, new ProcessFilesPage());

            CompletedPage completedPage = new CompletedPage();
            completedPage.PageSettings.BackButtonSettings.Visible = false;
            completedPage.PageSettings.NextButtonSettings.Text = Resources.Close;
            completedPage.PageSettings.CancelButtonSettings.Visible = false;
            completedPage.PageSettings.CustomButtonSettings.Text = Resources.Restart;
            completedPage.PageSettings.CustomButtonSettings.Visible = true;
            completedPage.NavigationSettings.CustomPageKey = Constants.ChooseNavigationKey;
            
            Pages.Add(Constants.CompletedNavigationKey, completedPage);
        }

        /// <summary>
        /// Stores the state values.
        /// </summary>
        private void StoreStateValues()
        {
            // SelectPathPage state values
            Settings.Default.LastSearchDirectory = (String)State[Constants.SearchPathStateKey];
            Settings.Default.SearchSubDirectories = (Boolean)State[Constants.SearchSubDirectoriesStateKey];
            Settings.Default.SearchFilterType = State[Constants.SearchFilterTypeStateKey].ToString();
            Settings.Default.RegularExpressionSearchFilter =
                (String)State[Constants.SearchFilterRegularExpressionStateKey];
            Settings.Default.WildcardSearchFilter = (String)State[Constants.SearchFilterWildcardStateKey];

            // NameFormatPage state values
            Settings.Default.NameFormat = (String)State[Constants.NameFormatStateKey];
            Settings.Default.IncrementOnCollision = (Boolean)State[Constants.IncrementOnCollisionStateKey];

            // Store the search directory MRU
            BindingList<String> searchDirectoryMRU = (BindingList<String>)State[Constants.SearchDirectoryMRUStateKey];
            AddItemToMru(
                searchDirectoryMRU, Settings.Default.LastSearchDirectory, Settings.Default.MaxSearchDirectoryMRUItems);
            Settings.Default.SearchDirectoryMRU = Helper.SerializeList(searchDirectoryMRU);

            // Store the name format MRU
            BindingList<String> nameFormatMRU = (BindingList<String>)State[Constants.NameFormatMRUStateKey];
            AddItemToMru(nameFormatMRU, Settings.Default.NameFormat, Settings.Default.MaxNameFormatMRUItems);
            Settings.Default.NameFormatMRU = Helper.SerializeList(nameFormatMRU);

            // Save the properties
            Settings.Default.Save();
        }
    }
}