using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Utils;

namespace ULearn.Be.Services.Services;

public class RUserFileService : IRUserFileService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IGuidUtility _guidUtility;
    private readonly IRUserFileRepository _userFileRepository;

    public RUserFileService(
        IRUserFileRepository userFileRepository,
        IAppUserRepository appUserRepository,
        IGuidUtility guidUtility
    )
    {
        _userFileRepository = userFileRepository;
        _appUserRepository = appUserRepository;
        _guidUtility = guidUtility;
    }

    public async Task UploadProfilePhoto(IFormFile files, string email)
    {
        var user = await _appUserRepository.GetByLogin(email, onlyConfirmed: false);
        await UploadPhoto(files, user.UserId);
    }

    public Task UploadProfilePhoto(IFormFile file, ClaimsPrincipal user)
    {
        return UploadPhoto(file, user.GetUserId());
    }

    public async Task DeleteFile(string filepath, ClaimsPrincipal user)
    {
        var file = await _userFileRepository.GetDbEntity(filepath, user.GetUserId());
        await _userFileRepository.RemoveRange(new List<RUserFile> { file });
    }

    public async Task Upload(IFormFileCollection files, ClaimsPrincipal user, CFileTypeEnum fileTypeEnum)
    {
        var userId = user.GetUserId();
        
        foreach (var file in files)
        {
            var fileInfo = new UserFileInputVM
            {
                FileTypeId = fileTypeEnum,
                FileName = file.FileName,
                Directory = $"{fileTypeEnum.GetEnumKey()}/{_guidUtility.GenerateGuidString()}",
                ContentType = file.ContentType,
            };

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var content = memoryStream.ToArray();

            await _userFileRepository.AddWithContent(fileInfo, content, userId);
        }
    }

    private async Task RemoveFiles(int userId, CFileTypeEnum fileTypeEnum)
    {
        var userFiles = await _userFileRepository.GetDbEntities(userId, fileTypeEnum);
        await _userFileRepository.RemoveRange(userFiles);
    }

    private async Task UploadPhoto(IFormFile file, int userId)
    {
        await RemoveFiles(userId, CFileTypeEnum.ProfilePhoto);
        await RemoveFiles(userId, CFileTypeEnum.ProfilePhotoThumbnail);

        var profilePhotoContent = await ImageConverterUtil.ConvertAndCompressToJpeg(file);
        var profileThumbnailContent = await ImageConverterUtil.ConvertAndCompressThumbnailToJpeg(file);

        var profilePhotoInfo = new UserFileInputVM
        {
            FileTypeId = CFileTypeEnum.ProfilePhoto,
            FileName = file.FileName,
            Directory = $"{CFileTypeEnum.ProfilePhoto.GetEnumKey()}/{_guidUtility.GenerateGuidString()}",
            ContentType = "image/jpeg",
        };
        var profilePhotoThumbnailInfo = new UserFileInputVM
        {
            FileTypeId = CFileTypeEnum.ProfilePhotoThumbnail,
            FileName = file.FileName,
            Directory = $"{CFileTypeEnum.ProfilePhotoThumbnail.GetEnumKey()}/{_guidUtility.GenerateGuidString()}",
            ContentType = "image/jpeg",
        };

        await _userFileRepository.AddWithContent(profilePhotoInfo, profilePhotoContent, userId);
        await _userFileRepository.AddWithContent(profilePhotoThumbnailInfo, profileThumbnailContent, userId);
    }
}