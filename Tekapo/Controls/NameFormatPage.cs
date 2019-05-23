namespace Tekapo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using EnsureThat;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class NameFormatPage : WizardBannerPage
    {
        private readonly IConfiguration _configuration;
        private readonly ISettings _settings;
        private readonly IMediaManager _mediaManager;
        private readonly IPathManager _pathManager;
        private Thread _exampleThread;

        public NameFormatPage(IMediaManager mediaManager, IPathManager pathManager, ISettings settings,
            IConfiguration configuration)
        {
            Ensure.Any.IsNotNull(mediaManager, nameof(mediaManager));
            Ensure.Any.IsNotNull(pathManager, nameof(pathManager));
            Ensure.Any.IsNotNull(settings, nameof(settings));
            Ensure.Any.IsNotNull(configuration, nameof(configuration));

            _mediaManager = mediaManager;
            _pathManager = pathManager;
            _settings = settings;
            _configuration = configuration;

            InitializeComponent();
        }

        private delegate string GetFormatValueDelegate();

        private delegate void SetExampleValueDelegate(string example);

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

        private void BuildExample()
        {
            if (_exampleThread != null
                && (_exampleThread.ThreadState & ThreadState.Running) == ThreadState.Running)
            {
                // Abort the currently running thread
                _exampleThread.Abort();

                _exampleThread = null;
            }

            // Create the thread
            _exampleThread = new Thread(CalculateExampleValue);
            _exampleThread.IsBackground = true;
            _exampleThread.Start();
        }

        private void CalculateExampleValue()
        {
            var renameFormat = GetFormatValue();
            string exampleMessage;

            // Check if a format is specified
            if (string.IsNullOrEmpty(renameFormat))
            {
                exampleMessage = Resources.NameFormatExampleNoFormatAvailable.Replace("\\n", "\n");
            }
            else if (_pathManager.IsFormatValid(renameFormat) == false)
            {
                // The rename format specified is valid
                exampleMessage = Resources.NameFormatExampleInvalidFormat.Replace("\\n", "\n");
            }
            else
            {
                // A name format is specified and is valid
                // Generate the example
                var exampleData = FindPictureTakenDate();

                if (exampleData.CreatedAt == null)
                {
                    // No example data was resolved
                    exampleMessage = Resources.NameFormatExampleNoValidFile;
                }
                else
                {
                    var incrementOnCollision = IncrementOnCollision.Checked;
                    var maxCollisionIncrement = _configuration.MaxCollisionIncrement;
                    var resultPath = _pathManager.GetRenamedPath(renameFormat,
                        exampleData.CreatedAt.Value,
                        exampleData.Path,
                        incrementOnCollision,
                        maxCollisionIncrement);

                    var sourceDirectory = Path.GetDirectoryName(exampleData.Path);
                    var targetDirectory = Path.GetDirectoryName(resultPath);
                    var sourceExample = exampleData.Path;
                    var targetExample = resultPath;

                    if (string.Equals(sourceDirectory, targetDirectory, StringComparison.OrdinalIgnoreCase))
                    {
                        sourceExample = Path.GetFileName(sourceExample);
                        targetExample = Path.GetFileName(targetExample);
                    }

                    exampleMessage = string.Format(CultureInfo.CurrentCulture,
                        Resources.NameFormatExample,
                        sourceExample,
                        targetExample);
                }
            }

            SetExampleValue(exampleMessage);
        }

        private ExampleData FindPictureTakenDate()
        {
            var paths = (BindingList<string>) State[Tekapo.State.FileListKey];

            foreach (var path in paths)
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    var mediaCreatedDate = _mediaManager.ReadMediaCreatedDate(stream);

                    if (mediaCreatedDate == null)
                    {
                        continue;
                    }

                    return new ExampleData {CreatedAt = mediaCreatedDate, Path = path};
                }
            }

            return default;
        }

        private string GetFormatValue()
        {
            if (NameFormat.InvokeRequired)
            {
                return (string) NameFormat.Invoke(new GetFormatValueDelegate(GetFormatValue));
            }

            return NameFormat.Text;
        }

        private void IncrementOnCollision_CheckedChanged(object sender, EventArgs e)
        {
            BuildExample();
        }

        private void InsertFormat_Click(object sender, EventArgs e)
        {
            var menuLocation = InsertFormat.Location;

            menuLocation.X += InsertFormat.Width;

            InsertFormat.ContextMenu.Show(InsertFormat.Parent, menuLocation);
        }

        private bool IsPageValid()
        {
            var result = true;

            // Clear the error provider
            ErrorDisplay.Clear();

            if (string.IsNullOrEmpty(NameFormat.Text))
            {
                // Set the error provider
                ErrorDisplay.SetError(NameFormat, Resources.ErrorNoNameFormatProvided);

                // There is no name format
                result = false;
            }
            else if (_pathManager.IsFormatValid(NameFormat.Text) == false)
            {
                // Set the error provider
                ErrorDisplay.SetError(NameFormat, Resources.ErrorNameFormatInvalid);

                // The name format specified is invalid
                result = false;
            }

            // Return the result
            return result;
        }

        private void NameFormat_TextChanged(object sender, EventArgs e)
        {
            BuildExample();
        }

        [SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "The instance lives beyond the scope of this method.")]
        private void NameFormatPage_Load(object sender, EventArgs e)
        {
            var formatMenu = new ContextMenu();

            // Populate the rename formats into the context menu
            foreach (var pair in PathManager.RenameFormats)
            {
                var renameMenuItem = new MenuItem();
                var formatValue = string.Concat("<", pair.Key, ">");
                var formatDescription = pair.Value;

                renameMenuItem.Text = string.Format(
                    CultureInfo.CurrentCulture,
                    "{0}  {1}",
                    formatValue,
                    formatDescription);
                renameMenuItem.Tag = formatValue;

                renameMenuItem.Click += delegate { NameFormat.Text += formatValue; };

                formatMenu.MenuItems.Add(renameMenuItem);
            }

            InsertFormat.ContextMenu = formatMenu;
        }

        private void NameFormatPage_Opening(object sender, EventArgs e)
        {
            // Load the state values
            NameFormat.DataSource = _settings.NameFormatList;

            // Build the example
            BuildExample();
        }

        private void SetExampleValue(string example)
        {
            if (Example.InvokeRequired)
            {
                object[] args = {example};

                Example.Invoke(new SetExampleValueDelegate(SetExampleValue), args);

                return;
            }

            Example.Text = example;
        }

        private struct ExampleData
        {
            public string Path { get; set; }

            public DateTime? CreatedAt { get; set; }
        }
    }
}