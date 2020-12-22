using Microsoft.Phone.Shell;
using SpaceAim3D.Models;
using SpaceAim3D.WebService;
using System.Windows.Navigation;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the Game screen.</summary>
    public class GameViewModel : ViewModel
    {
        private PhoneApplicationService m_phoneApplicationService = PhoneApplicationService.Current;

        /// <summary>Sends the current result to the web service.</summary>
        /// <param name="result">Current result.</param>
        public void SendResult(int result)
        {
            SA3DServiceClient ws = new SA3DServiceClient();
            ws.SendResultAsync(Settings.Key, Settings.Name, result);
            ws.CloseAsync();
        }

        /// <summary>Saves the current result in the local rank.</summary>
        /// <param name="result">Current result.</param>
        public void SaveResult(int result)
        {
            LocalRank localRank = new LocalRank();
            localRank.AddResult(Settings.Name, result);
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            this.m_phoneApplicationService.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        /// <summary>Called when the user navigates away from the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.m_phoneApplicationService.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }
    }
}
