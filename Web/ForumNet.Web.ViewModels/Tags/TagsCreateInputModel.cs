namespace ForumNet.Web.ViewModels.Tags
{
    using System.ComponentModel.DataAnnotations;

    using Infrastructure.Attributes;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public class TagsCreateInputModel
    {
        [Required]
        [StringLength(TagNameMaxLength, ErrorMessage = TagNameLengthErrorMessage, MinimumLength = TagNameMinLength)]
        [EnsureTagNameNotExists(ErrorMessage = TagExistingNameErrorMessage)]
        public string Name { get; set; }
    }
}
