namespace ForumNet.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class UsersThreadsViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string ProfilePicture { get; set; }

        public int Level { get; set; }

        public IEnumerable<UsersThreadsAllViewModel> Threads { get; set; }
    }
}