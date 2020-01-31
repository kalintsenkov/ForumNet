namespace ForumNet.Data.Models
{
    using System;

    public class Report
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }
    }
}