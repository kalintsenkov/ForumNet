namespace ForumNet.Web.ViewModels.PostReports
{
    using Ganss.XSS;

    public class PostReportsDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => new HtmlSanitizer().Sanitize(this.Description);

        public string CreatedOn { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }

        public int PostId { get; set; }

        public string PostTitle { get; set; }
    }
}
