namespace ForumNet.Data.Models
{
    using System;

    public class ReplyReport
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ReplyId { get; set; }

        public Reply Reply { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }
    }
}