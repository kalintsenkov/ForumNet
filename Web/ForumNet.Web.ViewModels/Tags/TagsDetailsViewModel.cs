namespace ForumNet.Web.ViewModels.Tags
{
    using System.Collections.Generic;

    using Posts;

    public class TagsDetailsViewModel
    {
        public TagsInfoViewModel Tag { get; set; }

        public IEnumerable<PostsListingViewModel> Posts { get; set; }
    }
}
