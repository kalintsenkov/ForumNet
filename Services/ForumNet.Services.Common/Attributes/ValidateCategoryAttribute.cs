namespace ForumNet.Services.Common.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Extensions.DependencyInjection;

    using Contracts;

    public class ValidateCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoriesService = validationContext.GetService<ICategoriesService>();
            var isExisting = categoriesService.IsExisting((int) value).GetAwaiter().GetResult();
            if (!isExisting)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
