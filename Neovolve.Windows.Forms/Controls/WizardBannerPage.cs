namespace Neovolve.Windows.Forms.Controls
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    ///     The <see cref="WizardBannerPage" />
    ///     class is used to provide a banner page implementation.
    /// </summary>
    public partial class WizardBannerPage : WizardPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardBannerPage" /> class.
        /// </summary>
        public WizardBannerPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the Paint event of the BannerPanel control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.
        /// </param>
        private void BannerPanel_Paint(object sender, PaintEventArgs e)
        {
            // Paint the border
            var heightValue = BannerPanel.Height - 2;
            e.Graphics.DrawLine(SystemPens.ControlDark, 0, heightValue, BannerPanel.Width, heightValue);
            e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, heightValue + 1, BannerPanel.Width, heightValue + 1);
        }

        /// <summary>
        ///     Gets or sets the banner image.
        /// </summary>
        /// <value>
        ///     The banner image.
        /// </value>
        [Category("Banner")]
        [Description("The image displayed in the background of the banner.")]
        public Image BannerImage
        {
            get { return BannerPanel.BackgroundImage; }
            set { BannerPanel.BackgroundImage = value; }
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [Category("Banner")]
        [Description("The description displayed under the title in the banner.")]
        public string Description { get { return DescriptionLabel.Text; } set { DescriptionLabel.Text = value; } }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        [Category("Banner")]
        [Description("The title displayed in the banner.")]
        public string Title { get { return TitleLabel.Text; } set { TitleLabel.Text = value; } }
    }
}