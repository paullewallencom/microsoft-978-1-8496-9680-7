using SpaceAim3D.Models;
using SpaceAim3D.Resources;
using SpaceAim3D.WebService;
using System.Linq;
using System.Windows.Navigation;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the Ranks screen.</summary>
    public class RanksViewModel : ViewModel
    {
        #region Properties with supporting private fields
        private RankItem[] m_rankLocal = null;

        /// <summary>Gets or sets a collection of items for the local rank.</summary>
        public RankItem[] RankLocal
        {
            get
            {
                return this.m_rankLocal;
            }

            set
            {
                this.m_rankLocal = value;
                this.OnPropertyChanged("RankLocal");
            }
        }

        private RankItem[] m_rankLast24h = null;

        /// <summary>Gets or sets a collection of items for the rank from the last 24 hours.</summary>
        public RankItem[] RankLast24h
        {
            get
            {
                return this.m_rankLast24h;
            }

            set
            {
                this.m_rankLast24h = value;
                this.OnPropertyChanged("RankLast24h");
            }
        }

        private RankItem[] m_rankOverall = null;

        /// <summary>Gets or sets a collection of items for the overall rank.</summary>
        public RankItem[] RankOverall
        {
            get
            {
                return this.m_rankOverall;
            }

            set
            {
                this.m_rankOverall = value;
                this.OnPropertyChanged("RankOverall");
            }
        }

        private string m_downloadStatus = string.Empty;

        /// <summary>Gets or sets a status of downloading data from the web service.</summary>
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

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DownloadStatus = AppResources.RanksDownloadingResults;
            SA3DServiceClient ws = new SA3DServiceClient();
            ws.GetResultsCompleted += this.Ws_GetResultsCompleted;
            ws.GetResultsAsync();
            ws.CloseAsync();

            LocalRank localRank = new LocalRank();
            this.RankLocal = localRank.ReadTopScores();
        }

        private void Ws_GetResultsCompleted(object sender, GetResultsCompletedEventArgs e)
        {
            this.DownloadStatus = string.Empty;
            if (e.Error != null)
            {
                this.DownloadStatus = AppResources.RanksCannotDownloadResults;
                return;
            }

            this.RankLast24h = this.GetRankItems(e.Result.Last24Hours.ToArray());
            this.RankOverall = this.GetRankItems(e.Result.Overall.ToArray());
        }

        private RankItem[] GetRankItems(SA3DRankItem[] items)
        {
            RankItem[] rankItems = new RankItem[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                rankItems[i] = new RankItem(i + 1, items[i].Name, items[i].Score);
            }

            return rankItems;
        }
    }
}
