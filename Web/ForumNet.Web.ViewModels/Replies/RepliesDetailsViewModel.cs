namespace ForumNet.Web.ViewModels.Replies
{
    using Ganss.XSS;

    public class RepliesDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => new HtmlSanitizer()
                .Sanitize(this.Description);

        public int Likes { get; set; }

        public int Loves { get; set; }

        public int HahaCount { get; set; }

        public int WowCount { get; set; }

        public int SadCount { get; set; }

        public int AngryCount { get; set; }

        public string CreatedOn { get; set; }

        public int PostId { get; set; }

        public string CurrentUserId { get; set; }

        public RepliesAuthorDetailsViewModel Author { get; set; }
    }
}
