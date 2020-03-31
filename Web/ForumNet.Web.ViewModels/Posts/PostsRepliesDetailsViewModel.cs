namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using Ganss.XSS;

    using Infrastructure;

    public class PostsRepliesDetailsViewModel
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public PostsRepliesDetailsViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedTags.Add(ModelConstants.IFrameAllowedTag);
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => this.htmlSanitizer.Sanitize(this.Description);

        public int Likes { get; set; }

        public int Loves { get; set; }

        public int HahaCount { get; set; }

        public int WowCount { get; set; }

        public int SadCount { get; set; }

        public int AngryCount { get; set; }

        public string CreatedOn { get; set; }

        public PostsAuthorDetailsViewModel Author { get; set; }

        public IEnumerable<PostsRepliesDetailsViewModel> Nested { get; set; }
    }
}