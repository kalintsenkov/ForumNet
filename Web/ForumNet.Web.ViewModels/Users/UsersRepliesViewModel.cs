namespace ForumNet.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class UsersRepliesViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string ProfilePicture { get; set; }

        public int Level { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public IEnumerable<UsersRepliesAllViewModel> Replies { get; set; }
    }
}
