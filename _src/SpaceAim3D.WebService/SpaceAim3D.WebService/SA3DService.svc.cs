using System;
using System.Linq;

namespace SpaceAim3D.WebService
{
    /// <summary>The class representing the Space Aim 3D service.</summary>
    public class SA3DService : ISA3DService
    {
        private DbDataContext _db = new DbDataContext();

        /// <summary>Returns top results, divided into two ranks (from the last 24 hours and overall).</summary>
        /// <returns>Top results (from the last 24 hours and overall).</returns>
        public SA3DRank GetResults()
        {
            var overall = from r in this._db.Results
                          orderby r.Score descending
                          select new SA3DRankItem() { Name = r.Player.Name, Score = r.Score };
            var last24h = from r in this._db.Results
                          where r.Date.AddHours(24) >= DateTime.Now
                          orderby r.Score descending
                          select new SA3DRankItem() { Name = r.Player.Name, Score = r.Score };
            return new SA3DRank()
            {
                Overall = overall.Take(10).ToArray(),
                Last24Hours = last24h.Take(10).ToArray()
            };
        }

        /// <summary>Saves the current result. It will be later used while calculating top scores.</summary>
        /// <remarks>The method also updates a name of the player.</remarks>
        /// <param name="key">A key of the player.</param>
        /// <param name="name">A name of the player.</param>
        /// <param name="score">A score.</param>
        public void SendResult(string key, string name, int score)
        {
            var player = this._db.Players.SingleOrDefault(p => p.Key == key);
            if (player == null)
            {
                player = new Player() { Key = key, Name = name, LastUpdate = DateTime.Now };
                this._db.Players.InsertOnSubmit(player);
            }
            Result result = new Result() { Player = player, Score = score, Date = DateTime.Now };
            this._db.Results.InsertOnSubmit(result);
            this._db.SubmitChanges();
        }

        /// <summary>Returns data about other players, who have been active in the last hour.</summary>
        /// <param name="key">A key of the player.</param>
        /// <returns>Data about other players.</returns>
        public SA3DPlayer[] GetPlayers(string key)
        {
            var vicinity = from p in this._db.Players
                           where p.Key != key
                               && p.LocationLatitude != null
                               && p.LocationLongitude != null
                               && p.LastUpdate.AddHours(1) >= DateTime.Now
                           select new SA3DPlayer()
                           {
                               Name = p.Name,
                               LocationLatitude = (float)p.LocationLatitude,
                               LocationLongitude = (float)p.LocationLongitude
                           };
            return vicinity.ToArray();
        }

        /// <summary>Saves the current location of the player.</summary>
        /// <remarks>The method also updates a name of the player.</remarks>
        /// <param name="key">A key of the player.</param>
        /// <param name="name">A name of the player.</param>
        /// <param name="latitude">GPS coordinates (latitude).</param>
        /// <param name="longitude">GPS coordinates (longitude).</param>
        public void SendLocation(string key, string name, float latitude, float longitude)
        {
            var player = this._db.Players.SingleOrDefault(p => p.Key == key);
            if (player == null)
            {
                player = new Player() { Key = key, Name = name, LastUpdate = DateTime.Now };
                this._db.Players.InsertOnSubmit(player);
            }
            player.LocationLatitude = latitude;
            player.LocationLongitude = longitude;
            player.LastUpdate = DateTime.Now;
            this._db.SubmitChanges();
        }
    }
}
