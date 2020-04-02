namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class PostsAllViewModel
    {
        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<PostsListingViewModel> Posts { get; set; }
    }
}