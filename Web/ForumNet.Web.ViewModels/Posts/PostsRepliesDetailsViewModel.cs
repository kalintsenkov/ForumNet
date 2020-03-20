namespace ForumNet.Web.ViewModels.Posts
{
    using Ganss.XSS;

    public class PostsRepliesDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => new HtmlSanitizer()
                .Sanitize(this.Description);

        public int Likes { get; set; }

        public string CreatedOn { get; set; }

        public PostsAuthorDetailsViewModel Author { get; set; }
    }
}