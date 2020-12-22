using System.Runtime.Serialization;

namespace SpaceAim3D.WebService
{
    /// <summary>The class representing the global rank (from the last 24 hours and overall).</summary>
    [DataContract]
    public class SA3DRank
    {
        /// <summary>Gets or sets data of the overall rank.</summary>
        [DataMember]
        public SA3DRankItem[] Overall { get; set; }

        /// <summary>Gets or sets data of the rank from the last 24 hours.</summary>
        [DataMember]
        public SA3DRankItem[] Last24Hours { get; set; }
    }
}