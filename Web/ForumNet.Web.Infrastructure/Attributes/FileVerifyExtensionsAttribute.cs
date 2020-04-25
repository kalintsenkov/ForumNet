namespace ForumNet.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    public class FileVerifyExtensionsAttribute : ValidationAttribute
    {
        private string[] AllowedExtensions { get; }

        public FileVerifyExtensionsAttribute(string fileExtensions) 
            => this.AllowedExtensions = fileExtensions
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var fileName = file.FileName;

                return this.AllowedExtensions.Any(y => fileName.EndsWith(y));
            }

            return true;
        }
    }
}