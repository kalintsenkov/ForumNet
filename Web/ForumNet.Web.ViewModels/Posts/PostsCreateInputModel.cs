namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Categories;
    using Common;
    using Common.Attributes;
    using Data.Common;
    using Data.Models.Enums;
    using Tags;

    public class PostsCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.PostTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [EnumDataType(typeof(PostType))]
        [Display(Name = ModelConstants.PostTypeDisplayName)]
        public PostType PostType { get; set; }

        [Required]
        [MaxLength(DataConstants.PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [ValidateCategoryId]
        public int CategoryId { get; set; }

        [Required]
        [ValidateTagIds]
        [Display(Name = ModelConstants.TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<CategoriesInfoViewModel> Categories { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }
    }
}