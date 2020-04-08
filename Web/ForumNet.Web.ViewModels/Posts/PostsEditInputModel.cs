namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Data.Common;
    using Infrastructure.Attributes;

    public class PostsEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.PostTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DataConstants.PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [EnsureCategoryIdExists(ErrorMessage = ErrorMessages.NonExistingCategoryIdErrorMessage)]
        public int CategoryId { get; set; }

        public string AuthorId { get; set; }

        [EnsureTagIdsExists(ErrorMessage = ErrorMessages.NonExistingTagIdErrorMessage)]
        [Display(Name = ModelConstants.TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }
    }
}