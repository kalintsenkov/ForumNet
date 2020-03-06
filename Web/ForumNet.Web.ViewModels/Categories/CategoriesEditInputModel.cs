namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;
    using Services.Common.Attributes;

    public class CategoriesEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [ValidateCategoryName]
        public string Name { get; set; }
    }
}