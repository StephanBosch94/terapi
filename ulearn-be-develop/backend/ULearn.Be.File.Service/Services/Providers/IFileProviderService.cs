using Microsoft.AspNetCore.Http;
using ULearn.Be.Data.Enums;
using ULearn.Be.File.Data.Enums;

namespace ULearn.Be.File.Service.Services.Providers;

public interface IFileProviderService
{
    Stream GetFile(string uniqueKey, CCompressionTypeEnum compressionType);
    Task SaveFile(string uniqueKey, IFormFile file, CCompressionTypeEnum compressionType);
    Task DeleteFile(string entityPath);
}
