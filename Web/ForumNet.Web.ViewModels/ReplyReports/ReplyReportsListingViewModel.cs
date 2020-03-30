namespace ForumNet.Web.ViewModels.ReplyReports
{
    using Ganss.XSS;

    public class ReplyReportsListingViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var sanitized = new HtmlSanitizer().Sanitize(this.Description);

                return this.Description.Length > 44
                   ? sanitized.Substring(0, 44) + "..."
                   : sanitized;
            }
        }

        public string CreatedOn { get; set; }

        public string ReplyPostTitle { get; set; }

        public string ReplyDescription { get; set; }

        public string ShortReplyDescription
        {
            get
            {
                var sanitized = new HtmlSanitizer().Sanitize(this.ReplyDescription);

                return this.ReplyDescription.Length > 44
                   ? sanitized.Substring(0, 44) + "..."
                   : sanitized;
            }
        }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }
    }
}
