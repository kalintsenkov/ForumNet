namespace ForumNet.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    public class FileVerifyExtensionsAttribute : ValidationAttribute
    {
        private readonly string extensions;

        public FileVerifyExtensionsAttribute(string extensions) 
            => this.extensions = extensions;

        public override bool IsValid(object value)
        {
            var allowedExtensions = this.extensions
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var file = value as IFormFile;
            if (file != null)
            {
                var fileName = file.FileName;

                return allowedExtensions.Any(y => fileName.EndsWith(y));
            }

            return true;
        }
    }
}