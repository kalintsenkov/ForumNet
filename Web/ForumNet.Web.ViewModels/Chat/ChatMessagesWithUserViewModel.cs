namespace ForumNet.Web.ViewModels.Chat
{
    using Ganss.XSS;

    public class ChatMessagesWithUserViewModel
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public ChatMessagesWithUserViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
        }

        public string Content { get; set; }

        public string SanitizedContent
            => this.htmlSanitizer.Sanitize(this.Content);

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }

        public string CreatedOn { get; set; }
    }
}
