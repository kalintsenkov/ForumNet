namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    using Categories;
    using Common;
    using Common.Attributes;
    using Data.Common;
    using Tags;

    public class PostsEditViewModel
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
        [ValidateCategoryId]
        public int CategoryId { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [ValidateTagIds]
        [Display(Name = ModelConstants.TagsDisplayName)]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }

        public IEnumerable<CategoriesInfoViewModel> Categories { get; set; }
    }
}