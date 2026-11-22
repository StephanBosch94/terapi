using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ULearn.Be.File.Service.ViewModels;

namespace ULearn.Be.File.Service.Services;

public interface IFileService
{
    Task<(Stream, AppFileRaw)> GetFileStream(string env, string filepath, bool ignoreDeleted = true);
    Task<(Stream, string, string)> GetArchiveStream(string env, string filepath, bool ignoreDeleted = true);

    Task<IReadOnlyCollection<AppFileRaw>> SaveFile(
        string environment,
        string filepath,
        IFormFileCollection files,
        ClaimsPrincipal principal
    );

    Task DeleteFile(string environment, string filePath, ClaimsPrincipal user);
    Task EditFile(string environment, string filePath, AppFileEditVM edit, ClaimsPrincipal user);
    Task<bool> ExistsFile(string environment, string filePath);
    Task<AppFileRaw> GetFileInfo(string environment, string filepath);
    Task<AppFileRaw> GetFileInfo(string fileKey);
}
