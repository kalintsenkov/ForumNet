namespace ForumNet.Web.ViewModels.Tags
{
    using System.Collections.Generic;

    public class TagsAllViewModel
    {
        public string Search { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }
    }
}
