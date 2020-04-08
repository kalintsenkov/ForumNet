namespace ForumNet.Web.ViewModels.Users
{
    using Ganss.XSS;

    using Common;

    public class UsersRepliesAllViewModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
        {
            get
            {
                var sanitized = new HtmlSanitizer().Sanitize(this.Description);

                return this.Description.Length > ModelConstants.ShortDescriptionAllowedLength
                    ? sanitized.Substring(0, ModelConstants.ShortDescriptionAllowedLength) + "..." 
                    : sanitized;
            }
        }

        public string Activity { get; set; }

        public int PostCategoryId { get; set; }

        public string PostCategoryName { get; set; }
    }
}