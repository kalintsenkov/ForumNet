namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Infrastructure;
    using Infrastructure.Attributes;

    public class CategoriesEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [ExistingCategoryName(ErrorMessage = ErrorMessages.ExistingCategoryNameErrorMessage)]
        public string Name { get; set; }
    }
}