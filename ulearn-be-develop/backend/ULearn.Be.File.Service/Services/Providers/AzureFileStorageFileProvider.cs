using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;
using ULearn.Be.File.Data.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.File.Service.Services.Providers;

public class AzureFileStorageFileProvider : IAzureFileStorageFileProvider
{
    private readonly AzureBlobSettings _azureBlobSettings;
    private readonly ICompressionService _compressionService;

    public AzureFileStorageFileProvider(
        IEnvConfigurationProvider envCfgProvider,
        ICompressionService compressionService
    )
    {
        _compressionService = compressionService;
        _azureBlobSettings =
            envCfgProvider.GetSettings<AzureBlobSettings>(EnvSettingsKeyEnum.FileServiceAzureSettings);
    }

    public Stream GetFile(string uniqueKey, CCompressionTypeEnum compressionType)
    {
        var stream = new MemoryStream();
        CreateBlobClient(uniqueKey).DownloadTo(stream);
        stream.Position = 0;
        if (compressionType == CCompressionTypeEnum.None)
        {
            return stream;
        }

        var decompressed = _compressionService.DecompressZip(stream.ToArray());
        return new MemoryStream(decompressed);
    }

    public async Task SaveFile(string uniqueKey, IFormFile file, CCompressionTypeEnum compressionType)
    {
        if (compressionType == CCompressionTypeEnum.None)
        {
            await CreateBlobClient(uniqueKey).UploadAsync(file.OpenReadStream());
            return;
        }

        var compressed = _compressionService.CompressZip(file.OpenReadStream().ReadAllBytes());
        await using var compressedStream = new MemoryStream(compressed);
        await CreateBlobClient(uniqueKey).UploadAsync(compressedStream);
    }

    public Task DeleteFile(string entityPath)
    {
        throw new NotImplementedException();
    }

    private BlobClient CreateBlobClient(string path)
    {
        var connectionString = _azureBlobSettings.ConnectionString();

        var container = new BlobContainerClient(connectionString, _azureBlobSettings.ContainerName);
        return container.GetBlobClient(path);
    }
}
