namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using Categories;
    using Tags;

    public class PostsListingViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public CategoriesInfoViewModel Category { get; set; }

        public int Likes { get; set; }

        public int RepliesCount { get; set; }

        public int Views { get; set; }

        //public int Activity { get; set; }

        public string AuthorProfilePicture { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }
    }
}
