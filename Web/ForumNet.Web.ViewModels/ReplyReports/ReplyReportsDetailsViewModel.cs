namespace ForumNet.Web.ViewModels.ReplyReports
{
    using Ganss.XSS;

    public class ReplyReportsDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => new HtmlSanitizer().Sanitize(this.Description);

        public string CreatedOn { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }

        public int ReplyId { get; set; }

        public string ReplyDescription { get; set; }

        public string SanitizedReplyDescription
            => new HtmlSanitizer().Sanitize(this.ReplyDescription);
    }
}
