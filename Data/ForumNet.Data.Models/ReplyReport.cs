namespace ForumNet.Data.Models
{
    using System;
    
    using Common;

    public class ReplyReport : IAuditInfo
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int ReplyId { get; set; }

        public Reply Reply { get; set; }

        public string AuthorId { get; set; }

        public ForumUser Author { get; set; }
    }
}