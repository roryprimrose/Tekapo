namespace Neovolve.Windows.Forms.Controls
{
    using System.ComponentModel;
    using Neovolve.Windows.Forms.Properties;

    /// <summary>
    ///     The <see cref="WizardPageSettings" /> class defines settings to be applied to a
    ///     <see cref="WizardPage" />instance.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WizardPageSettings
    {
        /// <summary>
        ///     Stores the settings for the previous button.
        /// </summary>
        private WizardButtonSettings _backButtonSettings;

        /// <summary>
        ///     Stores the settings for the cancel button.
        /// </summary>
        private WizardButtonSettings _cancelButtonSettings;

        /// <summary>
        ///     Stores the settings for the custom button.
        /// </summary>
        private WizardButtonSettings _customButtonSettings;

        /// <summary>
        ///     Stores the settings for the help button.
        /// </summary>
        private WizardButtonSettings _helpButtonSettings;

        /// <summary>
        ///     Stores the settings for the next button.
        /// </summary>
        private WizardButtonSettings _nextButtonSettings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageSettings" /> class.
        /// </summary>
        public WizardPageSettings()
            : this(null, null, null, null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageSettings" /> class.
        /// </summary>
        /// <param name="nextButton">
        ///     The next button.
        /// </param>
        public WizardPageSettings(WizardButtonSettings nextButton)
            : this(nextButton, null, null, null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageSettings" /> class.
        /// </summary>
        /// <param name="nextButton">
        ///     The next button.
        /// </param>
        /// <param name="previousButton">
        ///     The previous button.
        /// </param>
        public WizardPageSettings(WizardButtonSettings nextButton, WizardButtonSettings previousButton)
            : this(nextButton, previousButton, null, null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageSettings" /> class.
        /// </summary>
        /// <param name="nextButton">
        ///     The next button.
        /// </param>
        /// <param name="previousButton">
        ///     The previous button.
        /// </param>
        /// <param name="cancelButton">
        ///     The cancel button.
        /// </param>
        public WizardPageSettings(
            WizardButtonSettings nextButton,
            WizardButtonSettings previousButton,
            WizardButtonSettings cancelButton)
            : this(nextButton, previousButton, cancelButton, null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageSettings" /> class.
        /// </summary>
        /// <param name="nextButton">
        ///     The next button.
        /// </param>
        /// <param name="previousButton">
        ///     The previous button.
        /// </param>
        /// <param name="cancelButton">
        ///     The cancel button.
        /// </param>
        /// <param name="helpButton">
        ///     The help button.
        /// </param>
        public WizardPageSettings(
            WizardButtonSettings nextButton,
            WizardButtonSettings previousButton,
            WizardButtonSettings cancelButton,
            WizardButtonSettings helpButton)
            : this(nextButton, previousButton, cancelButton, helpButton, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageSettings" /> class.
        /// </summary>
        /// <param name="nextButton">
        ///     The next button.
        /// </param>
        /// <param name="previousButton">
        ///     The previous button.
        /// </param>
        /// <param name="cancelButton">
        ///     The cancel button.
        /// </param>
        /// <param name="helpButton">
        ///     The help button.
        /// </param>
        /// <param name="customButton">
        ///     The custom button.
        /// </param>
        public WizardPageSettings(
            WizardButtonSettings nextButton,
            WizardButtonSettings previousButton,
            WizardButtonSettings cancelButton,
            WizardButtonSettings helpButton,
            WizardButtonSettings customButton)
        {
            _nextButtonSettings = nextButton;
            _backButtonSettings = previousButton;
            _cancelButtonSettings = cancelButton;
            _helpButtonSettings = helpButton;
            _customButtonSettings = customButton;
        }

        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///     A new object that is a copy of this instance.
        /// </returns>
        public WizardPageSettings Clone()
        {
            // Return the new instance
            return new WizardPageSettings(NextButtonSettings.Clone(),
                BackButtonSettings.Clone(),
                CancelButtonSettings.Clone(),
                HelpButtonSettings.Clone(),
                CustomButtonSettings.Clone());
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents the current <see cref="object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents the current <see cref="object" />.
        /// </returns>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        ///     Gets or sets the back button settings.
        /// </summary>
        /// <value>
        ///     The back button settings.
        /// </value>
        [Category("Button Settings")]
        [Description("The Previous button settings.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardButtonSettings BackButtonSettings
        {
            get
            {
                // Check if the settings exist
                if (_backButtonSettings != null)
                {
                    // Return the settings
                    return _backButtonSettings;
                }

                // Create the settings with the default values
                _backButtonSettings = new WizardButtonSettings(Resources.Back);

                // Return the settings
                return _backButtonSettings;
            }
            set { _backButtonSettings = value; }
        }

        /// <summary>
        ///     Gets or sets the cancel button settings.
        /// </summary>
        /// <value>
        ///     The cancel button settings.
        /// </value>
        [Category("Button Settings")]
        [Description("The Cancel button settings.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardButtonSettings CancelButtonSettings
        {
            get
            {
                // Check if the settings exist
                if (_cancelButtonSettings != null)
                {
                    // Return the settings
                    return _cancelButtonSettings;
                }

                // Create the settings with the default values
                _cancelButtonSettings = new WizardButtonSettings(Resources.Cancel);

                // Return the settings
                return _cancelButtonSettings;
            }
            set { _cancelButtonSettings = value; }
        }

        /// <summary>
        ///     Gets or sets the custom button settings.
        /// </summary>
        /// <value>
        ///     The custom button settings.
        /// </value>
        [Category("Button Settings")]
        [Description("The Custom button settings.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardButtonSettings CustomButtonSettings
        {
            get
            {
                // Check if the settings exist
                if (_customButtonSettings != null)
                {
                    // Return the settings
                    return _customButtonSettings;
                }

                // Create the settings with the default values
                _customButtonSettings = new WizardButtonSettings(Resources.Custom, true, false);

                // Return the settings
                return _customButtonSettings;
            }
            set { _customButtonSettings = value; }
        }

        /// <summary>
        ///     Gets or sets the help button settings.
        /// </summary>
        /// <value>
        ///     The help button settings.
        /// </value>
        [Category("Button Settings")]
        [Description("The Help button settings.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardButtonSettings HelpButtonSettings
        {
            get
            {
                // Check if the settings exist
                if (_helpButtonSettings != null)
                {
                    // Return the settings
                    return _helpButtonSettings;
                }

                // Create the settings with the default values
                _helpButtonSettings = new WizardButtonSettings(Resources.Help, true, false);

                // Return the settings
                return _helpButtonSettings;
            }
            set { _helpButtonSettings = value; }
        }

        /// <summary>
        ///     Gets or sets the next button settings.
        /// </summary>
        /// <value>
        ///     The next button settings.
        /// </value>
        [Category("Button Settings")]
        [Description("The Next button settings.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardButtonSettings NextButtonSettings
        {
            get
            {
                // Check if the settings exist
                if (_nextButtonSettings != null)
                {
                    // Return the settings
                    return _nextButtonSettings;
                }

                // Create the settings with the default values
                _nextButtonSettings = new WizardButtonSettings(Resources.Next);

                // Return the settings
                return _nextButtonSettings;
            }
            set { _nextButtonSettings = value; }
        }
    }
}