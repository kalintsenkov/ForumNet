namespace ForumNet.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using Posts;

    public class CategoriesDetailsViewModel
    {
        public string Search { get; set; }

        public CategoriesInfoViewModel Category { get; set; }

        public IEnumerable<PostsListingViewModel> Posts { get; set; }
    }
}