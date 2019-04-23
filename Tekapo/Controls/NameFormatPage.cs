namespace Tekapo.Controls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using Neovolve.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class NameFormatPage : WizardBannerPage
    {
        private Thread _exampleThread;

        private readonly IMediaManager _mediaManager;

        public NameFormatPage(IMediaManager mediaManager)
        {
            _mediaManager = mediaManager;

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
            else if (ImageRenaming.IsFormatValid(renameFormat) == false)
            {
                // The rename format specified is valid
                exampleMessage = Resources.NameFormatExampleInvalidFormat.Replace("\\n", "\n");
            }
            else
            {
                // A name format is specified and is valid
                // Generate the example
                var sourcePath = ((BindingList<string>) State[Constants.FileListStateKey])[0];
                var mediaCreatedDate = _mediaManager.ReadMediaCreatedDate(sourcePath);
                var incrementOnCollision = IncrementOnCollision.Checked;
                var maxCollisionIncrement = Settings.Default.MaxCollisionIncrement;
                var resultPath = ImageRenaming.GetRenamedPath(renameFormat,
                    mediaCreatedDate,
                    sourcePath,
                    incrementOnCollision,
                    maxCollisionIncrement);
                var exampleFormat = Resources.NameFormatExample.Replace("\\n", "\n");

                exampleMessage = string.Format(CultureInfo.CurrentCulture, exampleFormat, sourcePath, resultPath);
            }

            SetExampleValue(exampleMessage);
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
            else if (ImageRenaming.IsFormatValid(NameFormat.Text) == false)
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

        private void NameFormatPage_Closing(object sender, EventArgs e)
        {
            State[Constants.NameFormatStateKey] = NameFormat.Text;
            State[Constants.IncrementOnCollisionStateKey] = IncrementOnCollision.Checked;
        }

        [SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "The instance lives beyond the scope of this method.")]
        private void NameFormatPage_Load(object sender, EventArgs e)
        {
            var formatMenu = new ContextMenu();

            // Populate the rename formats into the context menu
            foreach (var pair in ImageRenaming.RenameFormats)
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
            NameFormat.Text = (string) State[Constants.NameFormatStateKey];
            NameFormat.DataSource = State[Constants.NameFormatMRUStateKey];
            IncrementOnCollision.Checked = (bool) State[Constants.IncrementOnCollisionStateKey];

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
    }
}