using GART.Data;

namespace SpaceAim3D.Models
{
    /// <summary>The class representing data of the single indicator in the Augmented Reality feature.</summary>
    public class ARItemExtended : ARItem
    {
        /// <summary>Gets data of the player for navigation, i.e. a name and a location.</summary>
        public PlayerData Player
        {
            get
            {
                return new PlayerData()
                {
                    Name = (string)this.Content,
                    Location = this.GeoLocation
                };
            }
        }
    }
}
