namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public GenderType Gender { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime BirthDate { get; set; }

        public byte[] ProfilePicture { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<Reply> Replies { get; set; } = new HashSet<Reply>();
    }
}