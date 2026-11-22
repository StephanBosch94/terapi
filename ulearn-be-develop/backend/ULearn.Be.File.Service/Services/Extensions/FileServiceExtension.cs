using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.File.Service.Services.Providers;

namespace ULearn.Be.File.Service.Services.Extensions;

public static class FileServiceExtension
{
    public static void AddFileServices(this IServiceCollection services, bool isDisk)
    {
        if (isDisk)
        {
            services.AddTransient<IFileProviderService, DiskFileProviderService>();
        }
        else
        {
            services.AddTransient<IFileProviderService, AzureFileStorageFileProvider>();
        }

        services.AddScoped<IDiskFileProviderService, DiskFileProviderService>();
        services.AddScoped<IAzureFileStorageFileProvider, AzureFileStorageFileProvider>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ICompressionService, CompressionService>();
    }
}