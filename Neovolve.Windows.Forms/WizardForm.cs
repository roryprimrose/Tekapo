namespace Neovolve.Windows.Forms
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Neovolve.Windows.Forms.Controls;

    /// <summary>
    ///     The <see cref="Neovolve.Windows.Forms.WizardForm" /> class is a wizard style form that contains the logic of
    ///     managing the navigation flow
    ///     of <see cref="WizardPage" /> derived controls.
    /// </summary>
    public partial class WizardForm : Form
    {
        /// <summary>
        ///     Stores the navigation history from the start page to the current page.
        /// </summary>
        private readonly Stack<WizardPage> _pageHistory;

        /// <summary>
        ///     Stores the set of wizard pages.
        /// </summary>
        private readonly WizardPageDictionary _pages;

        /// <summary>
        ///     Stores the state information for the wizard.
        /// </summary>
        private readonly StateCollection _state;

        /// <summary>
        ///     Stores the AutomaticTabOrdering value.
        /// </summary>
        private bool _autoTabOrdering = true;

        /// <summary>
        ///     Stores whether a confirmation message will be displayed when the Cancel navigation is invoked.
        /// </summary>
        private bool _confirmCancel = true;

        /// <summary>
        ///     Stores the current page.
        /// </summary>
        private WizardPage _currentPage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardForm" /> class.
        /// </summary>
        public WizardForm()
        {
            components = new Container();

            InitializeComponent();

            // Create the collections
            _pages = new WizardPageDictionary();

            components.Add(_pages);

            _pageHistory = new Stack<WizardPage>();
            _state = new StateCollection();

            // Hook up the page collection events
            _pages.PageAdded += PageAdded;
            _pages.PageRemoved += PageRemoved;
        }
        
        /// <summary>
        ///     Defines the delegate used to close the wizard.
        /// </summary>
        /// <param name="result">
        ///     The dialog result to assign.
        /// </param>
        private delegate void CloseWizardDelegate(DialogResult result);

        /// <summary>
        ///     Defines the delegate used to switch threads when a navigation event is generated.
        /// </summary>
        /// <param name="navigationType">
        ///     Type of the navigation.
        /// </param>
        /// <param name="navigationKey">
        ///     The navigation key.
        /// </param>
        private delegate void GenerateNavigationEventDelegate(
            WizardFormNavigationType navigationType,
            string navigationKey);

        /// <summary>
        ///     Defines a parameterless thread switch delegate.
        /// </summary>
        private delegate void ThreadSwitchDelegate();

        /// <summary>
        ///     Raised when a navigation has occurred.
        /// </summary>
        public event EventHandler<WizardFormNavigationEventArgs> Navigated;

        /// <summary>
        ///     Raised when a navigation is about to occur.
        /// </summary>
        public event EventHandler<WizardFormNavigationEventArgs> Navigating;

        /// <summary>
        ///     Generates the navigation event.
        /// </summary>
        /// <param name="navigationType">
        ///     Type of the navigation.
        /// </param>
        internal void GenerateNavigationEvent(WizardFormNavigationType navigationType)
        {
            var page = CurrentPage;
            string navigationKey;

            // Determine the navigation key
            switch (navigationType)
            {
                case WizardFormNavigationType.Previous:

                    navigationKey = page.CurrentNavigationSettings.PreviousPageKey;

                    break;

                case WizardFormNavigationType.Next:

                    navigationKey = page.CurrentNavigationSettings.NextPageKey;

                    break;

                case WizardFormNavigationType.Cancel:

                    navigationKey = page.CurrentNavigationSettings.CancelPageKey;

                    break;

                case WizardFormNavigationType.Help:

                    navigationKey = page.CurrentNavigationSettings.HelpPageKey;

                    break;

                case WizardFormNavigationType.Custom:

                    navigationKey = page.CurrentNavigationSettings.CustomPageKey;

                    break;

                default:

                    throw new NotSupportedException();
            }

            // Forward the call to the overload
            GenerateNavigationEvent(navigationType, navigationKey);
        }

        /// <summary>
        ///     Generates the navigation event.
        /// </summary>
        /// <param name="navigationType">
        ///     Type of the navigation.
        /// </param>
        /// <param name="navigationKey">
        ///     The navigation key.
        /// </param>
        internal void GenerateNavigationEvent(WizardFormNavigationType navigationType, string navigationKey)
        {
            // Check if a thread switch is required
            if (InvokeRequired)
            {
                // Invoke the method on the UI thread
                object[] args = {navigationType, navigationKey};
                Invoke(new GenerateNavigationEventDelegate(GenerateNavigationEvent), args);

                return;
            }

            var page = CurrentPage;

            // Check if the type is for a specific key where no key is provided
            if (navigationType == WizardFormNavigationType.NavigationKey
                && string.IsNullOrEmpty(navigationKey))
            {
                throw new ArgumentNullException("navigationKey");
            }

            // Invoke the navigation
            OnNavigate(new WizardFormNavigationEventArgs(page, navigationType, navigationKey));
        }

        /// <summary>
        ///     Calculates the tab ordering.
        /// </summary>
        protected virtual void CalculateTabOrdering()
        {
            SetContainerTabOrdering(this, 0);
        }

        /// <summary>
        ///     Navigates the cancel.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void NavigateCancel(WizardFormNavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Clear the dialog result
            DialogResult = DialogResult.None;

            // Get the navigation key
            var navigationKey = e.NavigationKey;

            // Check if there is a navigation key
            if (string.IsNullOrEmpty(navigationKey) == false)
            {
                // Set the next page
                CurrentPage = Pages[navigationKey];
            }
            else
            {
                // There isn't a page defined for the cancel action

                // Close the wizard
                CloseWizard(DialogResult.Cancel);
            }
        }

        /// <summary>
        ///     Navigates the custom.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void NavigateCustom(WizardFormNavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Get the navigation key
            var navigationKey = e.NavigationKey;

            // Check if there is a navigation key
            if (string.IsNullOrEmpty(navigationKey) == false)
            {
                // Set the next page
                CurrentPage = Pages[navigationKey];
            }
            else
            {
                // There isn't a page defined for the custom action
                e.CurrentPage.InvokeCustomNavigation(e);

                // Check if the navigation details have changed
                if (e.NavigationType != WizardFormNavigationType.Custom
                    || string.IsNullOrEmpty(e.NavigationKey) == false)
                {
                    // The page has specified a new navigation value

                    // Call the navigation method again
                    OnNavigate(e);
                }
            }
        }

        /// <summary>
        ///     Navigates the help.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void NavigateHelp(WizardFormNavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Get the navigation key
            var navigationKey = e.NavigationKey;

            // Check if there is a navigation key
            if (string.IsNullOrEmpty(navigationKey) == false)
            {
                // Set the next page
                CurrentPage = Pages[navigationKey];
            }
            else
            {
                // There isn't a page defined for the help action
                e.CurrentPage.InvokeHelpNavigation(e);
            }
        }

        /// <summary>
        ///     Navigates the next.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void NavigateNext(WizardFormNavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Get the navigation key
            var navigationKey = e.NavigationKey;
            WizardPage page;

            // Check if there is a navigation key
            if (string.IsNullOrEmpty(navigationKey) == false)
            {
                // Get the next page
                page = Pages[navigationKey];
            }
            else
            {
                // Get the next page in the collection
                page = Pages.GetNextStoredPage(e.CurrentPage);
            }

            // Check if there is a next page
            if (page != null)
            {
                // Set the next page
                CurrentPage = page;
            }
            else
            {
                // There isn't a page defined for the next action
                // This is the end of the wizard
                CloseWizard(DialogResult.OK);
            }
        }

        /// <summary>
        ///     Navigates the previous.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void NavigatePrevious(WizardFormNavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Get the navigation key
            var navigationKey = e.NavigationKey;
            WizardPage page;

            // Check if there is a navigation key
            if (string.IsNullOrEmpty(navigationKey) == false)
            {
                // Get the next page to navigate to
                page = Pages[navigationKey];
            }
            else
            {
                // Get the previous page in the history
                page = _pageHistory.Peek();
            }

            // Check if there is a next page
            if (page != null)
            {
                // Set the next page
                CurrentPage = page;
            }
        }

        /// <summary>
        ///     Raises the <see cref="Navigating" /> and <see cref="Navigated" /> events.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void OnNavigate(WizardFormNavigationEventArgs e)
        {
            OnNavigating(e);
            OnNavigated(e);
        }

        /// <summary>
        ///     Raises the <see cref="Navigated" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void OnNavigated(WizardFormNavigationEventArgs e)
        {
            // Check if anything is handling the event
            if (Navigated != null)
            {
                // Raise the event
                Navigated(this, e);
            }
        }

        /// <summary>
        ///     Raises the <see cref="Navigating" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void OnNavigating(WizardFormNavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Check if the ignore navigation has been specified
            if (e.NavigationType == WizardFormNavigationType.Ignore)
            {
                return;
            }

            // Check if the current page will allow the navigation
            if (e.CurrentPage.CanNavigate(e) == false)
            {
                // The current page will not allow the navigation to continue
                // This will normally be because of validation in the page has failed
                return;
            }

            // Check if anything is handling the event
            if (Navigating != null)
            {
                // Raise the event
                Navigating(this, e);
            }

            switch (e.NavigationType)
            {
                case WizardFormNavigationType.Previous:

                    NavigatePrevious(e);

                    break;

                case WizardFormNavigationType.Next:

                    NavigateNext(e);

                    break;

                case WizardFormNavigationType.Cancel:

                    NavigateCancel(e);

                    break;

                case WizardFormNavigationType.Help:

                    NavigateCustom(e);

                    break;

                case WizardFormNavigationType.Custom:

                    NavigateCustom(e);

                    break;

                case WizardFormNavigationType.NavigationKey:

                    NavigateKey(e);

                    break;

                default:

                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Handles the PageRemoved event of the WizardPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="WizardPageDictionaryEventArgs" /> instance containing the event data.
        /// </param>
        private static void PageRemoved(object sender, WizardPageDictionaryEventArgs e)
        {
            // Remove this form as the owner of the page
            e.Page.SetOwner(null);
        }

        /// <summary>
        ///     Sets the container tab ordering.
        /// </summary>
        /// <param name="container">
        ///     The container.
        /// </param>
        /// <param name="currentIndex">
        ///     Index of the current.
        /// </param>
        /// <returns>
        ///     The current tab index of the form.
        /// </returns>
        private static int SetContainerTabOrdering(Control container, int currentIndex)
        {
            // Assign the new tab index for this item
            container.TabIndex = currentIndex;

            // Increment the index
            currentIndex++;

            var sortedControls = new ArrayList(container.Controls);
            sortedControls.Sort(new ControlPositionComparer());

            foreach (Control item in sortedControls)
            {
                // Control has children -- recurse.
                currentIndex = SetContainerTabOrdering(item, currentIndex);
            }

            return currentIndex;
        }

        /// <summary>
        ///     Handles the Click event of the Back control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Back_Click(object sender, EventArgs e)
        {
            GenerateNavigationEvent(WizardFormNavigationType.Previous);
        }

        /// <summary>
        ///     Handles the Paint event of the BottomPanel control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.
        /// </param>
        private void BottomPanel_Paint(object sender, PaintEventArgs e)
        {
            // Paint the border
            e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, BottomPanel.Width, 0);
            e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, 1, BottomPanel.Width, 1);
        }

        /// <summary>
        ///     Handles the Click event of the Cancel control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            GenerateNavigationEvent(WizardFormNavigationType.Cancel);
        }

        /// <summary>
        ///     Determines whether this instance can continue the cancel operation.
        /// </summary>
        /// <returns>
        ///     <item>
        ///         True
        ///     </item>
        ///     if this instance can continue the cancel operation.; otherwise,
        ///     <item>
        ///         False
        ///     </item>
        ///     .
        /// </returns>
        private bool CanContinueCancel()
        {
            // Confirm if the user wants to cancel
            if (ConfirmCancel && MessageBox.Show("Are you sure you want to cancel?",
                    Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Closes the wizard.
        /// </summary>
        /// <param name="result">
        ///     The result.
        /// </param>
        private void CloseWizard(DialogResult result)
        {
            // Check if a thread switch is required
            if (InvokeRequired)
            {
                object[] args = {result};
                Invoke(new CloseWizardDelegate(CloseWizard), args);

                return;
            }

            if (result == DialogResult.OK)
            {
                IgnoreCancelCheck = true;
            }

            DialogResult = result;

            // Close the dialog
            Close();
        }

        /// <summary>
        ///     Configures the current page.
        /// </summary>
        private void ConfigureCurrentPage()
        {
            // Check if a thread switch is required
            if (InvokeRequired)
            {
                // Switch threads
                Invoke(new ThreadSwitchDelegate(ConfigureCurrentPage), null);

                return;
            }

            // Get the current settings for the page
            var currentSettings = _currentPage.CurrentSettings;

            // Set up the next button
            Next.Text = currentSettings.NextButtonSettings.Text;
            Next.Enabled = currentSettings.NextButtonSettings.Enabled;
            Next.Visible = currentSettings.NextButtonSettings.Visible;

            // Set up the previous button
            Back.Text = currentSettings.BackButtonSettings.Text;
            Back.Enabled = _pageHistory.Count > 0 && currentSettings.BackButtonSettings.Enabled;
            Back.Visible = currentSettings.BackButtonSettings.Visible;

            // Set up the cancel button
            Cancel.Text = currentSettings.CancelButtonSettings.Text;
            Cancel.Enabled = currentSettings.CancelButtonSettings.Enabled;
            Cancel.Visible = currentSettings.CancelButtonSettings.Visible;

            // Set up the help button
            Help.Text = currentSettings.HelpButtonSettings.Text;
            Help.Enabled = currentSettings.HelpButtonSettings.Enabled;
            Help.Visible = currentSettings.HelpButtonSettings.Visible;

            // Set up the custom button
            Custom.Text = currentSettings.CustomButtonSettings.Text;
            Custom.Enabled = currentSettings.CustomButtonSettings.Enabled;
            Custom.Visible = currentSettings.CustomButtonSettings.Visible;
        }

        /// <summary>
        ///     Handles the Click event of the Custom control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Custom_Click(object sender, EventArgs e)
        {
            GenerateNavigationEvent(WizardFormNavigationType.Custom);
        }

        /// <summary>
        ///     Handles the Click event of the Help control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Help_Click(object sender, EventArgs e)
        {
            GenerateNavigationEvent(WizardFormNavigationType.Help);
        }

        /// <summary>
        ///     Navigates to the page with the specified key.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs" /> instance containing the event data.
        /// </param>
        private void NavigateKey(WizardFormNavigationEventArgs e)
        {
            // Set the next page
            CurrentPage = Pages[e.NavigationKey];
        }

        /// <summary>
        ///     Handles the Click event of the Next control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Next_Click(object sender, EventArgs e)
        {
            GenerateNavigationEvent(WizardFormNavigationType.Next);
        }

        /// <summary>
        ///     Handles the PageAdded event of the WizardPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="WizardPageDictionaryEventArgs" /> instance containing the event data.
        /// </param>
        private void PageAdded(object sender, WizardPageDictionaryEventArgs e)
        {
            // Assign this form as the owner
            e.Page.SetOwner(this);

            // Check if there is a current page
            if (_currentPage == null)
            {
                // Set this first page as the current page
                CurrentPage = e.Page;
            }
        }

        /// <summary>
        ///     Populates the current page.
        /// </summary>
        private void PopulateCurrentPage()
        {
            // Check if a thread switch is required
            if (InvokeRequired)
            {
                // Switch threads
                Invoke(new ThreadSwitchDelegate(PopulateCurrentPage), null);

                return;
            }

            // Set up the wizard page
            _currentPage.Dock = DockStyle.Fill;

            ConfigureCurrentPage();

            // Add the page to the form
            PagePanel.Controls.Add(_currentPage);

            // Check if the tab ordering needs to be calculated
            if (AutoTabOrdering)
            {
                // Calculate the tab ordering
                CalculateTabOrdering();
            }

            if (_currentPage.DefaultFocus != null
                && _currentPage.DefaultFocus.CanFocus)
            {
                _currentPage.DefaultFocus.Focus();
            }
            else if (Next.CanFocus)
            {
                Next.Focus();
            }
        }

        /// <summary>
        ///     Removes the current page.
        /// </summary>
        private void RemoveCurrentPage()
        {
            // Check if a thread switch is required
            if (InvokeRequired)
            {
                // Switch threads
                Invoke(new ThreadSwitchDelegate(RemoveCurrentPage), null);

                return;
            }

            // Remove the old page
            PagePanel.Controls.Clear();
        }

        /// <summary>
        ///     Handles the UpdateWizardSettingsRequired event of the WizardPage control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void UpdateWizardSettingsRequired(object sender, EventArgs e)
        {
            // Update the state of the wizard UI
            ConfigureCurrentPage();
        }

        /// <summary>
        ///     Handles the FormClosed event of the WizardForm control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.FormClosedEventArgs" /> instance containing the event data.
        /// </param>
        private void WizardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Notify the current page that it is closed
            if (_currentPage != null)
            {
                _currentPage.OnClosed(new EventArgs());
            }
        }

        /// <summary>
        ///     Handles the FormClosing event of the WizardForm control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.FormClosingEventArgs" /> instance containing the event data.
        /// </param>
        private void WizardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the user wants the cancel to continue
            if (IgnoreCancelCheck == false
                && e.CloseReason == CloseReason.UserClosing
                && CanContinueCancel() == false)
            {
                e.Cancel = true;

                return;
            }

            // Notify the current page that it is closing
            if (_currentPage != null)
            {
                _currentPage.OnClosing(new EventArgs());

                // Remove the event handler for the current page
                _currentPage.UpdateWizardSettingsRequired -= UpdateWizardSettingsRequired;
            }

            // Remove the page collection events
            _pages.PageAdded -= PageAdded;
            _pages.PageRemoved -= PageRemoved;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether automatic tab ordering is used.
        /// </summary>
        /// <value>
        ///     <item>
        ///         True
        ///     </item>
        ///     if automatic tab ordering is used; otherwise,
        ///     <item>
        ///         False
        ///     </item>
        ///     .
        /// </value>
        [Category("Behaviour")]
        [Description("Determines whether control tab ordering is automatically calculated based on positioning.")]
        [DefaultValue(true)]
        public bool AutoTabOrdering { get => _autoTabOrdering; set => _autoTabOrdering = value; }

        /// <summary>
        ///     Gets or sets a value indicating whether [confirm cancel].
        /// </summary>
        /// <value>
        ///     <item>
        ///         True
        ///     </item>
        ///     if [confirm cancel]; otherwise,
        ///     <item>
        ///         False
        ///     </item>
        ///     .
        /// </value>
        [Category("Behaviour")]
        [Description("Determines whether the user will be asked to confirm a cancel action.")]
        [DefaultValue(true)]
        public bool ConfirmCancel { get => _confirmCancel; set => _confirmCancel = value; }

        /// <summary>
        ///     Gets or sets the current page.
        /// </summary>
        /// <value>
        ///     The current page.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     The value is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The new value is not in the collection of pages.
        /// </exception>
        [Browsable(false)]
        public WizardPage CurrentPage
        {
            get
            {
                // Check if there is a current page
                if (_currentPage != null)
                {
                    // Return the current page
                    return _currentPage;
                }

                // Check if there are any pages
                if (Pages.Count > 0)
                {
                    // Get the first page
                    _currentPage = Pages.GetNextStoredPage(null);

                    // Return the current page
                    return _currentPage;
                }

                // There is no page available
                return null;
            }
            set
            {
                // Check if a value has been supplied
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                // Check that the page exists in the collection
                if (Pages.ContainsValue(value) == false)
                {
                    throw new InvalidOperationException("Page not found.");
                }

                // Get a reference to the old current page
                var oldPage = _currentPage;

                // Check if the navigation history contains the specified item
                if (_pageHistory.Contains(value))
                {
                    // We are going back in history

                    // Clear out all the items in the stack that are either the item requested or after the item requested
                    WizardPage previousPage;

                    do
                    {
                        previousPage = _pageHistory.Pop();
                    } while (previousPage.Equals(value) == false);
                }
                else if (oldPage != null)
                {
                    // We are going forward in history

                    // Put the old page into the stack
                    _pageHistory.Push(oldPage);
                }

                // Check if there is a current page
                if (oldPage != null)
                {
                    // Remove the event handler for the current page
                    oldPage.UpdateWizardSettingsRequired -= UpdateWizardSettingsRequired;

                    // Inform the old page that it is being removed
                    oldPage.OnClosing(new EventArgs());
                }

                // Remove the current page
                RemoveCurrentPage();

                // Check if there is an old page
                if (oldPage != null)
                {
                    // Inform the page that it has been removed
                    oldPage.OnClosed(new EventArgs());
                }

                // Store the new current page
                _currentPage = value;

                // Add the event handler for the current page
                _currentPage.UpdateWizardSettingsRequired += UpdateWizardSettingsRequired;

                // Inform the new page that it is being added
                _currentPage.OnOpening(new EventArgs());

                // Populate the current page
                PopulateCurrentPage();

                // Inform the new page that it has been added
                _currentPage.OnOpened(new EventArgs());
            }
        }

        /// <summary>
        ///     Gets the pages.
        /// </summary>
        /// <value>
        ///     The pages.
        /// </value>
        [Browsable(false)]
        public WizardPageDictionary Pages => _pages;

        /// <summary>
        ///     Gets the state.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        [Browsable(false)]
        public StateCollection State => _state;

        /// <summary>
        ///     Gets or sets a value indicating whether to ignore the cancel check.
        /// </summary>
        /// <value>
        ///     <item>
        ///         True
        ///     </item>
        ///     if the cancel check should be ignored; otherwise,
        ///     <item>
        ///         False
        ///     </item>
        ///     .
        /// </value>
        [Browsable(false)]
        private bool IgnoreCancelCheck { get; set; }
    }
}