namespace SpaceAim3D.Models
{
    /// <summary>The class representing the single rank item.</summary>
    public class RankItem
    {
        /// <summary>Gets or sets a position of the item in the rank (starts from 1).</summary>
        public int Number { get; set; }

        /// <summary>Gets or sets a name of the player.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets a score.</summary>
        public int Score { get; set; }

        /// <summary>Gets a value indicating whether the item is in top three scores.</summary>
        public bool IsTopThree
        {
            get { return this.Number <= 3; }
        }

        /// <summary>Initializes a new instance of the RankItem class.</summary>
        /// <param name="number">A position in the rank.</param>
        /// <param name="name">A name of the player.</param>
        /// <param name="score">A score.</param>
        public RankItem(int number, string name, int score)
        {
            this.Number = number;
            this.Name = name;
            this.Score = score;
        }
    }
}
