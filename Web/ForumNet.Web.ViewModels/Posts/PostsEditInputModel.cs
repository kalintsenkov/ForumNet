namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Infrastructure.Attributes;

    public class PostsEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.PostTitleMaxLength, ErrorMessage = ErrorMessages.PostTitleLengthErrorMessage, MinimumLength = GlobalConstants.PostTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [EnsureCategoryIdExists(ErrorMessage = ErrorMessages.CategoryNonExistingIdErrorMessage)]
        public int CategoryId { get; set; }

        public string AuthorId { get; set; }

        [EnsureTagIdsExists(ErrorMessage = ErrorMessages.TagNonExistingIdErrorMessage)]
        [Display(Name = ModelConstants.TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }
    }
}