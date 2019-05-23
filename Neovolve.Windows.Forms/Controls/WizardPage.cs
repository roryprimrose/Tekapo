namespace Neovolve.Windows.Forms.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    ///     The <see cref="WizardPage" /> class defines the base control used to create pages in the
    ///     <see cref="Neovolve.Windows.Forms.WizardForm" />form.
    /// </summary>
    public partial class WizardPage : UserControl
    {
        /// <summary>
        ///     Stores the navigation settings for the page.
        /// </summary>
        private WizardPageNavigationSettings _navigationSettings;

        /// <summary>
        ///     Stores the owner wizard form.
        /// </summary>
        private WizardForm _owner;

        /// <summary>
        ///     Stores the settings for the page.
        /// </summary>
        private WizardPageSettings _pageSettings;

        /// <summary>
        ///     Stores whether the wizard buttons are disabled.
        /// </summary>
        private bool _wizardButtonsDisabled;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPage" /> class.
        /// </summary>
        public WizardPage()
            : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPage" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owner.
        /// </param>
        public WizardPage(WizardForm owner)
        {
            InitializeComponent();

            // Assign the owner
            _owner = owner;
        }

        /// <summary>
        ///     Raised when the page has been removed as the current page in the owning
        ///     <see cref="Neovolve.Windows.Forms.WizardForm" />.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        ///     Raised when the page is about to be removed as the current page in the owning
        ///     <see cref="Neovolve.Windows.Forms.WizardForm" />.
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        ///     Raised when the page is has been made the current page in the owning
        ///     <see cref="Neovolve.Windows.Forms.WizardForm" />.
        /// </summary>
        public event EventHandler Opened;

        /// <summary>
        ///     Raised when the page is about to be the current page in the owning
        ///     <see cref="Neovolve.Windows.Forms.WizardForm" />.
        /// </summary>
        public event EventHandler Opening;

        /// <summary>
        ///     Raised when the user interface of the <see cref="Neovolve.Windows.Forms.WizardForm" /> needs to be updated.
        /// </summary>
        public event EventHandler UpdateWizardSettingsRequired;

        /// <summary>
        ///     Determines whether this instance can navigate the specified e.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        /// <returns>
        ///     <item>
        ///         True
        ///     </item>
        ///     if this instance can navigate the specified e; otherwise,
        ///     <item>
        ///         False
        ///     </item>
        ///     .
        /// </returns>
        public virtual bool CanNavigate(WizardFormNavigationEventArgs e)
        {
            return true;
        }

        /// <summary>
        ///     Invokes the custom navigation.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        public virtual void InvokeCustomNavigation(WizardFormNavigationEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Invokes the help navigation.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        public virtual void InvokeHelpNavigation(WizardFormNavigationEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Sets the owner.
        /// </summary>
        /// <param name="owner">
        ///     The owner.
        /// </param>
        internal void SetOwner(WizardForm owner)
        {
            _owner = owner;
        }

        /// <summary>
        ///     Raises the <see cref="Closed" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected internal virtual void OnClosed(EventArgs e)
        {
            // Check if there is a handler
            if (Closed != null)
            {
                // Raise the event
                Closed(this, e);
            }
        }

        /// <summary>
        ///     Raises the <see cref="Closing" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected internal virtual void OnClosing(EventArgs e)
        {
            // Check if there is a handler
            if (Closing != null)
            {
                // Raise the event
                Closing(this, e);
            }
        }

        /// <summary>
        ///     Raises the <see cref="Opened" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected internal virtual void OnOpened(EventArgs e)
        {
            // Check if there is a handler
            if (Opened != null)
            {
                // Raise the event
                Opened(this, e);
            }
        }

        /// <summary>
        ///     Raises the <see cref="Opening" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected internal virtual void OnOpening(EventArgs e)
        {
            // Check if there is a handler
            if (Opening != null)
            {
                // Raise the event
                Opening(this, e);
            }
        }

        /// <summary>
        ///     Raises the <see cref="UpdateWizardSettingsRequired" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected internal virtual void OnUpdateWizardSettingsRequired(EventArgs e)
        {
            // Check if there is a handler
            if (UpdateWizardSettingsRequired != null)
            {
                // Raise the event
                UpdateWizardSettingsRequired(this, e);
            }
        }

        /// <summary>
        ///     Invokes the navigation.
        /// </summary>
        /// <param name="navigationType">
        ///     Type of the navigation.
        /// </param>
        protected void InvokeNavigation(WizardFormNavigationType navigationType)
        {
            ValidateOwnerAndPage();

            // Invoke the navigation on the owner
            _owner.GenerateNavigationEvent(navigationType);
        }

        /// <summary>
        ///     Invokes the navigation.
        /// </summary>
        /// <param name="navigationKey">
        ///     The navigation key.
        /// </param>
        protected void InvokeNavigation(string navigationKey)
        {
            ValidateOwnerAndPage();

            // Invoke the navigation on the owner
            _owner.GenerateNavigationEvent(WizardFormNavigationType.NavigationKey, navigationKey);
        }

        /// <summary>
        ///     Validates the owner and page.
        /// </summary>
        private void ValidateOwnerAndPage()
        {
            // Check that the owner exists
            if (_owner == null)
            {
                throw new InvalidOperationException("The page isn't owned by a WizardForm.");
            }

            // Check that the current page is this page
            if (this != _owner.CurrentPage)
            {
                throw new InvalidOperationException("The page invoking the navigation is not the current page.");
            }
        }

        /// <summary>
        ///     Gets the current navigation settings.
        /// </summary>
        /// <value>
        ///     The current navigation settings.
        /// </value>
        [Browsable(false)]
        public virtual WizardPageNavigationSettings CurrentNavigationSettings => NavigationSettings;

        /// <summary>
        ///     Gets the current settings.
        /// </summary>
        /// <value>
        ///     The current settings.
        /// </value>
        [Browsable(false)]
        public virtual WizardPageSettings CurrentSettings
        {
            get
            {
                // Check if the wizard buttons should be disabled
                if (WizardButtonsDisabled == false)
                {
                    return PageSettings.Clone();
                }

                // The wizard buttons should be disabled
                // Get the current settings
                var pageSettings = PageSettings.Clone();

                // Disable all the buttons
                pageSettings.CancelButtonSettings.Enabled = false;
                pageSettings.CustomButtonSettings.Enabled = false;
                pageSettings.HelpButtonSettings.Enabled = false;
                pageSettings.NextButtonSettings.Enabled = false;
                pageSettings.BackButtonSettings.Enabled = false;

                // Return the settings
                return pageSettings;
            }
        }

        /// <summary>
        ///     Gets or sets the default focus.
        /// </summary>
        /// <value>
        ///     The default focus.
        /// </value>
        [Category("Behaviour")]
        [Description("Determines the control that will receive the focus by default.")]
        [DefaultValue(null)]
        public Control DefaultFocus { get; set; }

        /// <summary>
        ///     Gets or sets the navigation settings.
        /// </summary>
        /// <value>
        ///     The navigation settings.
        /// </value>
        [Category("Wizard")]
        [Description("The navigation settings of the wizard page.")]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardPageNavigationSettings NavigationSettings
        {
            get
            {
                // Check if there is a value
                if (_navigationSettings != null)
                {
                    return _navigationSettings;
                }

                // Create a new object
                _navigationSettings = new WizardPageNavigationSettings();

                // Return the value
                return _navigationSettings;
            }
            set => _navigationSettings = value;
        }

        /// <summary>
        ///     Gets or sets the page settings.
        /// </summary>
        /// <value>
        ///     The page settings.
        /// </value>
        [Category("Wizard")]
        [Description("The page settings of the wizard page.")]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardPageSettings PageSettings
        {
            get
            {
                // Check if there is a value
                if (_pageSettings != null)
                {
                    // Return the settings
                    return _pageSettings;
                }

                // Create the settings
                _pageSettings = new WizardPageSettings();

                // Return the settings
                return _pageSettings;
            }
            set => _pageSettings = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the wizard buttons disabled.
        /// </summary>
        /// <value>
        ///     <c>true</c>if the wizard buttons are disabled; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public bool WizardButtonsDisabled
        {
            get => _wizardButtonsDisabled;
            set
            {
                _wizardButtonsDisabled = value;

                // Update the UI
                OnUpdateWizardSettingsRequired(new EventArgs());
            }
        }

        /// <summary>
        ///     Gets the state data.
        /// </summary>
        /// <value>
        ///     The state data.
        /// </value>
        /// <exception cref="InvalidOperationException">
        ///     The control is not hosted in a <see cref="WizardForm" /> instance.
        /// </exception>
        [Browsable(false)]
        protected StateCollection State
        {
            get
            {
                if (_owner == null)
                {
                    throw new InvalidOperationException("Control is not hosted by a WizardForm.");
                }

                return _owner.State;
            }
        }
    }
}