using System;
using Neovolve.Windows.Forms.Controls;

namespace Neovolve.Windows.Forms
{
    /// <summary>
    /// The <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs"/> class defines the event argument information when a
    /// <see cref="Neovolve.Windows.Forms.WizardForm.Navigating"/>event is fired.
    /// </summary>
    public class WizardFormNavigationEventArgs : EventArgs
    {
        /// <summary>
        /// Stores a reference to the current page.
        /// </summary>
        private readonly WizardPage _currentPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs"/> class.
        /// </summary>
        /// <param name="currentPageValue">
        /// The current page value.
        /// </param>
        public WizardFormNavigationEventArgs(WizardPage currentPageValue)
            : this(currentPageValue, WizardFormNavigationType.Next, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs"/> class.
        /// </summary>
        /// <param name="currentPageValue">
        /// The current page value.
        /// </param>
        /// <param name="navigationValue">
        /// The navigation value.
        /// </param>
        public WizardFormNavigationEventArgs(WizardPage currentPageValue, WizardFormNavigationType navigationValue)
            : this(currentPageValue, navigationValue, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Neovolve.Windows.Forms.WizardFormNavigationEventArgs"/> class.
        /// </summary>
        /// <param name="currentPageValue">
        /// The current page value.
        /// </param>
        /// <param name="navigationType">
        /// The navigation value.
        /// </param>
        /// <param name="navigationKey">
        /// The custom navigation value.
        /// </param>
        public WizardFormNavigationEventArgs(
            WizardPage currentPageValue, WizardFormNavigationType navigationType, String navigationKey)
        {
            _currentPage = currentPageValue;
            NavigationType = navigationType;
            NavigationKey = navigationKey;
        }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public WizardPage CurrentPage
        {
            get
            {
                return _currentPage;
            }
        }

        /// <summary>
        /// Gets or sets the navigation key.
        /// </summary>
        /// <value>
        /// The navigation key.
        /// </value>
        public String NavigationKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the navigation.
        /// </summary>
        /// <value>
        /// The type of the navigation.
        /// </value>
        public WizardFormNavigationType NavigationType
        {
            get;
            set;
        }
    }
}