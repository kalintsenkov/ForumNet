namespace ForumNet.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using Posts;

    public class HomeViewModel
    {
        public IEnumerable<PostsListingViewModel> Posts { get; set; }
    }
}