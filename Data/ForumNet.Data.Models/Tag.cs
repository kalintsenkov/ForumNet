namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Common;

    public class Tag : IAuditInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public ICollection<PostTag> Posts { get; set; } = new HashSet<PostTag>();
    }
}