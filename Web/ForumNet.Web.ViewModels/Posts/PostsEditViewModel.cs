namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Categories;
    using Services.Common.Attributes;
    using Tags;

    public class PostsEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [ValidateCategory]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Tags")]
        [ValidateTagIds]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }

        public IEnumerable<CategoriesInfoViewModel> Categories { get; set; }
    }
}