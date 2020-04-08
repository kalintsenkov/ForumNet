namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;
   
    using Common;

    public class Reply : IAuditInfo, IDeletableEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsBestAnswer { get; set; }

        public int? ParentId { get; set; }

        public Reply Parent { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public string AuthorId { get; set; }

        public ForumUser Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<ReplyReport> Reports { get; set; } = new HashSet<ReplyReport>();

        public ICollection<ReplyReaction> Reactions { get; set; } = new HashSet<ReplyReaction>();
    }
}