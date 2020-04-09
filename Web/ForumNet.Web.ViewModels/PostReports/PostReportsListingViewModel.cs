namespace ForumNet.Web.ViewModels.PostReports
{
    using Ganss.XSS;

    using Common;

    public class PostReportsListingViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedTags.Add(GlobalConstants.IFrameAllowedTag);

                var sanitized = sanitizer.Sanitize(this.Description);

                return this.Description.Length > GlobalConstants.ShortDescriptionAllowedLength
                   ? sanitized.Substring(0, GlobalConstants.ShortDescriptionAllowedLength) + "..."
                   : sanitized;
            }
        }

        public string CreatedOn { get; set; }

        public string PostTitle { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }
    }
}
