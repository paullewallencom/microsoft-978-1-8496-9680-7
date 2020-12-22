using Microsoft.Devices.Sensors;
using SpaceAim3D.Models;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the Help screen.</summary>
    public class HelpViewModel : ViewModel
    {
        private Accelerometer m_accelerometer;

        #region Properties with supporting private fields
        private SolidColorBrush m_brushUp = new SolidColorBrush(Colors.Transparent);

        /// <summary>Gets or sets a brush for the "up" arrow.</summary>
        public SolidColorBrush BrushUp
        {
            get
            {
                return this.m_brushUp;
            }

            set
            {
                this.m_brushUp = value;
                this.OnPropertyChanged("BrushUp");
            }
        }

        private SolidColorBrush m_brushDown = new SolidColorBrush(Colors.Transparent);

        /// <summary>Gets or sets a brush for the "down" arrow.</summary>
        public SolidColorBrush BrushDown
        {
            get
            {
                return this.m_brushDown;
            }

            set
            {
                this.m_brushDown = value;
                this.OnPropertyChanged("BrushDown");
            }
        }

        private SolidColorBrush m_brushLeft = new SolidColorBrush(Colors.Transparent);

        /// <summary>Gets or sets a brush for the "left" arrow.</summary>
        public SolidColorBrush BrushLeft
        {
            get
            {
                return this.m_brushLeft;
            }

            set
            {
                this.m_brushLeft = value;
                this.OnPropertyChanged("BrushLeft");
            }
        }

        private SolidColorBrush m_brushRight = new SolidColorBrush(Colors.Transparent);

        /// <summary>Gets or sets a brush for the "right" arrow.</summary>
        public SolidColorBrush BrushRight
        {
            get
            {
                return this.m_brushRight;
            }

            set
            {
                this.m_brushRight = value;
                this.OnPropertyChanged("BrushRight");
            }
        }
        #endregion

        #region Commands
        /// <summary>Gets or sets a command, which opens the project website.</summary>
        public ICommand CmdWebsite { get; set; }

        /// <summary>Gets or sets a command, which opens the project profile at Facebook.</summary>
        public ICommand CmdProfile { get; set; }

        /// <summary>Gets or sets a command, which allows to send an e-mail message to the author.</summary>
        public ICommand CmdEmail { get; set; }
        #endregion

        /// <summary>Initializes a new instance of the HelpViewModel class.</summary>
        public HelpViewModel()
        {
            this.CmdWebsite = new Command(() => this.OpenProjectWebsite());
            this.CmdProfile = new Command(() => this.OpenFacebookProfile());
            this.CmdEmail = new Command(() => this.SendMessage());
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Accelerometer.IsSupported)
            {
                try
                {
                    this.m_accelerometer = new Accelerometer();
                    this.m_accelerometer.CurrentValueChanged += this.Accelerometer_CurrentValueChanged;
                    this.m_accelerometer.Start();
                }
                catch (Exception)
                {
                    this.m_accelerometer = null;
                }
            }
        }

        /// <summary>Called when the user navigates away from the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (this.m_accelerometer != null)
            {
                this.m_accelerometer.Stop();
                this.m_accelerometer = null;
            }
        }

        private void OpenProjectWebsite()
        {
            GameHelpers.OpenWebsite(GameHelpers.PROJECT_WEBSITE);
        }

        private void OpenFacebookProfile()
        {
            GameHelpers.OpenWebsite(GameHelpers.FACEBOOK_PROFILE);
        }

        private void SendMessage()
        {
            GameHelpers.SendEmail(GameHelpers.AUTHOR_EMAIL, "[Space Aim 3D] ...");
        }

        private void Accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            float x = e.SensorReading.Acceleration.X;
            float y = e.SensorReading.Acceleration.Y;
            float z = e.SensorReading.Acceleration.Z;

            Color mainColor = new Color() { R = 175, G = 40, B = 0 };
            Color upColor = mainColor;
            Color downColor = mainColor;
            Color leftColor = mainColor;
            Color rightColor = mainColor;
            upColor.A = (byte)(255 * Math.Max(x, 0));
            downColor.A = (byte)(255 * Math.Abs(Math.Min(x, 0)));
            leftColor.A = (byte)(255 * Math.Max(y, 0));
            rightColor.A = (byte)(255 * Math.Abs(Math.Min(y, 0)));

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.BrushUp = new SolidColorBrush(upColor);
                this.BrushDown = new SolidColorBrush(downColor);
                this.BrushLeft = new SolidColorBrush(leftColor);
                this.BrushRight = new SolidColorBrush(rightColor);
            });
        }
    }
}
