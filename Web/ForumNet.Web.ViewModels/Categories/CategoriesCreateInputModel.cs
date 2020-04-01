namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Infrastructure;
    using Infrastructure.Attributes;

    public class CategoriesCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [ExistingCategoryName(ErrorMessage = ErrorMessages.ExistingCategoryNameErrorMessage)]
        public string Name { get; set; }
    }
}