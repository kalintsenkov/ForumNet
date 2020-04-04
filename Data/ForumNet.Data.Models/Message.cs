namespace ForumNet.Data.Models
{
    using Common;
    using System;

    public class Message : IAuditInfo, IDeletableEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public ForumUser Author { get; set; }

        public string ReceiverId { get; set; }

        public ForumUser Receiver { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
