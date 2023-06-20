using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Npgsql.BackendMessages;
using Rideshare.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Infrastructure.Services;
public class ResourceManager : IResourceManager
{
    private readonly Cloudinary _cloudinary;

    public ResourceManager(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<Uri> UploadPDF(IFormFile pdf)
    {
        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(pdf.FileName, pdf.OpenReadStream()),
        };
        var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
        return uploadResult.Url;
    }

    public async Task<Uri> UploadImage(IFormFile image)
        {
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
            };
            var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
            return uploadResult.Url;
        }
}
