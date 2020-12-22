using System.Runtime.Serialization;

namespace SpaceAim3D.WebService
{
    /// <summary>The class representing the single player.</summary>
    [DataContract]
    public class SA3DPlayer
    {
        /// <summary>Gets or sets a name of the player.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets a latitude of the current player location.</summary>
        [DataMember]
        public float LocationLatitude { get; set; }

        /// <summary>Gets or sets a longitude of the current player location.</summary>
        [DataMember]
        public float LocationLongitude { get; set; }
    }
}