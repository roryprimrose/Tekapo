using System;
using System.ComponentModel;

namespace Neovolve.Windows.Forms.Controls
{
    /// <summary>
    /// The <see cref="WizardPageNavigationSettings"/> class defines the page keys to navigate to when a previous,
    /// cancel, help or custom navigation is requested for a page.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WizardPageNavigationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageNavigationSettings"/> class.
        /// </summary>
        public WizardPageNavigationSettings()
            : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageNavigationSettings"/> class.
        /// </summary>
        /// <param name="nextPage">
        /// The next page.
        /// </param>
        public WizardPageNavigationSettings(String nextPage)
            : this(nextPage, String.Empty, String.Empty, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageNavigationSettings"/> class.
        /// </summary>
        /// <param name="nextPage">
        /// The next page.
        /// </param>
        /// <param name="previousPage">
        /// The previous page.
        /// </param>
        public WizardPageNavigationSettings(String nextPage, String previousPage)
            : this(nextPage, previousPage, String.Empty, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageNavigationSettings"/> class.
        /// </summary>
        /// <param name="nextPage">
        /// The next page.
        /// </param>
        /// <param name="previousPage">
        /// The previous page.
        /// </param>
        /// <param name="cancelPage">
        /// The cancel page.
        /// </param>
        public WizardPageNavigationSettings(String nextPage, String previousPage, String cancelPage)
            : this(nextPage, previousPage, cancelPage, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageNavigationSettings"/> class.
        /// </summary>
        /// <param name="nextPage">
        /// The next page.
        /// </param>
        /// <param name="previousPage">
        /// The previous page.
        /// </param>
        /// <param name="cancelPage">
        /// The cancel page.
        /// </param>
        /// <param name="helpPage">
        /// The help page.
        /// </param>
        public WizardPageNavigationSettings(String nextPage, String previousPage, String cancelPage, String helpPage)
            : this(nextPage, previousPage, cancelPage, helpPage, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageNavigationSettings"/> class.
        /// </summary>
        /// <param name="nextPage">
        /// The next page.
        /// </param>
        /// <param name="previousPage">
        /// The previous page.
        /// </param>
        /// <param name="cancelPage">
        /// The cancel page.
        /// </param>
        /// <param name="helpPage">
        /// The help page.
        /// </param>
        /// <param name="customPage">
        /// The custom page.
        /// </param>
        public WizardPageNavigationSettings(
            String nextPage, String previousPage, String cancelPage, String helpPage, String customPage)
        {
            NextPageKey = nextPage;
            PreviousPageKey = previousPage;
            CancelPageKey = cancelPage;
            HelpPageKey = helpPage;
            CustomPageKey = customPage;
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> that represents the current <see cref="Object"/>.
        /// </returns>
        public override String ToString()
        {
            return String.Empty;
        }

        /// <summary>
        /// Gets or sets the cancel page key.
        /// </summary>
        /// <value>
        /// The cancel page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Cancel action is requested.")]
        [DefaultValue("")]
        public String CancelPageKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom page key.
        /// </summary>
        /// <value>
        /// The custom page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Custom action is requested.")]
        [DefaultValue("")]
        public String CustomPageKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the help page key.
        /// </summary>
        /// <value>
        /// The help page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Help action is requested.")]
        [DefaultValue("")]
        public String HelpPageKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the next page key.
        /// </summary>
        /// <value>
        /// The next page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Next action is requested.")]
        [DefaultValue("")]
        public String NextPageKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the previous page key.
        /// </summary>
        /// <value>
        /// The previous page key.
        /// </value>
        [Category("Navigation")]
        [Description(
            "The navigation key used to identify the page to navigate " + "to when a Previous action is requested.")]
        [DefaultValue("")]
        public String PreviousPageKey
        {
            get;
            set;
        }
    }
}