using System.Device.Location;

namespace SpaceAim3D.Models
{
    /// <summary>The class representing data of the player, which are necessary for the navigation.</summary>
    public class PlayerData
    {
        /// <summary>Gets or sets a name of the player.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets a location of the player.</summary>
        public GeoCoordinate Location { get; set; }
    }
}
