using Microsoft.AspNetCore.Http;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;
using ULearn.Be.File.Data.Enums;
using ULearn.Be.File.Service.Services.Extensions;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.File.Service.Services.Providers;

public class DiskFileProviderService : IDiskFileProviderService
{
    private readonly ICompressionService _compressionService;
    private readonly DiskSettings _diskSettings;

    public DiskFileProviderService(IEnvConfigurationProvider envCfgProvider, ICompressionService compressionService)
    {
        _compressionService = compressionService;
        _diskSettings = envCfgProvider.GetSettings<DiskSettings>(EnvSettingsKeyEnum.FileServiceDiskSettings);
    }

    public Stream GetFile(string uniqueKey, CCompressionTypeEnum compressionType)
    {
        var fileStream = new FileStream(GetRealPath(uniqueKey), FileMode.Open);
        if (compressionType == CCompressionTypeEnum.None)
        {
            // When returning filestream the framework automatically closes stream
            return fileStream;
        }

        var content = fileStream.ReadAllBytes();
        fileStream.Close(); //must be closed explicitly
        var decompressed = _compressionService.DecompressZip(content);
        return new MemoryStream(decompressed);
    }

    public async Task SaveFile(string uniqueKey, IFormFile file, CCompressionTypeEnum compressionType)
    {
        var readPath = GetRealPath(uniqueKey);
        Directory.CreateDirectory(Path.GetDirectoryName(readPath) ?? string.Empty);
        await using var fileStream = new FileStream(readPath, FileMode.Create);
        if (compressionType == CCompressionTypeEnum.None)
        {
            await file.CopyToAsync(fileStream);
            return;
        }

        var compressed = _compressionService.CompressZip(file.OpenReadStream().ReadAllBytes());
        await fileStream.WriteAsync(compressed);
    }

    public Task DeleteFile(string uniqueKey)
    {
        var readPath = GetRealPath(uniqueKey);
        System.IO.File.Delete(readPath);

        return Task.CompletedTask;
    }

    private string GetRealPath(string uniqueKey)
    {
        return Path.Combine(_diskSettings.FileStoragePath.NormalizeDirectoryPath(), uniqueKey);
    }
}
