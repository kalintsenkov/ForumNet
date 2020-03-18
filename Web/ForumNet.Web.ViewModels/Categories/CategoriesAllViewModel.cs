namespace ForumNet.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class CategoriesAllViewModel
    {
        public string Search { get; set; }

        public IEnumerable<CategoriesInfoViewModel> Categories { get; set; }
    }
}