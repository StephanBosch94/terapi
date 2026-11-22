using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.Notifications.Slack.Enums;
using ULearn.Be.Providers.Notifications.Slack.Inrefaces;
using ULearn.Be.Providers.Utils;

namespace ULearn.Be.Providers.Notifications.Slack;

public class SlackProvider : ISlackProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SlackProvider> _logger;
    private readonly SlackSettings _slackSettings;

    public SlackProvider(
        IEnvConfigurationProvider envConfigurationProvider,
        IHttpClientFactory httpClientFactory,
        ILogger<SlackProvider> logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _slackSettings = envConfigurationProvider.GetSettings<SlackSettings>(EnvSettingsKeyEnum.SlackSettings);
    }

    public async Task SendNotification(
        SlackInputVM input,
        SlackChannelPriorityEnum priorityEnum = SlackChannelPriorityEnum.High,
        bool throwIfNotSuccess = false
    )
    {
        _logger.LogInformation("Calling Slack API sendNotification()");
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = GetChannelUri(priorityEnum),
            Headers = { { "accept", "application/json" } },
            Content = HttpUtils.CreateContent(GetMessage(input))
        };

        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Slack API: failed to send a notification: {ResponseString}", responseString);
        }

        if (throwIfNotSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("Slack API: success");
    }

    private Uri GetChannelUri(SlackChannelPriorityEnum priorityEnum)
    {
        var url = priorityEnum == SlackChannelPriorityEnum.High
            ? _slackSettings.WebhookEndpoint
            : _slackSettings.LowPriorityWebhookEndpoint;
        return new Uri(url);
    }

    private static SlackMessageVm GetMessage(SlackInputVM input)
    {
        var roleEnum = (CUserRoleEnum)input.RoleId;
        return new SlackMessageVm
        {
            Text = $"[{input.UserName}] [{roleEnum.ToString()}] [{input.Email}]: {input.Text}"
        };
    }
}

public class SlackMessageVm
{
    [JsonProperty("text")] public string Text { get; set; }
}