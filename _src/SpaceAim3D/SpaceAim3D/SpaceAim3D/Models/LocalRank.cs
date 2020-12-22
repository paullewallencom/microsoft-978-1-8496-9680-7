using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace SpaceAim3D.Models
{
    /// <summary>The class representing the local rank. It allows to add results to it, as well as read top scores.</summary>
    public class LocalRank
    {
        private const string FILENAME = "Rank.txt";
        private IsolatedStorageFile m_file = IsolatedStorageFile.GetUserStoreForApplication();

        /// <summary>Adds a result to the local rank.</summary>
        /// <param name="name">A name of the player.</param>
        /// <param name="score">A score.</param>
        public void AddResult(string name, int score)
        {
            IsolatedStorageFileStream stream = this.m_file.OpenFile(FILENAME, FileMode.Append, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("{0};{1}", name, score);
            }
        }

        /// <summary>Returns ten top scores from the local rank.</summary>
        /// <returns>Ten rank items with the highest scores.</returns>
        public RankItem[] ReadTopScores()
        {
            List<RankItem> items = new List<RankItem>();
            IsolatedStorageFileStream stream = this.m_file.OpenFile(FILENAME, FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    RankItem item = new RankItem(-1, parts[0], int.Parse(parts[1]));
                    items.Add(item);
                }
            }

            items = items.OrderByDescending(i => i.Score).Take(10).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Number = i + 1;
            }
            return items.ToArray();
        }
    }
}
