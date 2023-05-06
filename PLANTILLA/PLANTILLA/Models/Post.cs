using System;
using System.Text.RegularExpressions;

namespace PLANTILLA.Models
{
    public class Post
    {
        public Post(
            string title,
            string description,
            string content,
            string category,
            string author,
            string url,
            DateTime publishedOn)
        {
            this.Title = title;
            this.Description = description;
            this.Content = content;
            this.Category = category;
            this.Url = url;
            this.Author = author;
            this.Slug = Regex.Replace(this.Title.ToLower(), @"\s", "-", RegexOptions.Compiled);
            this.PostId = Guid.NewGuid();
            this.PublishedOn = publishedOn;
        }

        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Slug { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
    }
}
