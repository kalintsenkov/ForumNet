namespace ForumNet.Web.ViewModels.Users
{
    using System;

    public class UsersFollowersAllViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string ProfilePicture { get; set; }

        public string ThreadsCount { get; set; }

        public string RepliesCount { get; set; }

        public int Level { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
