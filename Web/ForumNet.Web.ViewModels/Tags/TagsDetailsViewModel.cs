namespace ForumNet.Web.ViewModels.Tags
{
    using System.Collections.Generic;

    using Posts;

    public class TagsDetailsViewModel
    {
        public string Search { get; set; }

        public TagsInfoViewModel Tag { get; set; }

        public IEnumerable<PostsListingViewModel> Posts { get; set; }
    }
}
