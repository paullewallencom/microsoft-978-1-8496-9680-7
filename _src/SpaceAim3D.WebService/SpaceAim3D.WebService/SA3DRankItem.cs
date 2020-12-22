using System.Runtime.Serialization;

namespace SpaceAim3D.WebService
{
    /// <summary>The class representing the single rank item.</summary>
    [DataContract]
    public class SA3DRankItem
    {
        /// <summary>Gets or sets a name of the player.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets a score.</summary>
        [DataMember]
        public int Score { get; set; }
    }
}