namespace ForumNet.Web.ViewModels.Tags
{
    using System.Collections.Generic;

    public class TagsAllViewModel
    {
        public string Search { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NextPage
        {
            get
            {
                if (this.PageIndex >= this.TotalPages)
                {
                    return 1;
                }

                return this.PageIndex + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.PageIndex <= 1)
                {
                    return this.TotalPages;
                }

                return this.PageIndex - 1;
            }
        }
    }
}
