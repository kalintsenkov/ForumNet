namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class PostsAllViewModel
    {
        public IEnumerable<PostsListingViewModel> Posts { get; set; }
    }
}