namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Services.Common.Attributes;

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
        [ValidateCategoryId]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Tags")]
        [ValidateTagIds]
        public IEnumerable<int> TagIds { get; set; }
    }
}