namespace ForumNet.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class YouTubeUrlAttribute : ValidationAttribute
    {
        private const string YouTubeRegexPattern = @"^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var match = Regex.Match((string) value, YouTubeRegexPattern);
            if (!match.Success)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}