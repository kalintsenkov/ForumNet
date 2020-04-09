namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Infrastructure.Attributes;

    public class CategoriesCreateInputModel
    {
        [Required]
        [StringLength(GlobalConstants.CategoryNameMaxLength, ErrorMessage = ErrorMessages.CategoryNameLengthErrorMessage, MinimumLength = GlobalConstants.CategoryNameMinLength)]
        [EnsureCategoryNameNotExists(ErrorMessage = ErrorMessages.CategoryExistingNameErrorMessage)]
        public string Name { get; set; }
    }
}