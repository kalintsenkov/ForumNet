namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Common;

    public class Post : IAuditInfo, IDeletableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int AuthorId { get; set; }

        public ForumUser Author { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Reply> Replies { get; set; } = new HashSet<Reply>();

        public ICollection<PostTag> Tags { get; set; } = new HashSet<PostTag>();

        public ICollection<PostReport> Reports { get; set; } = new HashSet<PostReport>();
    }
}