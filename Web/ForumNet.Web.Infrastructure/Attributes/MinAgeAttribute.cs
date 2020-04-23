namespace ForumNet.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int minAge;

        public MinAgeAttribute(int minAge) => this.minAge = minAge;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateOfBirth = (DateTime?)value;
            if (!dateOfBirth.HasValue)
            {
                return null;
            }

            var currentYear = DateTime.UtcNow.Year;
            var yearOfBirth = dateOfBirth.Value.Year;
            var age = currentYear - yearOfBirth;
            if (dateOfBirth > DateTime.UtcNow.AddYears(-age))
            {
                age--;
            }

            if (age < this.minAge)
            {
                return new ValidationResult(string.Format(this.ErrorMessage, this.minAge));
            }

            return ValidationResult.Success;
        }
    }
}
