namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Data.Models.Enums;
    using Infrastructure;
    using Infrastructure.Attributes;

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
        [NonExistingCategoryId(ErrorMessage = ErrorMessages.NonExistingCategoryIdErrorMessage)]
        public int CategoryId { get; set; }

        [Required]
        [NonExistingTagIds(ErrorMessage = ErrorMessages.NonExistingTagIdErrorMessage)]
        [Display(Name = ModelConstants.TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<PostsCategoryDetailsViewModel> Categories { get; set; }

        public IEnumerable<PostsTagsDetailsViewModel> Tags { get; set; }
    }
}