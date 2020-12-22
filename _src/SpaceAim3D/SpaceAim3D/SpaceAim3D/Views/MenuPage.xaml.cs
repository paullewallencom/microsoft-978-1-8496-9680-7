using Microsoft.Phone.Controls;
using SpaceAim3D.ViewModels;
using System.Windows.Controls;
using System.Windows.Navigation;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the Menu screen.</summary>
    public partial class MenuPage : PhoneApplicationPage
    {
        private MenuViewModel m_viewModel = new MenuViewModel();

        /// <summary>Initializes a new instance of the MenuPage class.</summary>
        public MenuPage()
        {
            this.InitializeComponent();
            this.DataContext = this.m_viewModel;
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.m_viewModel.NavigationService = this.NavigationService;
        }

        private void BrdPage_Tap(object sender, GestureEventArgs e)
        {
            string page = ((Border)sender).Tag as string;
            this.m_viewModel.NavigateToScreen(page);
        }

        private void BrdVoice_Tap(object sender, GestureEventArgs e)
        {
            this.m_viewModel.NavigateByVoiceAsync();
        }
    }
}