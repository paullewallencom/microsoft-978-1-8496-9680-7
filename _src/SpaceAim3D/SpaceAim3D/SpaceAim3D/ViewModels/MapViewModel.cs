using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;
using SpaceAim3D.Models;
using SpaceAim3D.Resources;
using SpaceAim3D.WebService;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Windows.Devices.Geolocation;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the Map screen.</summary>
    public class MapViewModel : ViewModel
    {
        // Constant values regarding zooming - default, maximum, and minimum values,
        // as well as a value of the single change
        private const double ZOOM_DEFAULT = 12.0;
        private const double ZOOM_MAX = 20.0;
        private const double ZOOM_MIN = 1.0;
        private const double ZOOM_CHANGE = 1.0;

        // Default values for settings regarding landmarks 
        // and additional information for pedestrians
        private const bool LANDMARKS_DEFAULT = true;
        private const bool PEDESTRIANS_DEFAULT = false;

        // Default cartographic mode
        private const MapCartographicMode MODE_DEFAULT = MapCartographicMode.Terrain;

        // Default center location
        private const double CENTER_DEFAULT_LATITUDE = 50.04;
        private const double CENTER_DEFAULT_LONGITUDE = 22.00;

        // Map application ID and authentication token
        private const string MAP_APPLICATION_ID = "---";
        private const string MAP_AUTHENTICATION_TOKEN = "---";

        private Geolocator m_geolocator;
        private SA3DServiceClient m_ws;

        #region Properties with supporting private fields
        private double m_zoom = ZOOM_DEFAULT;

        /// <summary>Gets or sets a zoom level for the map.</summary>
        public double Zoom
        {
            get
            {
                return this.m_zoom;
            }

            set
            {
                this.m_zoom = value;
                this.OnPropertyChanged("Zoom");
            }
        }

        private bool m_landmarks = LANDMARKS_DEFAULT;

        /// <summary>Gets or sets a value indicating whether landmarks should be shown on the map.</summary>
        public bool Landmarks
        {
            get
            {
                return this.m_landmarks;
            }

            set
            {
                this.m_landmarks = value;
                this.OnPropertyChanged("Landmarks");
            }
        }

        private bool m_pedestrians = PEDESTRIANS_DEFAULT;

        /// <summary>Gets or sets a value indicating whether additional information 
        /// for pedestrians should be presented on the map.</summary>
        public bool Pedestrians
        {
            get
            {
                return this.m_pedestrians;
            }

            set
            {
                this.m_pedestrians = value;
                this.OnPropertyChanged("Pedestrians");
            }
        }

        private MapCartographicMode m_mode = MODE_DEFAULT;

        /// <summary>Gets or sets a current cartographic mode on the map.</summary>
        public MapCartographicMode Mode
        {
            get
            {
                return this.m_mode;
            }

            set
            {
                this.m_mode = value;
                this.OnPropertyChanged("Mode");
            }
        }

        private GeoCoordinate m_center = new GeoCoordinate(CENTER_DEFAULT_LATITUDE, CENTER_DEFAULT_LONGITUDE);

        /// <summary>Gets or sets a center of the map.</summary>
        public GeoCoordinate Center
        {
            get
            {
                return this.m_center;
            }

            set
            {
                this.m_center = value;
                this.OnPropertyChanged("Center");
            }
        }

        private ObservableCollection<PlayerData> m_players = new ObservableCollection<PlayerData>();

        /// <summary>Gets or sets a collection of players data to show on the map.</summary>
        public ObservableCollection<PlayerData> Players
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

        private GeoCoordinate m_userLocation = GeoCoordinate.Unknown;

        /// <summary>Gets or sets a current user location, which should be presented on the map.</summary>
        public GeoCoordinate UserLocation
        {
            get
            {
                return this.m_userLocation;
            }

            set
            {
                this.m_userLocation = value;
                this.OnPropertyChanged("UserLocation");
            }
        }

        private string m_geolocatorStatus = AppResources.GeolocatorInitializing;

        /// <summary>Gets or sets an information regarding the status of the geolocation.</summary>
        public string GeolocatorStatus
        {
            get
            {
                return this.m_geolocatorStatus;
            }

            set
            {
                this.m_geolocatorStatus = value;
                this.OnPropertyChanged("GeolocatorStatus");
            }
        }

        private string m_downloadStatus = string.Empty;

        /// <summary>Gets or sets an information regarding the status of downloading data from the web service.</summary>
        public string DownloadStatus
        {
            get
            {
                return this.m_downloadStatus;
            }

            set
            {
                this.m_downloadStatus = value;
                this.OnPropertyChanged("DownloadStatus");
            }
        }
        #endregion

        #region Commands
        /// <summary>Gets or sets a command, which zooms the map in.</summary>
        public ICommand CmdZoomIn { get; set; }

        /// <summary>Gets or sets a command, which zooms the map out.</summary>
        public ICommand CmdZoomOut { get; set; }

        /// <summary>Gets or sets a command, which toggles the setting 
        /// of presenting landmarks on the map.</summary>
        public ICommand CmdLandmarks { get; set; }

        /// <summary>Gets or sets a command, which toggles the setting 
        /// of presenting additional information for pedestrians on the map.</summary>
        public ICommand CmdPedestrians { get; set; }

        /// <summary>Gets or sets a command, which changes the cartographic mode 
        /// of the map in a cyclic way.</summary>
        public ICommand CmdMode { get; set; }
        #endregion

        /// <summary>Initializes a new instance of the MapViewModel class.</summary>
        public MapViewModel()
        {
            this.CmdZoomIn = new Command(this.ZoomIn);
            this.CmdZoomOut = new Command(this.ZoomOut);
            this.CmdLandmarks = new Command(this.ToggleLandmarks);
            this.CmdPedestrians = new Command(this.TogglePedestrians);
            this.CmdMode = new Command(this.ChangeMode);
        }

        /// <summary>Sets credentials for the map services.</summary>
        public void SetMapCredentials()
        {
            MapsSettings.ApplicationContext.ApplicationId = MAP_APPLICATION_ID;
            MapsSettings.ApplicationContext.AuthenticationToken = MAP_AUTHENTICATION_TOKEN;
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            // Create the web service client
            this.m_ws = new SA3DServiceClient();
            this.m_ws.GetPlayersCompleted += this.Ws_GetPlayersCompleted;

            // Initialize geolocator
            this.m_geolocator = new Geolocator();
            this.m_geolocator.DesiredAccuracy = PositionAccuracy.High;
            this.m_geolocator.MovementThreshold = 1;
            this.m_geolocator.StatusChanged += this.Geolocator_StatusChanged;
            this.m_geolocator.PositionChanged += this.Geolocator_PositionChanged;

            // Download players data
            this.DownloadStatus = AppResources.MapDownloadingData;
            this.m_ws.GetPlayersAsync(Settings.Key);
        }

        /// <summary>Starts navigation to the player, whose data are given as the parameter.</summary>
        /// <param name="player">Data of the player.</param>
        public void NavigateToPlayer(PlayerData player)
        {
            GameHelpers.NavigateToPlayer(player.Name, player.Location);
        }

        private void Geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                string status = this.ResolveGeolocatorStatus(args.Status);
                this.GeolocatorStatus = status;
            });
        }

        private void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                GeoCoordinate location = new GeoCoordinate(
                    args.Position.Coordinate.Latitude,
                    args.Position.Coordinate.Longitude);
                if (this.UserLocation == GeoCoordinate.Unknown)
                {
                    this.Center = location;
                    if (Settings.Location)
                    {
                        this.m_ws.SendLocationAsync(
                            Settings.Key,
                            Settings.Name,
                            (float)location.Latitude,
                            (float)location.Longitude);
                    }
                    this.m_ws.CloseAsync();
                }

                this.UserLocation = location;
            });
        }

        private void Ws_GetPlayersCompleted(object sender, GetPlayersCompletedEventArgs e)
        {
            this.Players.Clear();
            this.DownloadStatus = string.Empty;
            if (e.Error != null)
            {
                this.DownloadStatus = AppResources.MapCannotDownloadData;
                return;
            }

            foreach (SA3DPlayer player in e.Result)
            {
                PlayerData playerData = new PlayerData()
                {
                    Name = player.Name,
                    Location = new GeoCoordinate(player.LocationLatitude, player.LocationLongitude)
                };
                this.Players.Add(playerData);
            }
        }

        private void ZoomIn()
        {
            if (this.Zoom < ZOOM_MAX)
            {
                this.Zoom += ZOOM_CHANGE;
            }
        }

        private void ZoomOut()
        {
            if (this.Zoom > ZOOM_MIN)
            {
                this.Zoom -= ZOOM_CHANGE;
            }
        }

        private void ChangeMode()
        {
            switch (this.Mode)
            {
                case MapCartographicMode.Aerial:
                    this.Mode = MapCartographicMode.Hybrid;
                    break;
                case MapCartographicMode.Hybrid:
                    this.Mode = MapCartographicMode.Road;
                    break;
                case MapCartographicMode.Road:
                    this.Mode = MapCartographicMode.Terrain;
                    break;
                case MapCartographicMode.Terrain:
                    this.Mode = MapCartographicMode.Aerial;
                    break;
            }
        }

        private void ToggleLandmarks()
        {
            this.Landmarks = !this.Landmarks;
        }

        private void TogglePedestrians()
        {
            this.Pedestrians = !this.Pedestrians;
        }

        private string ResolveGeolocatorStatus(PositionStatus status)
        {
            switch (status)
            {
                case PositionStatus.NotAvailable:
                    return AppResources.GeolocatorNotAvailable;
                case PositionStatus.Disabled:
                    return AppResources.GeolocatorDisabled;
                case PositionStatus.Initializing:
                    return AppResources.GeolocatorInitializing;
                case PositionStatus.NoData:
                    return AppResources.GeolocatorNoData;
                default:
                    return string.Empty;
            }
        }
    }
}
