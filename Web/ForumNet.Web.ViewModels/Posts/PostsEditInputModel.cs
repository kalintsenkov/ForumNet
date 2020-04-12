namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Infrastructure.Attributes;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public class PostsEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(PostTitleMaxLength, ErrorMessage = PostTitleLengthErrorMessage, MinimumLength = PostTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [EnsureCategoryIdExists(ErrorMessage = CategoryNonExistingIdErrorMessage)]
        public int CategoryId { get; set; }

        public string AuthorId { get; set; }

        [EnsureTagIdsExists(ErrorMessage = TagNonExistingIdErrorMessage)]
        [Display(Name = TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<PostsTagsDetailsViewModel> Tags { get; set; }

        public IEnumerable<PostsCategoryDetailsViewModel> Categories { get; set; }
    }
}