namespace ForumNet.Web.ViewModels.Users
{
    using Ganss.XSS;

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

                return this.Description.Length > 44 
                    ? sanitized.Substring(0, 44) + "..." 
                    : sanitized;
            }
        }

        public string Activity { get; set; }

        public int PostCategoryId { get; set; }

        public string PostCategoryName { get; set; }
    }
}