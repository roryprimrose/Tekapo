namespace Neovolve.Windows.Forms.Test
{
    using System.Diagnostics.CodeAnalysis;
    using Neovolve.Windows.Forms.Controls;

    /// <summary>
    ///     The <see cref="Form1" />
    ///     class is used to test the wizard form framework.
    /// </summary>
    public partial class Form1 : WizardForm
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Form1" /> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            CreateWizard();
        }

        /// <summary>
        ///     Create wizard.
        /// </summary>
        [SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Pages are disposed when the form is disposed.")]
        private void CreateWizard()
        {
            var splash = new WizardSplashPage
            {
                Title = "A great title",
                Description = "A description that would\nbe value for something this app does."
            };

            // splash.SplashImage = Image.FromFile("C:\\TestA.png");
            var banner = new WizardBannerPage
            {
                Title = "Some title",
                Description =
                    "Some description that tells you about this page\nwith a lot more information to display."
            };

            // banner.BannerImage = Image.FromFile("C:\\TestB.png");
            var banner2 = new WizardBannerPage {Title = "Banner 2"};

            var banner3 = new WizardBannerPage {Title = "Banner 3"};

            Pages.Add("Page 1", new WizardPage());
            Pages.Add("Test banner", new TestBannerPage());
            Pages.Add("TabTest", new TabIndexPage());
            Pages.Add("Banner2", banner2);
            Pages.Add("Timer page", new TimerPage());
            Pages.Add("Banner3", banner3, null, new WizardPageNavigationSettings(string.Empty, "Banner2"));
            Pages.Add("Thread test",
                new ThreadedNavigationPage(),
                new WizardPageSettings(new WizardButtonSettings("Next", false)));
            Pages.Add("Page 2", splash);
            Pages.Add("Page 3",
                banner,
                new WizardPageSettings(null, null, null, null, new WizardButtonSettings("Skip")),
                new WizardPageNavigationSettings(string.Empty, string.Empty, string.Empty, string.Empty, "Page 5"));
            Pages.Add("Custom navigation", new InvokeKeyPage());
            Pages.Add("Page 5",
                new WizardPage(),
                new WizardPageSettings(new WizardButtonSettings("Finish")),
                new WizardPageNavigationSettings(string.Empty, string.Empty, "Page 2"));
        }
    }
}