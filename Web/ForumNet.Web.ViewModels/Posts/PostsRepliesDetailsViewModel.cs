namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using Ganss.XSS;

    public class PostsRepliesDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => new HtmlSanitizer()
                .Sanitize(this.Description);

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