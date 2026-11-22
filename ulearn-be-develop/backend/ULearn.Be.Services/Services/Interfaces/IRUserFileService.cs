using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IRUserFileService
{
    Task Upload(IFormFileCollection file, ClaimsPrincipal user, CFileTypeEnum fileTypeEnum);
    Task UploadProfilePhoto(IFormFile files, string email);
    Task UploadProfilePhoto(IFormFile files, ClaimsPrincipal user);
    Task DeleteFile(string filepath, ClaimsPrincipal user);
}