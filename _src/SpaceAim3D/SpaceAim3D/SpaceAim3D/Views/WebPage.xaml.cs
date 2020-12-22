using Microsoft.Phone.Controls;
using SpaceAim3D.ViewModels;
using System.Windows.Navigation;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the Web screen.</summary>
    public partial class WebPage : PhoneApplicationPage
    {
        private WebViewModel m_viewModel = new WebViewModel();

        /// <summary>Initializes a new instance of the WebPage class.</summary>
        public WebPage()
        {
            this.InitializeComponent();
            this.DataContext = this.m_viewModel;
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.FbBrowser.ClearCookiesAsync();
            this.TwBrowser.ClearCookiesAsync();
            this.m_viewModel.NavigationService = this.NavigationService;
            this.m_viewModel.OnNavigatedTo(e);
        }

        private void Fb_Navigated(object sender, NavigationEventArgs e)
        {
            this.m_viewModel.Fb_Navigated(e);
        }
    }
}