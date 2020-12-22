using System;

namespace SpaceAim3D.Models
{
    /// <summary>The class representing the single news, e.g. read from a RSS feed.</summary>
    public class News
    {
        /// <summary>Gets or sets a title of the news.</summary>
        public string Title { get; set; }

        /// <summary>Gets or sets a short description of the news.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets a publishing date of the news.</summary>
        public DateTime Date { get; set; }

        /// <summary>Initializes a new instance of the News class.</summary>
        /// <param name="title">A title of the news.</param>
        /// <param name="description">A description of the news.</param>
        /// <param name="date">A publishing date of the news.</param>
        public News(string title, string description, DateTime date)
        {
            this.Title = title;
            this.Description = description;
            this.Date = date;
        }
    }
}
