namespace ForumNet.Web.ViewModels.PostReports
{
    using Ganss.XSS;

    using Common;

    public class PostReportsListingViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public PostReportsListingViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(GlobalConstants.IFrameTag);
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

        public string PostTitle { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }
    }
}
