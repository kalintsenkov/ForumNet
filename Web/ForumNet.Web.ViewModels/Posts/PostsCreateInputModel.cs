namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Enums;

    public class PostsCreateInputModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Post Type")]
        [EnumDataType(typeof(PostType))]
        public PostType PostType { get; set; }

        [Display(Name = "Url")]
        [DataType(DataType.Url)]
        public string ImageOrVideoUrl { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<CategoriesListingViewModel> Categories { get; set; }

        public IEnumerable<TagsListingViewModel> Tags { get; set; }
    }
}