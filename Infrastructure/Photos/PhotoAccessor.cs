using System;
using System.Net;
using Application.Interfaces;
using Application.Photos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Photos
{
  public class PhotoAccessor : IPhotoAccessor
  {
    private readonly Cloudinary _cloudinary;

    // this allow access to user-secrets
    public PhotoAccessor(IOptions<CloudinarySettings> config)
    {
      var acc = new Account(
        config.Value.CloudName,
        config.Value.ApiKey,
        config.Value.ApiSecret
      );

      _cloudinary = new Cloudinary(acc);
    }

    public PhotoUploadResult AddPhoto(IFormFile file)
    {
      // contains information after parsing image up to cloudinary.
      var uploadResult = new ImageUploadResult();

      // check if file is not empty
      if(file.Length > 0)
      {
        // read that file into memory BUT, due to storing it in memory, I want to dispose
        // it after operation.
        using (var stream = file.OpenReadStream())
        {
          // Information what we're passing - a file (photo).
          var uploadParams = new ImageUploadParams
          {
            // stream is a content of that file, what we are uploading.
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500)
              .Crop("fill").Gravity("face")
          };

          // This uploads it.
          uploadResult = _cloudinary.Upload(uploadParams);
        }
      }

      // Check if there is any errors in uploading.
      if (uploadResult.Error != null)
      {
        throw new Exception(uploadResult.Error.Message);
      }

      // after uploading an image, it will return two properties that I need
      // URL and public ID.
      return new PhotoUploadResult
      {
        PublicId = uploadResult.PublicId,
        Url = uploadResult.SecureUrl.AbsoluteUri
      };
    }

    public string DeletePhoto(string publicId)
    {
      var deleteParams = new DeletionParams(publicId);

      var result = _cloudinary.Destroy(deleteParams);

      return result.Result == "ok" ? result.Result : null;
    }
  }
}