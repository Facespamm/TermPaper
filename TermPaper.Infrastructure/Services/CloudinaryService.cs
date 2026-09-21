using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TermPaper.Infrastructure.Settings;

namespace TermPaper.Infrastructure.Services;

public class CloudinaryService
{
    private readonly CloudinarySettings  _settings;
    private readonly Cloudinary _cloudinary;
    
    public CloudinaryService(IOptions<CloudinarySettings> settings)
    {
        _settings = settings.Value;

        var account = new Account(
            _settings.CloudName,
            _settings.ApiKey,
            _settings.ApiSecret);
        
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string?> UploadImage(Stream stream, string fileName)
    {
        var uploadParams = new ImageUploadParams 
        { 
            File = new FileDescription(fileName, stream) 
        }; 
        var result = await _cloudinary.UploadAsync(uploadParams); 
 
        return result.SecureUrl?.ToString(); 


    }

}