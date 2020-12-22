using Microsoft.Phone.Tasks;
using System;
using System.Device.Location;

namespace SpaceAim3D.Models
{
    /// <summary>The class with auxiliary methods for the managed part of the game.</summary>
    public static class GameHelpers
    {
        /// <summary>An address of the project website.</summary>
        public const string PROJECT_WEBSITE = "http://jamro.biz/spaceaim3d/";

        /// <summary>An address of the Facebook profile.</summary>
        public const string FACEBOOK_PROFILE = "http://facebook.com/spaceaim3d";

        /// <summary>An address of the RSS feed with news regarding the game.</summary>
        public const string RSS_FEED = "http://jamro.biz/spaceaim3d/rss.xml";

        /// <summary>An e-mail address of the author.</summary>
        public const string AUTHOR_EMAIL = "marcin@jamro.biz";

        /// <summary>Opens the website, which address is given as the parameter.</summary>
        /// <param name="link">An address of the website.</param>
        public static void OpenWebsite(string link)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.Uri = new Uri(link, UriKind.Absolute);
            wbt.Show();
        }

        /// <summary>Starts a process of sending an e-mail message with the given subject, to particular receipient.</summary>
        /// <remarks>This method does not send the message. It just opens another application, where we can compose the e-mail.</remarks>
        /// <param name="toAddress">An address of the receipient.</param>
        /// <param name="subject">A subject of the message.</param>
        public static void SendEmail(string toAddress, string subject)
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.Subject = subject;
            ect.To = toAddress;
            ect.Show();
        }

        /// <summary>Launches an application that can navigate us to the particular location.</summary>
        /// <param name="playerName">A name of the player, whose location we want to use as a target.</param>
        /// <param name="location">A location of the player, used as a target for the navigation.</param>
        public static void NavigateToPlayer(string playerName, GeoCoordinate location)
        {
            MapsDirectionsTask task = new MapsDirectionsTask();
            LabeledMapLocation endPoint = new LabeledMapLocation(playerName, location);
            task.End = endPoint;
            task.Show();
        }
    }
}
