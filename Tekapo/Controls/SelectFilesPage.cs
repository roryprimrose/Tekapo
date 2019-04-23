namespace Tekapo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Properties;

    /// <summary>
    ///     The <see cref="SelectFilesPage" /> class is a Wizard page that allows the user to add and remove images that have
    ///     been found through the <see cref="FileSearchPage" />.
    /// </summary>
    public partial class SelectFilesPage : WizardBannerPage
    {
        /// <summary>
        ///     Stores the last directory path viewed.
        /// </summary>
        private string _lastDirectoryPath;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SelectFilesPage" /> class.
        /// </summary>
        public SelectFilesPage()
        {
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
            if (e.NavigationType == WizardFormNavigationType.Next)
            {
                if (IsPageValid() == false)
                {
                    // The page isn't valid
                    return false;
                }

                // Define what the next navigation will go to
                if ((string) State[Constants.TaskStateKey] == Constants.RenameTask)
                {
                    e.NavigationKey = Constants.NameFormatNavigationKey;
                }
                else
                {
                    e.NavigationKey = Constants.TimeShiftNavigationKey;
                }
            }

            return base.CanNavigate(e);
        }

        /// <summary>
        ///     Builds the filter.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> value.
        /// </returns>
        private static string BuildFilter()
        {
            var supportedFileTypes = Helper.GetSupportedFileTypes();
            var filterValue = string.Empty;

            // Loop through each supported file type
            for (var index = 0; index < supportedFileTypes.Count; index++)
            {
                var fileType = supportedFileTypes[index];

                filterValue += "|" + fileType.ToUpper(CultureInfo.CurrentCulture) + " files (*." + fileType + ")|*."
                               + fileType;
            }

            // Strip the leading | character
            filterValue = filterValue.Substring(1);

            // Return the filter value
            return filterValue;
        }

        /// <summary>
        ///     Handles the Click event of the AddFiles control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void AddFiles_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select files to add.";
                dialog.AddExtension = true;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = Helper.GetSupportedFileTypes()[0];
                dialog.Filter = BuildFilter();
                dialog.FilterIndex = 0;
                dialog.Multiselect = true;
                dialog.InitialDirectory = _lastDirectoryPath;

                // Check if the user clicked Ok
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    for (var index = 0; index < dialog.FileNames.Length; index++)
                    {
                        var newPath = dialog.FileNames[index];

                        if (FileList.Contains(newPath) == false)
                        {
                            // Add the files
                            FileList.Add(newPath);
                        }
                    }

                    // Store the last path
                    _lastDirectoryPath = Path.GetDirectoryName(dialog.FileName);
                }
            }
        }

        /// <summary>
        ///     Handles the DragDrop event of the Files control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.DragEventArgs" /> instance containing the event data.
        /// </param>
        private void Files_DragDrop(object sender, DragEventArgs e)
        {
            // Check if the dragged data contains file references
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of files
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

                // Loop through each item being dragged
                for (var index = 0; index < files.Length; index++)
                {
                    var item = files[index];

                    // Check that the item is a file which is supported, but not yet in the list
                    if (File.Exists(item)
                        && Helper.IsFileSupported(item)
                        && FileList.Contains(item) == false)
                    {
                        // Add the file to the list
                        FileList.Add(item);
                    }
                }
            }
        }

        /// <summary>
        ///     Handles the DragEnter event of the Files control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.DragEventArgs" /> instance containing the event data.
        /// </param>
        private void Files_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the dragged data contains file references
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of files
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

                // Loop through each item being dragged
                for (var index = 0; index < files.Length; index++)
                {
                    var item = files[index];

                    // Check that the item is a file which is supported, but not yet in the list
                    if (File.Exists(item)
                        && Helper.IsFileSupported(item)
                        && FileList.Contains(item) == false)
                    {
                        // Determine whether this item is a valid extension
                        e.Effect = DragDropEffects.Link;

                        return;
                    }
                }
            }

            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        ///     Handles the KeyUp event of the Files control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.KeyEventArgs" /> instance containing the event data.
        /// </param>
        private void Files_KeyUp(object sender, KeyEventArgs e)
        {
            // Check if the key is Delete
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedFiles();
            }
            else if (e.Control
                     && e.KeyCode == Keys.A)
            {
                // Select all the files

                // Loop through each index
                for (var index = 0; index < Files.Items.Count; index++)
                {
                    // Select the file by its index
                    Files.SetSelected(index, true);
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
            ErrorDisplay.Clear();

            if (Files.Items.Count == 0)
            {
                // Set the error provider
                ErrorDisplay.SetError(Files, Resources.ErrorNoFilesSelected);

                // There is no wildcard value
                result = false;
            }

            // Return the result
            return result;
        }

        /// <summary>
        ///     Handles the Click event of the RemoveAll control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void RemoveAll_Click(object sender, EventArgs e)
        {
            // Clear all the items
            FileList.Clear();
        }

        /// <summary>
        ///     Handles the Click event of the RemoveSelected control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void RemoveSelected_Click(object sender, EventArgs e)
        {
            RemoveSelectedFiles();
        }

        /// <summary>
        ///     Removes the selected files.
        /// </summary>
        private void RemoveSelectedFiles()
        {
            // Create an array to hold the selected items
            var items = new string[Files.SelectedItems.Count];

            // Copy the selected items across to the array
            Files.SelectedItems.CopyTo(items, 0);

            // Remove the binding
            Files.DataSource = null;

            // Loop through each item in the array
            for (var index = 0; index < items.Length; index++)
            {
                var item = items[index];

                // Remove the item
                FileList.Remove(item);
            }

            Files.DataSource = FileList;
        }

        /// <summary>
        ///     Handles the Opening event of the SelectFiles control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void SelectFiles_Opening(object sender, EventArgs e)
        {
            Files.DataSource = State[Constants.FileListStateKey];
            _lastDirectoryPath = (string) State[Constants.SearchPathStateKey];
        }

        /// <summary>
        ///     Gets the file list.
        /// </summary>
        /// <value>
        ///     The file list.
        /// </value>
        public BindingList<string> FileList => (BindingList<string>) State[Constants.FileListStateKey];
    }
}