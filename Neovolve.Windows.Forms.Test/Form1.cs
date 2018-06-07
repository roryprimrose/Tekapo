using System;
using Neovolve.Windows.Forms.Controls;

namespace Neovolve.Windows.Forms.Test
{
    /// <summary>
    /// The <see cref="Form1"/>
    /// class is used to test the wizard form framework.
    /// </summary>
    public partial class Form1 : WizardForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            CreateWizard();
        }

        /// <summary>
        /// Create wizard.
        /// </summary>
        private void CreateWizard()
        {
            WizardSplashPage splash = new WizardSplashPage();
            splash.Title = "A great title";
            splash.Description = "A description that would\nbe value for something this app does.";

            // splash.SplashImage = Image.FromFile("C:\\TestA.png");
            WizardBannerPage banner = new WizardBannerPage();
            banner.Title = "Some title";
            banner.Description =
                "Some description that tells you about this page\nwith a lot more information to display.";

            // banner.BannerImage = Image.FromFile("C:\\TestB.png");
            WizardBannerPage banner2 = new WizardBannerPage();
            banner2.Title = "Banner 2";

            WizardBannerPage banner3 = new WizardBannerPage();
            banner3.Title = "Banner 3";
            
            Pages.Add("Page 1", new WizardPage());
            Pages.Add("Test banner", new TestBannerPage());
            Pages.Add("TabTest", new TabIndexPage());
            Pages.Add("Banner2", banner2);
            Pages.Add("Timer page", new TimerPage());
            Pages.Add("Banner3", banner3, null, new WizardPageNavigationSettings(String.Empty, "Banner2"));
            Pages.Add(
                "Thread test",
                new ThreadedNavigationPage(),
                new WizardPageSettings(new WizardButtonSettings("Next", false)));
            Pages.Add("Page 2", splash);
            Pages.Add(
                "Page 3",
                banner,
                new WizardPageSettings(null, null, null, null, new WizardButtonSettings("Skip")),
                new WizardPageNavigationSettings(String.Empty, String.Empty, String.Empty, String.Empty, "Page 5"));
            Pages.Add("Custom navigation", new InvokeKeyPage());
            Pages.Add(
                "Page 5",
                new WizardPage(),
                new WizardPageSettings(new WizardButtonSettings("Finish")),
                new WizardPageNavigationSettings(String.Empty, String.Empty, "Page 2"));
        }
    }
}