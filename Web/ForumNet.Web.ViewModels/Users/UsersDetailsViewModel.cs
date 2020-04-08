namespace ForumNet.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class UsersDetailsViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string ProfilePicture { get; set; }

        public int Points { get; set; }

        public bool IsFollowed { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public IEnumerable<UsersThreadsAllViewModel> Threads { get; set; }

        public IEnumerable<UsersRepliesAllViewModel> Replies { get; set; }

        public IEnumerable<UsersFollowersAllViewModel> Followers { get; set; }

        public IEnumerable<UsersFollowingAllViewModel> Following { get; set; }
    }
}