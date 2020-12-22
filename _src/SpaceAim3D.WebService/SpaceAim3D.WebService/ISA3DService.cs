using System.ServiceModel;

namespace SpaceAim3D.WebService
{
    /// <summary>The interface describing operations provided by the service.</summary>
    [ServiceContract]
    public interface ISA3DService
    {
        /// <summary>Returns top results, divided into two ranks (from the last 24 hours and overall).</summary>
        /// <returns>Top results (from the last 24 hours and overall).</returns>
        [OperationContract]
        SA3DRank GetResults();

        /// <summary>Saves the current result. It will be later used while calculating top scores.</summary>
        /// <remarks>The method also updates a name of the player.</remarks>
        /// <param name="key">A key of the player.</param>
        /// <param name="name">A name of the player.</param>
        /// <param name="score">A score.</param>
        [OperationContract]
        void SendResult(string key, string name, int score);

        /// <summary>Returns data about other players, who have been active in the last hour.</summary>
        /// <param name="key">A key of the player.</param>
        /// <returns>Data about other players.</returns>
        [OperationContract]
        SA3DPlayer[] GetPlayers(string key);

        /// <summary>Saves the current location of the player.</summary>
        /// <remarks>The method also updates a name of the player.</remarks>
        /// <param name="key">A key of the player.</param>
        /// <param name="name">A name of the player.</param>
        /// <param name="latitude">GPS coordinates (latitude).</param>
        /// <param name="longitude">GPS coordinates (longitude).</param>
        [OperationContract]
        void SendLocation(string key, string name, float latitude, float longitude);
    }
}
