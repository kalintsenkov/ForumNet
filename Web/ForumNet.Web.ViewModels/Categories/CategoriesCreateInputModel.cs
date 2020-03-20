namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Infrastructure.Attributes;

    public class CategoriesCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [ValidateCategoryName]
        public string Name { get; set; }
    }
}