namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Views { get; set; }

        public int Rating { get; set; }

        public bool IsDeleted { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Reply> Replies { get; set; } = new HashSet<Reply>();

        public ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();
    }
}