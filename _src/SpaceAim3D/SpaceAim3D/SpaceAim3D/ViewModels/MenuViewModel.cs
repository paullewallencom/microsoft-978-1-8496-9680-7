using SpaceAim3D.Models;
using SpaceAim3D.Resources;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.Phone.Devices.Notification;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the Menu screen.</summary>
    public class MenuViewModel : ViewModel
    {
        private Dictionary<string, string> m_urls = new Dictionary<string, string>();
        private VibrationDevice m_vibration = VibrationDevice.GetDefault();

        /// <summary>Initializes a new instance of the MenuViewModel class.</summary>
        public MenuViewModel()
        {
            this.m_urls["play"] = "/Views/GamePage.xaml";
            this.m_urls["ranks"] = "/Views/RanksPage.xaml";
            this.m_urls["map"] = "/Views/MapPage.xaml";
            this.m_urls["world"] = "/Views/WorldPage.xaml";
            this.m_urls["settings"] = "/Views/SettingsPage.xaml";
            this.m_urls["help"] = "/Views/HelpPage.xaml";
            this.m_urls["web"] = "/Views/WebPage.xaml";
        }

        /// <summary>Navigates to a page, which name is given as the parameter.</summary>
        /// <param name="key">A name (key) of the page.</param>
        public void NavigateToScreen(string key)
        {
            Uri pageUri = new Uri(this.m_urls[key], UriKind.Relative);
            this.NavigationService.Navigate(pageUri);
            if (Settings.Vibrations)
            {
                this.m_vibration.Vibrate(TimeSpan.FromMilliseconds(100));
            }
        }

        /// <summary>Navigates to another page, using the speech recognition mechanism.
        /// While navigating, an additional message is synthesized with the text-to-speech mechanism.</summary>
        public async void NavigateByVoiceAsync()
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            SpeechRecognizerUI recognizer = new SpeechRecognizerUI();
            recognizer.Settings.ListenText = AppResources.MenuSpeechQuestion;
            recognizer.Settings.ExampleText = AppResources.MenuSpeechAnswerExample;
            recognizer.Settings.ReadoutEnabled = false;
            recognizer.Settings.ShowConfirmation = false;
            recognizer.Recognizer.Grammars.AddGrammarFromList("answers", this.m_urls.Keys);
            string answer = string.Empty;
            try
            {
                var result = await recognizer.RecognizeWithUIAsync();
                answer = result.RecognitionResult.Text;
            }
            catch
            {
                MessageBox.Show(AppResources.MenuSpeechErrorMessage, AppResources.MenuSpeechErrorTitle, MessageBoxButton.OK);
            }

            if (!string.IsNullOrEmpty(answer))
            {
                string voiceMessage = string.Format(AppResources.MenuSpeechNavigatingMessage, answer);
                this.NavigateToScreen(answer);
                await synthesizer.SpeakTextAsync(voiceMessage);
            }
        }
    }
}
