using Microsoft.Phone.Controls;
using SpaceAim3D.Models;
using SpaceAim3D.ViewModels;
using SpaceAim3DComp;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Navigation;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the Game screen.</summary>
    public partial class GamePage : PhoneApplicationPage
    {
        private Direct3DInterop m_d3dInterop = null;
        private GameViewModel m_viewModel = new GameViewModel();

        /// <summary>Initializes a new instance of the GamePage class.</summary>
        public GamePage()
        {
            this.InitializeComponent();
            this.DataContext = this.m_viewModel;
            this.BackKeyPress += this.GamePage_BackKeyPress;
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

        private void GamePage_BackKeyPress(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.m_d3dInterop.OnBackButtonPressed();
        }

        private void DrawingSurface_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.m_d3dInterop == null)
            {
                this.m_d3dInterop = new Direct3DInterop();

                // Set window bounds in dips
                this.m_d3dInterop.WindowBounds = new Windows.Foundation.Size(
                    (float)this.DrawingSurface.ActualWidth,
                    (float)this.DrawingSurface.ActualHeight);

                // Set native resolution in pixels
                this.m_d3dInterop.NativeResolution = new Windows.Foundation.Size(
                    (float)Math.Floor(this.DrawingSurface.ActualWidth * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f),
                    (float)Math.Floor(this.DrawingSurface.ActualHeight * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f));

                // Set render resolution to the full native resolution
                this.m_d3dInterop.RenderResolution = this.m_d3dInterop.NativeResolution;

                // Hook-up native component to DrawingSurface
                this.DrawingSurface.SetContentProvider(this.m_d3dInterop.CreateContentProvider());
                this.DrawingSurface.SetManipulationHandler(this.m_d3dInterop);

                // Set current language
                this.m_d3dInterop.LanguageCode = CultureInfo.CurrentUICulture.Name;

                // Events
                this.m_d3dInterop.ExitGame += this.Interop_ExitGame;
                this.m_d3dInterop.SendResult += this.Interop_SendResult;
                this.m_d3dInterop.SaveResult += this.Interop_SaveResult;

                // Enable or disable vibrations
                this.m_d3dInterop.EnableVibrations(Settings.Vibrations);
            }
        }

        private void Interop_ExitGame()
        {
            Dispatcher.BeginInvoke(() => NavigationService.GoBack());
        }

        private void Interop_SendResult()
        {
            this.m_viewModel.SendResult(this.m_d3dInterop.LastScore);
        }

        private void Interop_SaveResult()
        {
            this.m_viewModel.SaveResult(this.m_d3dInterop.LastScore);
        }
    }
}