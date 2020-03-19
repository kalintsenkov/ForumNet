namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using Categories;
    using Data.Models.Enums;
    using Replies;
    using Tags;
    using Users;

    public class PostsDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public PostType Type { get; set; }

        public string Description { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }

        public int RepliesCount { get; set; }

        public bool IsPinned { get; set; }

        public string CreatedOn { get; set; }

        public string CurrentUserId { get; set; }

        public UsersInfoViewModel Author { get; set; }

        public CategoriesInfoViewModel Category { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }

        public IEnumerable<RepliesDetailsViewModel> Replies { get; set; }
    }
}