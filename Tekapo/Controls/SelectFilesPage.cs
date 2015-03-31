using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Neovolve.Windows.Forms;
using Neovolve.Windows.Forms.Controls;

namespace Tekapo.Controls
{
    /// <summary>
    /// The <see cref="SelectFilesPage"/> class is a Wizard page that allows the user to add and remove images that have
    /// been found through the <see cref="FileSearchPage"/>.
    /// </summary>
    public partial class SelectFilesPage : WizardBannerPage
    {
        /// <summary>
        /// Stores the last directory path viewed.
        /// </summary>
        private String _lastDirectoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectFilesPage"/> class.
        /// </summary>
        public SelectFilesPage()
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
            if (e.NavigationType
                == WizardFormNavigationType.Next)
            {
                if (IsPageValid() == false)
                {
                    // The page isn't valid
                    return false;
                }

                // Define what the next navigation will go to
                if ((String)State[Constants.TaskStateKey]
                    == Constants.RenameTask)
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
        /// Builds the filter.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> value.
        /// </returns>
        private static String BuildFilter()
        {
            List<String> supportedFileTypes = Helper.GetSupportedFileTypes();
            String filterValue = String.Empty;

            // Loop through each supported file type
            for (Int32 index = 0; index < supportedFileTypes.Count; index++)
            {
                String fileType = supportedFileTypes[index];

                filterValue += "|" + fileType.ToUpper(CultureInfo.CurrentCulture) + " files (*." + fileType + ")|*."
                               + fileType;
            }

            // Strip the leading | character
            filterValue = filterValue.Substring(1);

            // Return the filter value
            return filterValue;
        }

        /// <summary>
        /// Handles the Click event of the AddFiles control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void AddFiles_Click(Object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
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
                if (dialog.ShowDialog(this)
                    == DialogResult.OK)
                {
                    for (Int32 index = 0; index < dialog.FileNames.Length; index++)
                    {
                        String newPath = dialog.FileNames[index];

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
        /// Handles the DragDrop event of the Files control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.
        /// </param>
        private void Files_DragDrop(Object sender, DragEventArgs e)
        {
            // Check if the dragged data contains file references
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of files
                String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);

                // Loop through each item being dragged
                for (Int32 index = 0; index < files.Length; index++)
                {
                    String item = files[index];

                    // Check that the item is a file which is supported, but not yet in the list
                    if (File.Exists(item) && Helper.IsFileSupported(item)
                        && (FileList.Contains(item) == false))
                    {
                        // Add the file to the list
                        FileList.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the DragEnter event of the Files control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.
        /// </param>
        private void Files_DragEnter(Object sender, DragEventArgs e)
        {
            // Check if the dragged data contains file references
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of files
                String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);

                // Loop through each item being dragged
                for (Int32 index = 0; index < files.Length; index++)
                {
                    String item = files[index];

                    // Check that the item is a file which is supported, but not yet in the list
                    if (File.Exists(item) && Helper.IsFileSupported(item)
                        && (FileList.Contains(item) == false))
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
        /// Handles the KeyUp event of the Files control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.
        /// </param>
        private void Files_KeyUp(Object sender, KeyEventArgs e)
        {
            // Check if the key is Delete
            if (e.KeyCode
                == Keys.Delete)
            {
                RemoveSelectedFiles();
            }
            else if (e.Control
                     && (e.KeyCode == Keys.A))
            {
                // Select all the files

                // Loop through each index
                for (Int32 index = 0; index < Files.Items.Count; index++)
                {
                    // Select the file by its index
                    Files.SetSelected(index, true);
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
            ErrorDisplay.Clear();

            if (Files.Items.Count == 0)
            {
                // Set the error provider
                ErrorDisplay.SetError(Files, Properties.Resources.ErrorNoFilesSelected);

                // There is no wildcard value
                result = false;
            }

            // Return the result
            return result;
        }

        /// <summary>
        /// Handles the Click event of the RemoveAll control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void RemoveAll_Click(Object sender, EventArgs e)
        {
            // Clear all the items
            FileList.Clear();
        }

        /// <summary>
        /// Handles the Click event of the RemoveSelected control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void RemoveSelected_Click(Object sender, EventArgs e)
        {
            RemoveSelectedFiles();
        }

        /// <summary>
        /// Removes the selected files.
        /// </summary>
        private void RemoveSelectedFiles()
        {
            // Create an array to hold the selected items
            String[] items = new String[Files.SelectedItems.Count];

            // Copy the selected items across to the array
            Files.SelectedItems.CopyTo(items, 0);

            // Remove the binding
            Files.DataSource = null;

            // Loop through each item in the array
            for (Int32 index = 0; index < items.Length; index++)
            {
                String item = items[index];

                // Remove the item
                FileList.Remove(item);
            }

            Files.DataSource = FileList;
        }

        /// <summary>
        /// Handles the Opening event of the SelectFiles control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void SelectFiles_Opening(Object sender, EventArgs e)
        {
            Files.DataSource = State[Constants.FileListStateKey];
            _lastDirectoryPath = (String)State[Constants.SearchPathStateKey];
        }

        /// <summary>
        /// Gets the file list.
        /// </summary>
        /// <value>
        /// The file list.
        /// </value>
        public BindingList<String> FileList
        {
            get
            {
                return (BindingList<String>)State[Constants.FileListStateKey];
            }
        }
    }
}