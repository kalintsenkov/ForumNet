namespace ForumNet.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;

    using Data.Models.Enums;
    using Tags;

    public class PostsDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public PostType Type { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public string Description { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorName { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }
    }
}