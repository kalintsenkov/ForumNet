namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Infrastructure;
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
        [NonExistingCategoryId(ErrorMessage = ErrorMessages.NonExistingCategoryIdErrorMessage)]
        public int CategoryId { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [NonExistingTagIds(ErrorMessage = ErrorMessages.NonExistingTagIdErrorMessage)]
        [Display(Name = ModelConstants.TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }
    }
}