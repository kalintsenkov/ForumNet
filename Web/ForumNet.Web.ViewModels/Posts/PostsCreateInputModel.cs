namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PostsCreateInputModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Post Type")]
        public string PostType { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Url")]
        public string ImageOrVideoUrl { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int[] TagIds { get; set; }

        public IEnumerable<CategoriesListingViewModel> Categories { get; set; }

        public IEnumerable<TagsListingViewModel> Tags { get; set; }
    }
}