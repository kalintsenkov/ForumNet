namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Categories;
    using Data.Common;
    using Data.Models.Enums;
    using Services.Common.Attributes;
    using Tags;

    public class PostsCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.PostTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Post Type")]
        [EnumDataType(typeof(PostType))]
        public PostType PostType { get; set; }

        [Display(Name = "Image Url")]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }

        [YouTubeUrl]
        [Display(Name = "Video Url")]
        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }

        [Required]
        [MaxLength(DataConstants.PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [ValidateCategoryId]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Tags")]
        [ValidateTagIds]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<CategoriesInfoViewModel> Categories { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }
    }
}