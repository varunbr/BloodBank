﻿using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadImage(IFormFile formFile);
        Task<DeletionResult> DeleteImage(string publicId);
    }
}
