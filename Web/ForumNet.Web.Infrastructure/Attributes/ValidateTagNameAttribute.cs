namespace ForumNet.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Extensions.DependencyInjection;

    using Services.Contracts;

    public class ValidateTagNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tagsService = validationContext.GetService<ITagsService>();
            var isExisting = tagsService.IsExisting((string)value).GetAwaiter().GetResult();
            if (isExisting)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
