using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomationTestProject.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AutomationTestProject.Application.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageStorageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task SaveProfileImage(string imgUrl)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(imgUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var imageStream = await response.Content.ReadAsStreamAsync())
                        {
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(imgUrl, imageStream),
                                Folder = "AutomationTestProject",
                                PublicId = "profile_image_" + Guid.NewGuid().ToString(),
                            };

                            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                throw new Exception("Failed to upload image to Cloudinary.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to fetch image from LinkedIn.");
                    }
                }
            }
        }
    }
}
