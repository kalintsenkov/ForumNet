namespace ForumNet.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime RepliedOn { get; set; }

        public bool IsDeleted { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }
    }
}