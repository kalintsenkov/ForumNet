namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Data.Models.Enums;
    using Infrastructure.Attributes;

    public class PostsCreateInputModel
    {
        [Required]
        [StringLength(GlobalConstants.PostTitleMaxLength, ErrorMessage = ErrorMessages.PostTitleLengthErrorMessage, MinimumLength = GlobalConstants.PostTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [EnumDataType(typeof(PostType))]
        [Display(Name = GlobalConstants.PostTypeDisplayName)]
        public PostType PostType { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [EnsureCategoryIdExists(ErrorMessage = ErrorMessages.CategoryNonExistingIdErrorMessage)]
        public int CategoryId { get; set; }

        [EnsureTagIdsExists(ErrorMessage = ErrorMessages.TagNonExistingIdErrorMessage)]
        [Display(Name = GlobalConstants.TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<PostsCategoryDetailsViewModel> Categories { get; set; }

        public IEnumerable<PostsTagsDetailsViewModel> Tags { get; set; }
    }
}