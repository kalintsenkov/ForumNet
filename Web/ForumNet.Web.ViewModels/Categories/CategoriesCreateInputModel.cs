namespace ForumNet.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class CategoriesCreateInputModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}