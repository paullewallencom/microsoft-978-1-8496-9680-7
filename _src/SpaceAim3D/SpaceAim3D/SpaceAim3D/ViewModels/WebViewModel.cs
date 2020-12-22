using Facebook;
using LinqToTwitter;
using Microsoft.Phone.Tasks;
using SpaceAim3D.Models;
using SpaceAim3D.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The view model class regarding the Web screen.</summary>
    public class WebViewModel : ViewModel
    {
        private const string FB_APP_ID = "---";
        private const string FB_PERMISSIONS = "publish_stream";
        private const string FB_REDIRECT_URI = GameHelpers.PROJECT_WEBSITE;
        private const string FB_RESPONSE_TYPE = "token";
        private const string FB_DISPLAY = "touch";
        private FacebookClient m_fbClient = new FacebookClient();

        private const string TW_CONSUMERKEY = "---";
        private const string TW_CONSUMERSECRET = "---";
        private const string TW_PIN_REGEX = @"^\d{7}$";
        private PinAuthorizer m_twPinAuthorizer = null;

        private const string FEED_URL = GameHelpers.RSS_FEED;

        #region Commands
        /// <summary>Gets or sets a command, which publishes a message 
        /// at the wall of the currently logged in Facebook user.</summary>
        public ICommand CmdFacebook { get; set; }

        /// <summary>Gets or sets a command, which updates a status
        /// of the currently logged in Twitter user.</summary>
        public ICommand CmdTwitter { get; set; }

        /// <summary>Gets or sets a command, which opens the review page,
        /// where the player can rate the game.</summary>
        public ICommand CmdRate { get; set; }

        /// <summary>Gets or sets a command, which opens the project website.</summary>
        public ICommand CmdWebsite { get; set; }

        /// <summary>Gets or sets a command, which confirms the PIN 
        /// entered by the player while authorizing the Twitter application.</summary>
        public ICommand CmdTwPINGo { get; set; }
        #endregion

        #region Properties with supporting private fields
        private Visibility m_fbVisibility = Visibility.Collapsed;

        /// <summary>Gets or sets a visibility of the UI part related to the Facebook support.</summary>
        public Visibility FbVisibility
        {
            get
            {
                return this.m_fbVisibility;
            }

            set
            {
                this.m_fbVisibility = value;
                this.OnPropertyChanged("FbVisibility");
            }
        }

        private Uri m_fbUri = null;

        /// <summary>Gets or sets an address of the website, which should 
        /// be presented in the web browser related to the Facebook support.</summary>
        public Uri FbUri
        {
            get
            {
                return this.m_fbUri;
            }

            set
            {
                this.m_fbUri = value;
                this.OnPropertyChanged("FbUri");
            }
        }

        private Visibility m_twVisibility = Visibility.Collapsed;

        /// <summary>Gets or sets a visibility of the UI part related to the Twitter support.</summary>
        public Visibility TwVisibility
        {
            get
            {
                return this.m_twVisibility;
            }

            set
            {
                this.m_twVisibility = value;
                this.OnPropertyChanged("TwVisibility");
            }
        }

        private Uri m_twUri = null;

        /// <summary>Gets or sets an address of the website, which should 
        /// be presented in the web browser related to the Twitter support.</summary>
        public Uri TwUri
        {
            get
            {
                return this.m_twUri;
            }

            set
            {
                this.m_twUri = value;
                this.OnPropertyChanged("TwUri");
            }
        }

        private string m_twPIN = string.Empty;

        /// <summary>Gets or sets a PIN related to the Twitter support.</summary>
        public string TwPIN
        {
            get
            {
                return this.m_twPIN;
            }

            set
            {
                this.m_twPIN = value;
                this.OnPropertyChanged("TwPIN");
            }
        }

        private News m_latestNews = null;

        /// <summary>Gets or sets the latest news read from the RSS feed.</summary>
        public News LatestNews
        {
            get
            {
                return this.m_latestNews;
            }

            set
            {
                this.m_latestNews = value;
                this.OnPropertyChanged("LatestNews");
            }
        }
        #endregion

        /// <summary>Initializes a new instance of the WebViewModel class.</summary>
        public WebViewModel()
        {
            this.CmdFacebook = new Command(() => this.PublishAtWall());
            this.CmdTwitter = new Command(() => this.Tweet());
            this.CmdRate = new Command(() => this.Rate());
            this.CmdWebsite = new Command(() => this.OpenProjectWebsite());

            this.m_fbClient.PostCompleted += this.Fb_PostCompleted;
            this.CmdTwPINGo = new Command(() => this.Tw_Authorize());
        }

        /// <summary>Called when the user navigates to the page.</summary>
        /// <param name="e">Additional arguments.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            this.FbVisibility = Visibility.Collapsed;
            this.TwVisibility = Visibility.Collapsed;
            this.ReadLatestNews();
        }

        #region Facebook support
        private void PublishAtWall()
        {
            if (this.m_fbClient.AccessToken == null)
            {
                var par = new Dictionary<string, object>();
                par["client_id"] = FB_APP_ID;
                par["redirect_uri"] = FB_REDIRECT_URI;
                par["response_type"] = FB_RESPONSE_TYPE;
                par["display"] = FB_DISPLAY;
                par["scope"] = FB_PERMISSIONS;
                this.FbUri = this.m_fbClient.GetLoginUrl(par);
                this.FbVisibility = Visibility.Visible;
            }
            else
            {
                this.Fb_PostMessage();
            }
        }

        /// <summary>Called when the web browser (related to the Facebook support) navigates to another website.</summary>
        /// <param name="e">Additional arguments.</param>
        public void Fb_Navigated(NavigationEventArgs e)
        {
            FacebookOAuthResult authResult;
            if (this.m_fbClient.TryParseOAuthCallbackUrl(e.Uri, out authResult))
            {
                this.FbVisibility = Visibility.Collapsed;
                if (authResult.IsSuccess)
                {
                    this.m_fbClient.AccessToken = authResult.AccessToken;
                    this.Fb_PostMessage();
                }
            }
        }

        private void Fb_PostMessage()
        {
            var par = new Dictionary<string, object>();
            par["message"] = AppResources.WebFbPost;
            this.m_fbClient.PostTaskAsync("me/feed", par);
        }

        private void Fb_PostCompleted(object sender, FacebookApiEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(
                    e.Error != null ? AppResources.WebFbMessageFailure : AppResources.WebFbMessageSuccess,
                    AppResources.WebFbMessageTitle,
                    MessageBoxButton.OK);
            });
        }
        #endregion

        #region Twitter support
        private void Tweet()
        {
            if (this.m_twPinAuthorizer == null || !this.m_twPinAuthorizer.IsAuthorized)
            {
                this.TwVisibility = Visibility.Visible;
                this.m_twPinAuthorizer = new PinAuthorizer();
                this.m_twPinAuthorizer.Credentials =
                   new InMemoryCredentials()
                   {
                       ConsumerKey = TW_CONSUMERKEY,
                       ConsumerSecret = TW_CONSUMERSECRET
                   };
                this.m_twPinAuthorizer.UseCompression = true;
                this.m_twPinAuthorizer.GoToTwitterAuthorization = new Action<string>(this.Tw_NavigateToLoginPage);
                this.m_twPinAuthorizer.BeginAuthorize(new Action<TwitterAsyncResponse<object>>(this.Tw_BeginAuthorized));
            }
            else
            {
                this.Tw_Tweet();
            }
        }

        private void Tw_NavigateToLoginPage(string link)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.TwUri = new Uri(link, UriKind.Absolute);
            });
        }

        private void Tw_BeginAuthorized(TwitterAsyncResponse<object> status)
        {
            switch (status.Status)
            {
                case TwitterErrorStatus.TwitterApiError:
                case TwitterErrorStatus.RequestProcessingException:
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(AppResources.WebTwError));
                    break;
            }
        }

        private void Tw_Authorize()
        {
            if (Regex.IsMatch(this.TwPIN, TW_PIN_REGEX))
            {
                this.m_twPinAuthorizer.CompleteAuthorize(this.TwPIN, new Action<TwitterAsyncResponse<UserIdentifier>>(this.Tw_AutorizationCompleted));
                this.TwVisibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show(AppResources.WebTwIncorrectPIN);
            }
        }

        private void Tw_AutorizationCompleted(TwitterAsyncResponse<UserIdentifier> result)
        {
            switch (result.Status)
            {
                case TwitterErrorStatus.Success:
                    this.Tw_Tweet();
                    break;
                default:
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(AppResources.WebTwIncorrectPIN));
                    break;
            }
        }

        private void Tw_Tweet()
        {
            TwitterContext tc = new TwitterContext(this.m_twPinAuthorizer);
            tc.UpdateStatus(AppResources.WebTwStatus, new Action<TwitterAsyncResponse<Status>>(this.Tw_TweetCompleted));
        }

        private void Tw_TweetCompleted(TwitterAsyncResponse<Status> obj)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(
                   obj.Status == TwitterErrorStatus.Success ? AppResources.WebTwMessageSuccess : AppResources.WebTwMessageFailure,
                   AppResources.WebTwMessageTitle,
                   MessageBoxButton.OK);
            });
        }
        #endregion

        #region RSS feeds
        private void ReadLatestNews()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += this.WebClient_DownloadStringCompleted;
            webClient.DownloadStringAsync(new Uri(FEED_URL));
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.LatestNews = new News(
                    AppResources.WebLatestNewsErrorTitle,
                    AppResources.WebLatestNewsErrorMessage,
                    DateTime.Now);
                return;
            }

            XDocument document = XDocument.Parse(e.Result);
            XElement item = document.Descendants("item").FirstOrDefault();
            if (item != null)
            {
                string title = item.Element("title").Value;
                string description = item.Element("description").Value;
                DateTime date = DateTime.Parse(item.Element("pubDate").Value);
                this.LatestNews = new News(title, description, date);
            }
        }
        #endregion

        #region Rating and opening the project website
        private void Rate()
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private void OpenProjectWebsite()
        {
            GameHelpers.OpenWebsite(GameHelpers.PROJECT_WEBSITE);
        }
        #endregion
    }
}
