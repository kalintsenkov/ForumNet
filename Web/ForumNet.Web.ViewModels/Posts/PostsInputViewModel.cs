namespace ForumNet.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;

    public class PostsInputViewModel
    {
        [Required]
        [MaxLength(130)]
        public string Title { get; set; }

        [Required]
        public string PostType { get; set; }

        [DataType(DataType.Url)]
        public string ImageOrVideoUrl { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string Tags { get; set; }
    }
}