namespace ForumNet.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;

    public class PostsInputViewModel
    {
        [Required]
        [MaxLength(130)]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string Tags { get; set; }
    }
}