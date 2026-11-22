using System.Security.Claims;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Providers.RemoteFileSystem.Models;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class FileService : IFileService
{
    private readonly IRUserFileRepository _rUserFileRepository;
    private readonly IAppMessageAttachmentRepository _appMessageAttachmentRepository;
    private readonly IRUserFilePermissionRepository _rUserFilePermissionRepository;

    public FileService(
        IRUserFileRepository rUserFileRepository,
        IAppMessageAttachmentRepository appMessageAttachmentRepository,
        IRUserFilePermissionRepository rUserFilePermissionRepository)
    {
        _rUserFileRepository = rUserFileRepository;
        _appMessageAttachmentRepository = appMessageAttachmentRepository;
        _rUserFilePermissionRepository = rUserFilePermissionRepository;
    }

    public async Task<RemoteFile> GetFile(string filePath, ClaimsPrincipal principal)
    {
        await CheckPermission(filePath, principal);
        
        // Try to get from user files first
        var userFile = await _rUserFileRepository.GetByFilePathOrDefault(filePath);
        if (userFile != null)
        {
            return new RemoteFile
            {
                Stream = new MemoryStream(userFile.Content),
                Name = userFile.FileName,
                ContentType = userFile.ContentType,
                Path = userFile.FilePath
            };
        }
        
        // Try to get from message attachments
        var attachment = await _appMessageAttachmentRepository.GetByFilePath(filePath);
        if (attachment != null)
        {
            return new RemoteFile
            {
                Stream = new MemoryStream(attachment.Content),
                Name = attachment.Name,
                ContentType = attachment.ContentType,
                Path = attachment.FilePath
            };
        }
        
        throw new NotFoundException($"File not found: {filePath}");
    }

    private async Task CheckPermission(string filePath, ClaimsPrincipal principal)
    {
        var ownerIds = await _rUserFilePermissionRepository.GetUserIds(filePath);
        var userId = principal.GetUserId();
        if (ownerIds.Any() && (principal == null || ownerIds.All(x => x != userId)))
        {
            throw new ForbiddenException();
        }
    }
}