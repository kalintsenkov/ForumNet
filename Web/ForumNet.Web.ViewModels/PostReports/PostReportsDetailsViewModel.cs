namespace ForumNet.Web.ViewModels.PostReports
{
    using Ganss.XSS;

    using Infrastructure;

    public class PostReportsDetailsViewModel
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public PostReportsDetailsViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedTags.Add(ModelConstants.IFrameAllowedTag);
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => this.htmlSanitizer.Sanitize(this.Description);

        public string CreatedOn { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }

        public int PostId { get; set; }

        public string PostTitle { get; set; }
    }
}
