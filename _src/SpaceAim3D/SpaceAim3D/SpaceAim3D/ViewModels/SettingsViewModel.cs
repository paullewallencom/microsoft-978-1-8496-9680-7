using SpaceAim3D.Models;
using SpaceAim3D.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the Settings screen.</summary>
    public class SettingsViewModel : ViewModel
    {
        /// <summary>A regular expression to check the player name.</summary>
        private const string NAME_REGEX = "^[a-zA-Z0-9_-]{3,10}$";

        #region Properties with supporting private fields
        private string m_name;

        /// <summary>Gets or sets a name of the player.</summary>
        public string Name
        {
            get
            {
                return this.m_name;
            }

            set
            {
                this.m_name = value;
                this.OnPropertyChanged("Name");
            }
        }

        private float m_volume;

        /// <summary>Gets or sets a music volume.</summary>
        public float Volume
        {
            get
            {
                return this.m_volume;
            }

            set
            {
                this.m_volume = value;
                this.OnPropertyChanged("Volume");
            }
        }

        private bool m_vibrations;

        /// <summary>Gets or sets a value indicating whether vibrations should be used.</summary>
        public bool Vibrations
        {
            get
            {
                return this.m_vibrations;
            }

            set
            {
                this.m_vibrations = value;
                this.OnPropertyChanged("Vibrations");
            }
        }

        private bool m_useLocation;

        /// <summary>Gets or sets a value indicating whether the current location 
        /// should be automatically sent to the web service.</summary>
        public bool UseLocation
        {
            get
            {
                return this.m_useLocation;
            }

            set
            {
                this.m_useLocation = value;
                this.OnPropertyChanged("UseLocation");
            }
        }
        #endregion

        /// <summary>Gets or sets a command, which opens a website with the privacy policy.</summary>
        public ICommand CmdPrivacyPolicy { get; set; }

        /// <summary>Initializes a new instance of the SettingsViewModel class.</summary>
        public SettingsViewModel()
        {
            this.CmdPrivacyPolicy = new Command(() => this.OpenPrivacyPolicy());
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Name = Settings.Name;
            this.Volume = Settings.Volume;
            this.Vibrations = Settings.Vibrations;
            this.UseLocation = Settings.Location;
        }

        /// <summary>Called when the user navigates away from the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Regex.IsMatch(this.Name, NAME_REGEX))
            {
                Settings.Name = this.Name;
            }
            else
            {
                MessageBox.Show(
                    AppResources.SettingsNameIncorrectMessage,
                    AppResources.SettingsNameIncorrectTitle,
                    MessageBoxButton.OK);
            }

            Settings.Volume = this.Volume;
            Settings.Vibrations = this.Vibrations;
            Settings.Location = this.UseLocation;

            (App.Current as App).UpdateMusicVolume();
        }

        private void OpenPrivacyPolicy()
        {
            GameHelpers.OpenWebsite(GameHelpers.PROJECT_WEBSITE);
        }
    }
}
