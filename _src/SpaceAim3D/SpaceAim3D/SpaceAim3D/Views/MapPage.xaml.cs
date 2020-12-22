using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Toolkit;
using SpaceAim3D.Models;
using SpaceAim3D.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the Map screen.</summary>
    public partial class MapPage : PhoneApplicationPage
    {
        private MapViewModel m_viewModel = new MapViewModel();

        /// <summary>Initializes a new instance of the MapPage class.</summary>
        public MapPage()
        {
            this.InitializeComponent();
            this.DataContext = this.m_viewModel;
            MapItemsControl items = MapExtensions.GetChildren(this.MapPlayers)[0] as MapItemsControl;
            items.ItemsSource = this.m_viewModel.Players;
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.m_viewModel.NavigationService = this.NavigationService;
            this.m_viewModel.OnNavigatedTo(e);
        }

        private void BtnNavigate_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            PlayerData player = (PlayerData)button.Tag;
            this.m_viewModel.NavigateToPlayer(player);
        }

        private void MapPlayers_Loaded(object sender, RoutedEventArgs e)
        {
            this.m_viewModel.SetMapCredentials();
        }
    }
}