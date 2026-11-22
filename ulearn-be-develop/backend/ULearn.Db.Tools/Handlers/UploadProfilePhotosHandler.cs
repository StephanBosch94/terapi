using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using ULearn.Db.Tools.Models;
using ULearn.Db.Tools.Models.Options;

namespace ULearn.Db.Tools.Handlers;

public class UploadProfilePhotosHandler
{
    private readonly ILogger<UploadProfilePhotosHandler> _logger;

    public UploadProfilePhotosHandler(ILogger<UploadProfilePhotosHandler> logger)
    {
        _logger = logger;
    }

    public async Task Run(UploadProfilePhotosOptions obj)
    {
        var photos = Directory.EnumerateFiles(obj.PathToFolder)
            .Select(Parse);


        using var httpClient = new HttpClient();

        foreach (var profilePhotoPathVm in photos)
        {
            await Send(httpClient, obj.BeApiUrl, obj.BearerToken, profilePhotoPathVm);
        }

        _logger.LogInformation("Photos are uploaded");
    }

    private async Task Send(HttpClient httpClient, string url, string token, ProfilePhotoPathVM photoPathVm)
    {
        var stream = new MemoryStream(photoPathVm.Content);
        stream.Seek(0, SeekOrigin.Begin);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
            RequestUri = new Uri(url),
            Content = new MultipartFormDataContent
            {
                {
                    new StreamContent(stream)
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") },
                    },
                    "files", photoPathVm.Email
                },
                new StringContent(photoPathVm.Email)
                {
                    Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "email",
                        }
                    }
                },
            }
        };
        _logger.LogInformation("Sending request to [{Url}] path [{Path}] email [{Email}]", url, photoPathVm.Path,
            photoPathVm.Email);
        var response = await httpClient.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("    Sent !");
        }
        catch (Exception e)
        {
            _logger.LogError("{Error}", e.ToString());
        }
    }

    private ProfilePhotoPathVM Parse(string path)
    {
        _logger.LogInformation("Reading \'{Path}\'", path);
        return new ProfilePhotoPathVM
        {
            Email = Path.GetFileNameWithoutExtension(path),
            Path = path,
            Content = File.ReadAllBytes(path)
        };
    }
}