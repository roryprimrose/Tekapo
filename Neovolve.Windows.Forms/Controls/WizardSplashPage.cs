namespace Neovolve.Windows.Forms.Controls
{
    using System.Drawing;

    /// <summary>
    ///     The <see cref="WizardSplashPage" /> class is a
    ///     <see cref="WizardPage" />derived control that defines a splash page for the wizard interface.
    /// </summary>
    public partial class WizardSplashPage : WizardPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardSplashPage" /> class.
        /// </summary>
        public WizardSplashPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get => DescriptionLabel.Text; set => DescriptionLabel.Text = value; }

        /// <summary>
        ///     Gets or sets the splash image.
        /// </summary>
        /// <value>
        ///     The splash image.
        /// </value>
        public Image SplashImage { get => SplashPicture.Image; set => SplashPicture.Image = value; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title { get => TitleLabel.Text; set => TitleLabel.Text = value; }
    }
}