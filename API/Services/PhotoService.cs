﻿using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    
    public class PhotoService : IPhotoService
    {

        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {

            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary( acc );
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var imageUploadResult= new ImageUploadResult();
            if(file.Length > 0)
            {
                using var stream=file.OpenReadStream();
                var uploadsparams = new ImageUploadParams
                {
                  File=new FileDescription(file.FileName, stream),
                  Transformation= new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
                  Folder="Dating-App-.Net7"
                };

                imageUploadResult= await _cloudinary.UploadAsync(uploadsparams);

            }
            return imageUploadResult;

        }

        public async Task<DeletionResult> DeletePhotoAsync(string PublicId)
        {
            var deletePhoto = new DeletionParams(PublicId);
            return await _cloudinary.DestroyAsync(deletePhoto);
            
        }
    }
}
