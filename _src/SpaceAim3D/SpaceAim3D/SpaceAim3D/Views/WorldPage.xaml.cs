using Microsoft.Phone.Controls;
using SpaceAim3D.Models;
using SpaceAim3D.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the World screen.</summary>
    public partial class WorldPage : PhoneApplicationPage
    {
        private WorldViewModel m_viewModel = new WorldViewModel();

        /// <summary>Initializes a new instance of the WorldPage class.</summary>
        public WorldPage()
        {
            this.InitializeComponent();
            this.DataContext = this.m_viewModel;
            this.m_viewModel.Display = this.Display;
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.m_viewModel.NavigationService = this.NavigationService;
            this.m_viewModel.OnNavigatedTo(e);
        }

        /// <summary>Called when the user navigates away from the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.m_viewModel.OnNavigatedFrom(e);
        }

        private void BtnNavigate_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            PlayerData player = (PlayerData)button.Tag;
            this.m_viewModel.NavigateToPlayer(player);
        }
    }
}