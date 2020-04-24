namespace ForumNet.Web.ViewModels.Replies
{
    using Ganss.XSS;

    using Common;

    public class RepliesDeleteViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public RepliesDeleteViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(GlobalConstants.IFrameTag);
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => this.sanitizer.Sanitize(this.Description);

        public int Likes { get; set; }

        public int Loves { get; set; }

        public int HahaCount { get; set; }

        public int WowCount { get; set; }

        public int SadCount { get; set; }

        public int AngryCount { get; set; }

        public string CreatedOn { get; set; }

        public RepliesAuthorDetailsViewModel Author { get; set; }
    }
}