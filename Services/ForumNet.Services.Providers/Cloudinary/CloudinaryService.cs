namespace ForumNet.Services.Providers.Cloudinary
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary) => this.cloudinary = cloudinary;

        public async Task<string> UploadAsync(IFormFile file, string fileName)
        {
            byte[] destinationData;

            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult;

            await using (var ms = new MemoryStream(destinationData))
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, ms)
                };

                uploadResult = this.cloudinary.Upload(uploadParams);
            }

            var secureUri = uploadResult.SecureUri;

            var version = secureUri.Segments[4];
            var imageFullName = secureUri.Segments[5];

            var uri = version + imageFullName;
            return uri;
        }
    }
}