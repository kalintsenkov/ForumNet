namespace ForumNet.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();
    }
}