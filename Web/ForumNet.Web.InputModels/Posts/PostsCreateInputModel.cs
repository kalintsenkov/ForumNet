namespace ForumNet.Web.InputModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Enums;
    using Infrastructure.Attributes;
    using ViewModels.Posts;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public class PostsCreateInputModel
    {
        [Required]
        [StringLength(PostTitleMaxLength, ErrorMessage = PostTitleLengthErrorMessage, MinimumLength = PostTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [EnumDataType(typeof(PostType))]
        [Display(Name = PostTypeDisplayName)]
        public PostType PostType { get; set; }

        [Required]
        [MaxLength(PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [EnsureCategoryIdExists(ErrorMessage = CategoryNonExistingIdErrorMessage)]
        public int CategoryId { get; set; }

        [EnsureTagIdsExists(ErrorMessage = TagNonExistingIdErrorMessage)]
        [Display(Name = TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<PostsCategoryDetailsViewModel> Categories { get; set; }

        public IEnumerable<PostsTagsDetailsViewModel> Tags { get; set; }
    }
}