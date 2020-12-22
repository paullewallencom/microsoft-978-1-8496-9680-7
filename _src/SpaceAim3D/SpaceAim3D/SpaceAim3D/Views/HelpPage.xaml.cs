using Microsoft.Phone.Controls;
using SpaceAim3D.ViewModels;
using System.Windows.Navigation;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the Help screen.</summary>
    public partial class HelpPage : PhoneApplicationPage
    {
        private HelpViewModel m_viewModel = new HelpViewModel();

        /// <summary>Initializes a new instance of the HelpPage class.</summary>
        public HelpPage()
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

        /// <summary>Called when the user navigates away from the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.m_viewModel.OnNavigatedFrom(e);
        }
    }
}