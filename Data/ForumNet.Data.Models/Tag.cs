namespace ForumNet.Data.Models
{
    using System.Collections.Generic;

    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<PostTag> Posts { get; set; } = new HashSet<PostTag>();
    }
}