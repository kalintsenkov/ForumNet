namespace ForumNet.Web.ViewModels.Users
{
    using Ganss.XSS;

    public class UsersRepliesViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public UsersRepliesViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => this.sanitizer.Sanitize(this.Description);

        public string Activity { get; set; }

        public int PostCategoryId { get; set; }

        public string PostCategoryName { get; set; }
    }
}