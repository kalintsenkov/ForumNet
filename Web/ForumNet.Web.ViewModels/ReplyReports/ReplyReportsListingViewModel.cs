namespace ForumNet.Web.ViewModels.ReplyReports
{
    using Ganss.XSS;

    using Common;

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

                return this.Description.Length > GlobalConstants.ShortDescriptionAllowedLength
                   ? sanitized.Substring(0, GlobalConstants.ShortDescriptionAllowedLength) + "..."
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

                return this.Description.Length > GlobalConstants.ShortDescriptionAllowedLength
                   ? sanitized.Substring(0, GlobalConstants.ShortDescriptionAllowedLength) + "..."
                   : sanitized;
            }
        }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }
    }
}
