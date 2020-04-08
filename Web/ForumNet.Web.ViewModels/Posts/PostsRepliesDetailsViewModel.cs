namespace ForumNet.Web.ViewModels.Posts
{
    using Ganss.XSS;

    using Common;

    public class PostsRepliesDetailsViewModel
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public PostsRepliesDetailsViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedTags.Add(ModelConstants.IFrameAllowedTag);
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => this.htmlSanitizer.Sanitize(this.Description);

        public bool IsBestAnswer { get; set; }

        public int Likes { get; set; }

        public int Loves { get; set; }

        public int HahaCount { get; set; }

        public int WowCount { get; set; }

        public int SadCount { get; set; }

        public int AngryCount { get; set; }

        public string CreatedOn { get; set; }

        public int? ParentId { get; set; }

        public PostsAuthorDetailsViewModel Author { get; set; }
    }
}