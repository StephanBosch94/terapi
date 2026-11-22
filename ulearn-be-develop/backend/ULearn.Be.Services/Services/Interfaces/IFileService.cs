using System.Security.Claims;
using ULearn.Be.Providers.RemoteFileSystem.Models;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IFileService
{
    Task<RemoteFile> GetFile(string filePath, ClaimsPrincipal principal);
}