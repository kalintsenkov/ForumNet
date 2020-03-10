namespace ForumNet.Web.Common.Attributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Extensions.DependencyInjection;

    using Services.Contracts;

    public class ValidateTagIdsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tagsService = validationContext.GetService<ITagsService>();
            var areExisting = tagsService.AreExisting((IEnumerable<int>) value).GetAwaiter().GetResult();
            if (!areExisting)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}