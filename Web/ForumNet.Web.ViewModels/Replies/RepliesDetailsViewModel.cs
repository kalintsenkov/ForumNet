namespace ForumNet.Web.ViewModels.Replies
{
    using Users;

    public class RepliesDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Likes { get; set; }

        public string CreatedOn { get; set; }

        public int PostId { get; set; }

        public UsersInfoViewModel Author { get; set; }
    }
}
