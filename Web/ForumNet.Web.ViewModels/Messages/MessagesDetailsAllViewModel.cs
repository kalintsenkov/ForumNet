namespace ForumNet.Web.ViewModels.Messages
{
    using Ganss.XSS;

    using Infrastructure;

    public class MessagesDetailsAllViewModel
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public MessagesDetailsAllViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedTags.Add(ModelConstants.IFrameAllowedTag);
        }
        public int Id { get; set; }

        public string Content { get; set; }

        public string SanitizedContent
            => this.htmlSanitizer.Sanitize(this.Content);

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }

        public string CreatedOn { get; set; }
    }
}
