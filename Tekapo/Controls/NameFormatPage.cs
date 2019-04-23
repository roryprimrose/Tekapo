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

    /// <summary>
    ///     The <see cref="NameFormatPage" /> class is a Wizard page that allows the user to specify a format to use when
    ///     renaming images.
    /// </summary>
    public partial class NameFormatPage : WizardBannerPage
    {
        /// <summary>
        ///     Stores the thread used to calculate the rename example.
        /// </summary>
        private Thread _exampleThread;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NameFormatPage" /> class.
        /// </summary>
        public NameFormatPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Get format value delegate.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> value.
        /// </returns>
        private delegate string GetFormatValueDelegate();

        /// <summary>
        ///     Set example value delegate.
        /// </summary>
        /// <param name="example">
        ///     The example.
        /// </param>
        private delegate void SetExampleValueDelegate(string example);

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
            if (e.NavigationType == WizardFormNavigationType.Next
                && IsPageValid() == false)
            {
                // The page isn't valid
                return false;
            }

            return base.CanNavigate(e);
        }

        /// <summary>
        ///     Updates the example.
        /// </summary>
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

        /// <summary>
        ///     Calculates the example value.
        /// </summary>
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
                var pictureTaken = JpegInformation.ReadMediaCreatedDate(sourcePath);
                var incrementOnCollision = IncrementOnCollision.Checked;
                var maxCollisionIncrement = Settings.Default.MaxCollisionIncrement;
                var resultPath = ImageRenaming.GetRenamedPath(renameFormat,
                    pictureTaken,
                    sourcePath,
                    incrementOnCollision,
                    maxCollisionIncrement);
                var exampleFormat = Resources.NameFormatExample.Replace("\\n", "\n");

                exampleMessage = string.Format(CultureInfo.CurrentCulture, exampleFormat, sourcePath, resultPath);
            }

            SetExampleValue(exampleMessage);
        }

        /// <summary>
        ///     Gets the format value.
        /// </summary>
        /// <returns>
        ///     The format value.
        /// </returns>
        private string GetFormatValue()
        {
            if (NameFormat.InvokeRequired)
            {
                return (string) NameFormat.Invoke(new GetFormatValueDelegate(GetFormatValue));
            }

            return NameFormat.Text;
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the IncrementOnCollision control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void IncrementOnCollision_CheckedChanged(object sender, EventArgs e)
        {
            BuildExample();
        }

        /// <summary>
        ///     Handles the Click event of the InsertFormat control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void InsertFormat_Click(object sender, EventArgs e)
        {
            var menuLocation = InsertFormat.Location;

            menuLocation.X += InsertFormat.Width;

            InsertFormat.ContextMenu.Show(InsertFormat.Parent, menuLocation);
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

        /// <summary>
        ///     Handles the TextChanged event of the NameFormat control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void NameFormat_TextChanged(object sender, EventArgs e)
        {
            BuildExample();
        }

        /// <summary>
        ///     Handles the Closing event of the NameFormatPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void NameFormatPage_Closing(object sender, EventArgs e)
        {
            State[Constants.NameFormatStateKey] = NameFormat.Text;
            State[Constants.IncrementOnCollisionStateKey] = IncrementOnCollision.Checked;
        }

        /// <summary>
        ///     Handles the Load event of the NameFormatPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
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

        /// <summary>
        ///     Handles the Opening event of the NameFormatPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void NameFormatPage_Opening(object sender, EventArgs e)
        {
            // Load the state values
            NameFormat.Text = (string) State[Constants.NameFormatStateKey];
            NameFormat.DataSource = State[Constants.NameFormatMRUStateKey];
            IncrementOnCollision.Checked = (bool) State[Constants.IncrementOnCollisionStateKey];

            // Build the example
            BuildExample();
        }

        /// <summary>
        ///     Sets the example value.
        /// </summary>
        /// <param name="example">
        ///     The example.
        /// </param>
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