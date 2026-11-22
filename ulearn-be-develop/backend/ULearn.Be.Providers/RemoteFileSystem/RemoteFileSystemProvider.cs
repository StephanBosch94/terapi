using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.RemoteFileSystem.Interfaces;
using ULearn.Be.Providers.RemoteFileSystem.Models;

namespace ULearn.Be.Providers.RemoteFileSystem;

public class RemoteFileSystemProvider : IRemoteFileSystemProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly InstanceSettings _instanceSettings;
    private readonly ILogger<RemoteFileSystemProvider> _logger;
    private readonly RemoteFileSystemSettings _remoteFileSystemSettings;

    public RemoteFileSystemProvider(
        IEnvConfigurationProvider envCfgProvider,
        ILogger<RemoteFileSystemProvider> logger,
        IHttpClientFactory httpClientFactory
    )
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;

        _instanceSettings = envCfgProvider.GetSettings<InstanceSettings>(EnvSettingsKeyEnum.InstanceSettings);

        _remoteFileSystemSettings =
            envCfgProvider.GetSettings<RemoteFileSystemSettings>(EnvSettingsKeyEnum.RemoteFileSystemSettings);
    }

    public async Task<RemoteFile> GetFile(string path)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri =
                new Uri(
                    _remoteFileSystemSettings.ApiUrl +
                    $"environment/{UrlEscape(_instanceSettings.InstanceName)}/filepath/{UrlEscape(path)}"
                )
        };

        request.Headers.Authorization = GetToken();

        _logger.LogInformation("{Request}", $"   GET: {request.RequestUri}");
        var response = await httpClient.SendAsync(request);
        _logger.LogInformation("{Response}", $"   RESULT: [{response.StatusCode}]{response.ReasonPhrase}");

        EnsureSuccessStatusCode(response);

        return new RemoteFile
        {
            Stream = await response.Content.ReadAsStreamAsync(),
            ContentType = response.Content.Headers.ContentType?.MediaType,
            Name = response.Content.Headers.ContentDisposition?.FileName,
            Path = path
        };
    }

    public async Task SaveFile(Stream file, string directory, string fileName, string contentType = null)
    {
        file.Seek(0, SeekOrigin.Begin);
        var encodedFilePath = UrlEscape(directory);

        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_remoteFileSystemSettings.ApiUrl +
                                 $"environment/{_instanceSettings.InstanceName}/filepath/{encodedFilePath}"),
            Content = new MultipartFormDataContent
            {
                {
                    new StreamContent(file)
                    {
                        Headers =
                        {
                            ContentType = !string.IsNullOrEmpty(contentType)
                                ? new MediaTypeHeaderValue(contentType)
                                : null
                        }
                    },
                    "files", fileName
                }
            },
            Headers =
            {
                Authorization = GetToken(),
            }
        };

        _logger.LogInformation("   POST {RequestRequestUri} ; fileName=\"{FileName}\"", request.RequestUri, fileName);
        var response = await client.SendAsync(request);
        _logger.LogInformation("   RESULT: [{ResponseStatusCode}]", response.StatusCode);

        EnsureSuccessStatusCode(response);
    }

    public async Task EditFile(string path, FileEditVM edit)
    {
        var client = _httpClientFactory.CreateClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(
                _remoteFileSystemSettings.ApiUrl +
                $"environment/{_instanceSettings.InstanceName}/filepath/{Uri.EscapeDataString(path)}"
            )
        };
        request.Headers.Authorization = GetToken();
        request.Content = new StringContent(
            JsonConvert.SerializeObject(edit),
            Encoding.UTF8,
            "application/json"
        );
        _logger.LogInformation("   PUT {RequestRequestUri}", request.RequestUri);
        var response = await client.SendAsync(request);
        _logger.LogInformation("   RESULT: [{ResponseStatusCode}]{ResponseReasonPhrase}", response.StatusCode,
            response.ReasonPhrase);

        EnsureSuccessStatusCode(response);
    }

    public async Task DeleteFile(string path)
    {
        var client = _httpClientFactory.CreateClient();
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(
                _remoteFileSystemSettings.ApiUrl +
                $"environment/{_instanceSettings.InstanceName}/filepath/{Uri.EscapeDataString(path)}"
            )
        };
        request.Headers.Authorization = GetToken();

        _logger.LogInformation("   DELETE {RequestRequestUri}", request.RequestUri);
        var response = await client.SendAsync(request);
        _logger.LogInformation("   RESULT: [{ResponseStatusCode}]", response.StatusCode);
        EnsureSuccessStatusCode(response);
    }

    private void EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new ConflictException();
        }
    }

    private AuthenticationHeaderValue GetToken()
    {
        return new AuthenticationHeaderValue("Bearer", _remoteFileSystemSettings.AccessToken);
    }

    private static string UrlEscape(string original)
    {
        return Uri.EscapeDataString(original);
    }
}