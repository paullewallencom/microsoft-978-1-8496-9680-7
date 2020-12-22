using GART;
using GART.BaseControls;
using GART.Controls;
using GART.Data;
using Microsoft.Devices.Sensors;
using SpaceAim3D.Models;
using SpaceAim3D.Resources;
using SpaceAim3D.WebService;
using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Windows;
using System.Windows.Navigation;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the World screen.</summary>
    public class WorldViewModel : ViewModel
    {
        #region Fields and properties
        private Compass m_compass;
        private ObservableCollection<ARItem> m_players = new ObservableCollection<ARItem>();

        /// <summary>Gets or sets a collection of players data to present in the augmented reality.</summary>
        public ObservableCollection<ARItem> Players
        {
            get
            {
                return this.m_players;
            }

            set
            {
                this.m_players = value;
                this.OnPropertyChanged("Players");
            }
        }

        private string m_errorMessage = string.Empty;

        /// <summary>Gets or sets an error message related to the augmented reality feature.</summary>
        public string ErrorMessage
        {
            get
            {
                return this.m_errorMessage;
            }

            set
            {
                this.m_errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }

        /// <summary>Gets or sets a reference to the ARDisplay control from the user interface.</summary>
        public ARDisplay Display { get; set; }
        #endregion

        /// <summary>Initializes a new instance of the WorldViewModel class.</summary>
        public WorldViewModel()
        {
            this.Players = new ObservableCollection<ARItem>();
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Compass.IsSupported && e.NavigationMode != NavigationMode.Back)
            {
                this.m_compass = new Compass();
                this.m_compass.Start();
                this.m_compass.Calibrate += this.Compass_Calibrate;
                this.m_compass.CurrentValueChanged += this.Compass_CurrentValueChanged;
            }
            else
            {
                this.StartAugmentedReality();
            }
        }

        /// <summary>Called when the user navigates away from the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.Display.ServiceErrors -= this.Display_ServiceErrors;
            if (this.Display.Motion != null)
            {
                this.Display.StopServices();
            }
        }

        /// <summary>Starts navigation to the player, whose data are given as the parameter.</summary>
        /// <param name="player">Data of the player.</param>
        public void NavigateToPlayer(PlayerData player)
        {
            GameHelpers.NavigateToPlayer(player.Name, player.Location);
        }

        #region Private methods
        private void Compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            this.StopCompass();
            Deployment.Current.Dispatcher.BeginInvoke(() => this.StartAugmentedReality());
        }

        private void Compass_Calibrate(object sender, CalibrationEventArgs e)
        {
            this.StopCompass();
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.NavigationService.Navigate(new Uri("/Views/CalibrationPage.xaml", UriKind.Relative));
            });
        }

        private void StartAugmentedReality()
        {
            this.Display.ServiceErrors += this.Display_ServiceErrors;
            this.Display.StartServices();
            this.Display.Orientation = ControlOrientation.Clockwise270Degrees;

            SA3DServiceClient ws = new SA3DServiceClient();
            ws.GetPlayersCompleted += this.Ws_GetPlayersCompleted;
            ws.GetPlayersAsync(Settings.Key);
            ws.CloseAsync();
        }

        private void StopCompass()
        {
            this.m_compass.Stop();
            this.m_compass = null;
        }

        private void Display_ServiceErrors(object sender, ServiceErrorsEventArgs e)
        {
            this.Players.Clear();
            this.ErrorMessage = AppResources.WorldCannotStartAR;
        }

        private void Ws_GetPlayersCompleted(object sender, GetPlayersCompletedEventArgs e)
        {
            this.Players.Clear();
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                return;
            }
            else if (e.Error != null)
            {
                this.ErrorMessage = AppResources.WorldCannotDownloadData;
                return;
            }

            foreach (SA3DPlayer player in e.Result)
            {
                var playerData = new ARItemExtended()
                {
                    Content = player.Name,
                    GeoLocation = new GeoCoordinate(player.LocationLatitude, player.LocationLongitude)
                };
                this.Players.Add(playerData);
            }
        }
        #endregion
    }
}
