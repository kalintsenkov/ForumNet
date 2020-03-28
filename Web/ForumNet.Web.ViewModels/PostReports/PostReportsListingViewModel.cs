namespace ForumNet.Web.ViewModels.PostReports
{
    using Ganss.XSS;

    public class PostReportsListingViewModel
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

        public string PostTitle { get; set; }

        public string AuthorProfilePicture { get; set; }
    }
}
