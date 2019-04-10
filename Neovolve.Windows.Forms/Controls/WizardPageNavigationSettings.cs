namespace Neovolve.Windows.Forms.Controls
{
    using System.ComponentModel;

    /// <summary>
    ///     The <see cref="WizardPageNavigationSettings" /> class defines the page keys to navigate to when a previous,
    ///     cancel, help or custom navigation is requested for a page.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WizardPageNavigationSettings
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageNavigationSettings" /> class.
        /// </summary>
        public WizardPageNavigationSettings()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageNavigationSettings" /> class.
        /// </summary>
        /// <param name="nextPage">
        ///     The next page.
        /// </param>
        public WizardPageNavigationSettings(string nextPage)
            : this(nextPage, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageNavigationSettings" /> class.
        /// </summary>
        /// <param name="nextPage">
        ///     The next page.
        /// </param>
        /// <param name="previousPage">
        ///     The previous page.
        /// </param>
        public WizardPageNavigationSettings(string nextPage, string previousPage)
            : this(nextPage, previousPage, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageNavigationSettings" /> class.
        /// </summary>
        /// <param name="nextPage">
        ///     The next page.
        /// </param>
        /// <param name="previousPage">
        ///     The previous page.
        /// </param>
        /// <param name="cancelPage">
        ///     The cancel page.
        /// </param>
        public WizardPageNavigationSettings(string nextPage, string previousPage, string cancelPage)
            : this(nextPage, previousPage, cancelPage, string.Empty, string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageNavigationSettings" /> class.
        /// </summary>
        /// <param name="nextPage">
        ///     The next page.
        /// </param>
        /// <param name="previousPage">
        ///     The previous page.
        /// </param>
        /// <param name="cancelPage">
        ///     The cancel page.
        /// </param>
        /// <param name="helpPage">
        ///     The help page.
        /// </param>
        public WizardPageNavigationSettings(string nextPage, string previousPage, string cancelPage, string helpPage)
            : this(nextPage, previousPage, cancelPage, helpPage, string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageNavigationSettings" /> class.
        /// </summary>
        /// <param name="nextPage">
        ///     The next page.
        /// </param>
        /// <param name="previousPage">
        ///     The previous page.
        /// </param>
        /// <param name="cancelPage">
        ///     The cancel page.
        /// </param>
        /// <param name="helpPage">
        ///     The help page.
        /// </param>
        /// <param name="customPage">
        ///     The custom page.
        /// </param>
        public WizardPageNavigationSettings(
            string nextPage,
            string previousPage,
            string cancelPage,
            string helpPage,
            string customPage)
        {
            NextPageKey = nextPage;
            PreviousPageKey = previousPage;
            CancelPageKey = cancelPage;
            HelpPageKey = helpPage;
            CustomPageKey = customPage;
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
        ///     Gets or sets the cancel page key.
        /// </summary>
        /// <value>
        ///     The cancel page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Cancel action is requested.")]
        [DefaultValue("")]
        public string CancelPageKey { get; set; }

        /// <summary>
        ///     Gets or sets the custom page key.
        /// </summary>
        /// <value>
        ///     The custom page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Custom action is requested.")]
        [DefaultValue("")]
        public string CustomPageKey { get; set; }

        /// <summary>
        ///     Gets or sets the help page key.
        /// </summary>
        /// <value>
        ///     The help page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Help action is requested.")]
        [DefaultValue("")]
        public string HelpPageKey { get; set; }

        /// <summary>
        ///     Gets or sets the next page key.
        /// </summary>
        /// <value>
        ///     The next page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate to when a Next action is requested.")]
        [DefaultValue("")]
        public string NextPageKey { get; set; }

        /// <summary>
        ///     Gets or sets the previous page key.
        /// </summary>
        /// <value>
        ///     The previous page key.
        /// </value>
        [Category("Navigation")]
        [Description("The navigation key used to identify the page to navigate "
                     + "to when a Previous action is requested.")]
        [DefaultValue("")]
        public string PreviousPageKey { get; set; }
    }
}