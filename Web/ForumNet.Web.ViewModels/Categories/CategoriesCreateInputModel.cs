namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;
    
    using Data.Common;
    using Services.Common.Attributes;

    public class CategoriesCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [ValidateCategoryName]
        public string Name { get; set; }
    }
}