namespace ForumNet.Web.InputModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Infrastructure.Attributes;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public class CategoriesCreateInputModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength, ErrorMessage = CategoryNameLengthErrorMessage, MinimumLength = CategoryNameMinLength)]
        [EnsureCategoryNameNotExists(ErrorMessage = CategoryExistingNameErrorMessage)]
        public string Name { get; set; }
    }
}