namespace ForumNet.Web.ViewModels.ReplyReports
{
    using Ganss.XSS;

    using Common;

    public class ReplyReportsListingViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public ReplyReportsListingViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(GlobalConstants.IFrameAllowedTag);
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var sanitized = this.sanitizer.Sanitize(this.Description);

                return this.Description.Length > GlobalConstants.ShortDescriptionAllowedLength
                   ? sanitized.Substring(0, GlobalConstants.ShortDescriptionAllowedLength) + "..."
                   : sanitized;
            }
        }

        public string CreatedOn { get; set; }

        public string ReplyDescription { get; set; }

        public string ShortReplyDescription
        {
            get
            {
                var sanitized = this.sanitizer.Sanitize(this.ReplyDescription);

                return this.Description.Length > GlobalConstants.ShortDescriptionAllowedLength
                   ? sanitized.Substring(0, GlobalConstants.ShortDescriptionAllowedLength) + "..."
                   : sanitized;
            }
        }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }
    }
}
