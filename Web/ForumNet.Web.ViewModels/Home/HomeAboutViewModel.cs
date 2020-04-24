namespace ForumNet.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class HomeAboutViewModel
    {
        public int PostsCount { get; set; }

        public int UsersCount { get; set; }

        public int ReactionsCount { get; set; }

        public IEnumerable<HomeAdminViewModel> Admins { get; set; }
    }
}
