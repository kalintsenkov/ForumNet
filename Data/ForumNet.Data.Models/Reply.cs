﻿namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Reply
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Likes { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public ICollection<ReplyReport> Reports { get; set; } = new HashSet<ReplyReport>();
    }
}