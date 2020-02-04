namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Common;

    public class Category : IAuditInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}