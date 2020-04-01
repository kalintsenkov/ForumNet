namespace ForumNet.Web.ViewModels.Tags
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Infrastructure.Attributes;

    public class TagsCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.TagNameMaxLength)]
        [ValidateTagName]
        public string Name { get; set; }
    }
}
