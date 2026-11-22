using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.Utils;
using ULearn.Be.Providers.Video.Interfaces;
using ULearn.Be.Providers.Video.Models;

namespace ULearn.Be.Providers.Video;

public class VideoCallProvider : IVideoCallProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<VideoCallProvider> _logger;
    private readonly VideoCallSettings _videoCallSettings;

    public VideoCallProvider(IEnvConfigurationProvider envConfigurationProvider, IHttpClientFactory httpClientFactory,
        ILogger<VideoCallProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _videoCallSettings =
            envConfigurationProvider.GetSettings<VideoCallSettings>(EnvSettingsKeyEnum.VideoCallSettings);
    }

    public async Task<CreateRoomResponseVM> CreateRoom(int reservationDurationInMinutes)
    {
        _logger.LogInformation("Calling Video Call API createMeeting()");
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.videosdk.live/v2/rooms"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Authorization", GetToken() },
            },
            Content = HttpUtils.CreateContent(new CreateRoomRequestVM
            {
                AutoCloseConfig = new AutoCloseConfigVM
                {
                    Type = "session-ends",
                    Duration = reservationDurationInMinutes + 10
                }
            })
        };
        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            _logger.LogError("Calling Video Call API createMeeting(): Fail, message: {ResponseMessage}",
                responseString);
            throw;
        }

        _logger.LogInformation("Successfully created a meeting room");
        return JsonConvert.DeserializeObject<CreateRoomResponseVM>(responseString);
    }

    public string GetToken()
    {
        return JwtBuilder.Create()
#pragma warning disable CS0618
            .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
#pragma warning restore CS0618
            .WithSecret(_videoCallSettings.Secret)
            .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds())
            .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            .AddClaim("apikey", _videoCallSettings.ApiKey)
            .AddClaim("permissions", new string[2] { "allow_join", "allow_mod" })
            .Encode();
    }
}