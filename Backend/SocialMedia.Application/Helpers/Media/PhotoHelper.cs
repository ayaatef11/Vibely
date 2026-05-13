using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace SocialMedia.Application.Helpers.Media;

public class PhotoHelper
{
    private readonly Cloudinary _cloudinary;

    public PhotoHelper(IOptions<CloudinarySettings> config)
    {
        var settings = config.Value;
        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true; 
    }

    public async Task<string> UploadPhotoAsync(IFormFile photo)
    {
        if (photo.Length <= 0)
            throw new ArgumentException("File is empty.");

        await using var stream = photo.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(photo.FileName, stream),
            Folder = "social-media/posts",           
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")        
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error is not null)
            throw new Exception($"Cloudinary upload failed: {result.Error.Message}");

        return result.SecureUrl.ToString();
    }

    public async Task DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}