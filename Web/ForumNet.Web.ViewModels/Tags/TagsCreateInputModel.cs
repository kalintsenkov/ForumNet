namespace ForumNet.Web.ViewModels.Tags
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;
    using Infrastructure;
    using Infrastructure.Attributes;

    public class TagsCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.TagNameMaxLength)]
        [EnsureTagNameNotExists(ErrorMessage = ErrorMessages.ExistingTagNameErrorMessage)]
        public string Name { get; set; }
    }
}
