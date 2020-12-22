using Microsoft.Phone.Controls;
using SpaceAim3D.ViewModels;
using System.Windows.Navigation;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the Ranks screen.</summary>
    public partial class RanksPage : PhoneApplicationPage
    {
        private RanksViewModel m_viewModel = new RanksViewModel();

        /// <summary>Initializes a new instance of the RanksPage class.</summary>
        public RanksPage()
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
            this.m_viewModel.OnNavigatedTo(e);
        }
    }
}