namespace ForumNet.Web.ViewModels.Tags
{
    using System.ComponentModel.DataAnnotations;

    using Common;
    using Infrastructure.Attributes;

    public class TagsCreateInputModel
    {
        [Required]
        [StringLength(GlobalConstants.TagNameMaxLength, ErrorMessage = ErrorMessages.TagNameLengthErrorMessage, MinimumLength = GlobalConstants.TagNameMinLength)]
        [EnsureTagNameNotExists(ErrorMessage = ErrorMessages.TagExistingNameErrorMessage)]
        public string Name { get; set; }
    }
}
