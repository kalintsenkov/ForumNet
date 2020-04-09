namespace ForumNet.Web.ViewModels.Tags
{
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Infrastructure.Attributes;

    public class TagsCreateInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.TagNameMaxLength)]
        [EnsureTagNameNotExists(ErrorMessage = ErrorMessages.TagExistingNameErrorMessage)]
        public string Name { get; set; }
    }
}
