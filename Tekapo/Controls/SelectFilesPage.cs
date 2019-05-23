namespace Tekapo.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using EnsureThat;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class SelectFilesPage : WizardBannerPage
    {
        private readonly IMediaManager _mediaManager;
        private string _lastDirectoryPath;
        private readonly ISettings _settings;
        private readonly IFileSearcher _fileSearcher;

        public SelectFilesPage(IMediaManager mediaManager, IFileSearcher fileSearcher, ISettings settings)
        {
            Ensure.Any.IsNotNull(mediaManager, nameof(mediaManager));
            Ensure.Any.IsNotNull(fileSearcher, nameof(fileSearcher));
            Ensure.Any.IsNotNull(settings, nameof(settings));

            _mediaManager = mediaManager;
            _fileSearcher = fileSearcher;
            _settings = settings;

            InitializeComponent();
        }

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
                if ((TaskType)State[Tekapo.State.TaskKey] == TaskType.RenameTask)
                {
                    e.NavigationKey = NavigationKey.NameFormatPage;
                }
                else
                {
                    e.NavigationKey = NavigationKey.TimeShiftPage;
                }
            }

            return base.CanNavigate(e);
        }

        private static string BuildFilter(IList<string> supportedFileTypes)
        {
            // Get the extensions without the leading .
            var parts = supportedFileTypes.Select(x =>
                x.ToUpper(CultureInfo.CurrentCulture) + " files (*." + x + ")|*." + x);

            var filterValue = string.Join("|", parts);

            // Return the filter value
            return filterValue;
        }

        private void AddFiles_Click(object sender, EventArgs e)
        {
            var taskType = (TaskType)State[Tekapo.State.TaskKey];
            var operationType = taskType.AsMediaOperationType();
            var supportedFileTypes = _mediaManager.GetSupportedFileTypes(operationType).Select(x => x.Substring(1)).ToList();
            var dialogFilter = BuildFilter(supportedFileTypes);
            var defaultExtension = "jpg";
            var jpgIndex = supportedFileTypes.IndexOf(defaultExtension);

            if (jpgIndex == -1)
            {
                var firstExtension = supportedFileTypes.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(firstExtension) == false)
                {
                    defaultExtension = firstExtension;
                }
            }

            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select files to add";
                dialog.AddExtension = true;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = defaultExtension;
                dialog.Filter = dialogFilter;
                dialog.FilterIndex = jpgIndex + 1;
                dialog.Multiselect = true;
                dialog.InitialDirectory = _lastDirectoryPath;

                // Check if the user clicked Ok
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var paths = dialog.FileNames.ToList();

                AddPathsToList(paths);

                // Store the last path
                _lastDirectoryPath = Path.GetDirectoryName(dialog.FileName);
            }
        }

        private void Files_DragDrop(object sender, DragEventArgs e)
        {
            // Check if the dragged data contains file references
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of files
                var files = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();

                AddPathsToList(files);
            }
        }

        private void Files_DragEnter(object sender, DragEventArgs e)
        {
            var taskType = (TaskType)State[Tekapo.State.TaskKey];
            var operationType = taskType.AsMediaOperationType();

            // Check if the dragged data contains file references
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of files
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Loop through each item being dragged
                for (var index = 0; index < files.Length; index++)
                {
                    var item = files[index];

                    if (File.Exists(item) == false)
                    {
                        continue;
                    }

                    if (FileList.Contains(item))
                    {
                        continue;
                    }

                    // Check that the item is a file which is supported, but not yet in the list
                    if (_mediaManager.IsSupported(item, operationType))
                    {
                        // Determine whether this item is a valid extension
                        e.Effect = DragDropEffects.Link;

                        return;
                    }
                }
            }

            e.Effect = DragDropEffects.None;
        }

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

        private void RemoveAll_Click(object sender, EventArgs e)
        {
            // Clear all the items
            FileList.Clear();
        }

        private void RemoveSelected_Click(object sender, EventArgs e)
        {
            RemoveSelectedFiles();
        }

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

        private void SelectFiles_Opening(object sender, EventArgs e)
        {
            Files.DataSource = State[Tekapo.State.FileListKey];
            _lastDirectoryPath = _settings.SearchPath;
        }

        public BindingList<string> FileList => (BindingList<string>)State[Tekapo.State.FileListKey];

        private void AddFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = _lastDirectoryPath;
                dialog.Description = Resources.SelectPathDescription;
                dialog.ShowNewFolderButton = false;

                // Check if the user clicked OK
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                _lastDirectoryPath = dialog.SelectedPath;

                var paths = new List<string> { dialog.SelectedPath };

                AddPathsToList(paths);
            }
        }

        private void AddPathsToList(List<string> paths)
        {
            var taskType = (TaskType) State[Tekapo.State.TaskKey];
            var operationType = taskType.AsMediaOperationType();

            var files = _fileSearcher.FindSupportedFiles(paths, operationType);

            foreach (var file in files)
            {
                if (FileList.Contains(file))
                {
                    continue;
                }

                // Add the file to the list
                FileList.Add(file);
            }
        }
    }
}