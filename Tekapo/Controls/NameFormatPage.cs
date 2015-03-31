using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Neovolve.Windows.Forms;
using Neovolve.Windows.Forms.Controls;
using Tekapo.Processing;
using Tekapo.Properties;

namespace Tekapo.Controls
{
    /// <summary>
    /// The <see cref="NameFormatPage"/> class is a Wizard page that allows the user to specify a format to use when
    /// renaming images.
    /// </summary>
    public partial class NameFormatPage : WizardBannerPage
    {
        /// <summary>
        /// Stores the thread used to calculate the rename example.
        /// </summary>
        private Thread _exampleThread;

        /// <summary>
        /// Get format value delegate.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> value.
        /// </returns>
        private delegate String GetFormatValueDelegate();

        /// <summary>
        /// Set example value delegate.
        /// </summary>
        /// <param name="example">
        /// The example.
        /// </param>
        private delegate void SetExampleValueDelegate(String example);

        /// <summary>
        /// Initializes a new instance of the <see cref="NameFormatPage"/> class.
        /// </summary>
        public NameFormatPage()
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
            if ((e.NavigationType == WizardFormNavigationType.Next)
                && (IsPageValid() == false))
            {
                // The page isn't valid
                return false;
            }

            return base.CanNavigate(e);
        }

        /// <summary>
        /// Updates the example.
        /// </summary>
        private void BuildExample()
        {
            if ((_exampleThread != null)
                && ((_exampleThread.ThreadState & ThreadState.Running) == ThreadState.Running))
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
        /// Calculates the example value.
        /// </summary>
        private void CalculateExampleValue()
        {
            String renameFormat = GetFormatValue();
            String exampleMessage;

            // Check if a format is specified
            if (String.IsNullOrEmpty(renameFormat))
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
                String sourcePath = ((BindingList<String>)State[Constants.FileListStateKey])[0];
                DateTime pictureTaken = JpegInformation.GetPictureTaken(sourcePath);
                Boolean incrementOnCollision = IncrementOnCollision.Checked;
                Int32 maxCollisionIncrement = Settings.Default.MaxCollisionIncrement;
                String resultPath = ImageRenaming.GetRenamedPath(
                    renameFormat, pictureTaken, sourcePath, incrementOnCollision, maxCollisionIncrement);
                String exampleFormat = Resources.NameFormatExample.Replace("\\n", "\n");

                exampleMessage = String.Format(CultureInfo.CurrentUICulture, exampleFormat, sourcePath, resultPath);
            }

            SetExampleValue(exampleMessage);
        }

        /// <summary>
        /// Gets the format value.
        /// </summary>
        /// <returns>
        /// The format value.
        /// </returns>
        private String GetFormatValue()
        {
            if (NameFormat.InvokeRequired)
            {
                return (String)NameFormat.Invoke(new GetFormatValueDelegate(GetFormatValue));
            }

            return NameFormat.Text;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the IncrementOnCollision control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void IncrementOnCollision_CheckedChanged(Object sender, EventArgs e)
        {
            BuildExample();
        }

        /// <summary>
        /// Handles the Click event of the InsertFormat control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void InsertFormat_Click(Object sender, EventArgs e)
        {
            Point menuLocation = InsertFormat.Location;

            menuLocation.X += InsertFormat.Width;

            InsertFormat.ContextMenu.Show(InsertFormat.Parent, menuLocation);
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

            if (String.IsNullOrEmpty(NameFormat.Text))
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
        /// Handles the TextChanged event of the NameFormat control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void NameFormat_TextChanged(Object sender, EventArgs e)
        {
            BuildExample();
        }

        /// <summary>
        /// Handles the Closing event of the NameFormatPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void NameFormatPage_Closing(Object sender, EventArgs e)
        {
            State[Constants.NameFormatStateKey] = NameFormat.Text;
            State[Constants.IncrementOnCollisionStateKey] = IncrementOnCollision.Checked;
        }

        /// <summary>
        /// Handles the Load event of the NameFormatPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void NameFormatPage_Load(Object sender, EventArgs e)
        {
            ContextMenu formatMenu = new ContextMenu();

            // Populate the rename formats into the context menu
            foreach (KeyValuePair<String, String> pair in ImageRenaming.RenameFormats)
            {
                MenuItem renameMenuItem = new MenuItem();
                String formatValue = String.Concat("<", pair.Key, ">");
                String formatDescription = pair.Value;

                renameMenuItem.Text = String.Format(
                    CultureInfo.CurrentUICulture, "{0}  {1}", formatValue, formatDescription);
                renameMenuItem.Tag = formatValue;

                renameMenuItem.Click += delegate
                                        {
                                            NameFormat.Text += formatValue;
                                        };

                formatMenu.MenuItems.Add(renameMenuItem);
            }

            InsertFormat.ContextMenu = formatMenu;
        }

        /// <summary>
        /// Handles the Opening event of the NameFormatPage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void NameFormatPage_Opening(Object sender, EventArgs e)
        {
            // Load the state values
            NameFormat.Text = (String)State[Constants.NameFormatStateKey];
            NameFormat.DataSource = State[Constants.NameFormatMRUStateKey];
            IncrementOnCollision.Checked = (Boolean)State[Constants.IncrementOnCollisionStateKey];

            // Build the example
            BuildExample();
        }

        /// <summary>
        /// Sets the example value.
        /// </summary>
        /// <param name="example">
        /// The example.
        /// </param>
        private void SetExampleValue(String example)
        {
            if (Example.InvokeRequired)
            {
                Object[] args = new Object[]
                                    {
                                        example
                                    };

                Example.Invoke(new SetExampleValueDelegate(SetExampleValue), args);

                return;
            }

            Example.Text = example;
        }
    }
}