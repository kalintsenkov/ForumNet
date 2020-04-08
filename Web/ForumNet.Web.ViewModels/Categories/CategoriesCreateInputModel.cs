namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Data.Common;
    using Infrastructure.Attributes;

    public class CategoriesCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [EnsureCategoryNameNotExists(ErrorMessage = ErrorMessages.ExistingCategoryNameErrorMessage)]
        public string Name { get; set; }
    }
}