namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;
    
    using Common.Attributes;
    using Data.Common;

    public class CategoriesEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [ValidateCategoryName]
        public string Name { get; set; }
    }
}