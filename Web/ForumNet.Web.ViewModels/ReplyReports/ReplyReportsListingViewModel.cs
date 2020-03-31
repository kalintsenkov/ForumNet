namespace ForumNet.Web.ViewModels.ReplyReports
{
    using Ganss.XSS;

    using Infrastructure;

    public class ReplyReportsListingViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedTags.Add(ModelConstants.IFrameAllowedTag);

                var sanitized = sanitizer.Sanitize(this.Description);

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
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedTags.Add(ModelConstants.IFrameAllowedTag);

                var sanitized = sanitizer.Sanitize(this.Description);

                return this.Description.Length > 44
                   ? sanitized.Substring(0, 44) + "..."
                   : sanitized;
            }
        }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }
    }
}
