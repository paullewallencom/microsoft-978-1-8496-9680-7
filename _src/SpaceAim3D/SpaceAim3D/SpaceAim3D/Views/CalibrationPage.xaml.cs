using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the Calibration screen.</summary>
    public partial class CalibrationPage : PhoneApplicationPage
    {
        private bool m_resumeAudio = false;

        /// <summary>Initializes a new instance of the CalibrationPage class.</summary>
        public CalibrationPage()
        {
            this.InitializeComponent();
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.m_resumeAudio = false;
            if (!MediaPlayer.GameHasControl)
            {
                MediaPlayer.Pause();
                FrameworkDispatcher.Update();
                this.m_resumeAudio = true;
            }
        }

        /// <summary>Called when the user navigates away from the page.</summary>
        /// <param name="e">Additional arguments.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (this.m_resumeAudio)
            {
                MediaPlayer.Resume();
            }
            else
            {
                (Application.Current as App).StartBackgroundMusic();
            }
        }

        private void Me_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Play();
        }
    }
}