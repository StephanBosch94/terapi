using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Email;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.Notifications.Email.Interfaces;
using ULearn.Be.Providers.Utils;

namespace ULearn.Be.Providers.Notifications.Email;

public class EmailProvider : IEmailProvider
{
    private readonly EmailSettings _emailSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EmailProvider> _logger;

    public EmailProvider(ILogger<EmailProvider> logger, IEnvConfigurationProvider provider,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _emailSettings = provider.GetSettings<EmailSettings>(EnvSettingsKeyEnum.EmailSettings);
    }

    public async Task SendEmail(EmailDataInputVM input, bool throwIfNotSuccess = false)
    {
        _logger.LogInformation("Calling Email API sendEmail()");
        var httpClient = _httpClientFactory.CreateClient();
        var payload = new EmailDataInputFullVm
        {
            From = _emailSettings.SenderEmail,
            To = input.To,
            TemplateAlias = input.EmailTemplateEnum.GetEnumKey(),
            TemplateModel = GetContent(input)
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.postmarkapp.com/email/withTemplate"),
            Headers =
            {
                { "accept", "application/json" },
                { "X-Postmark-Server-Token", _emailSettings.ApiKey }
            },
            Content = HttpUtils.CreateContent(payload)
        };
        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage =
                $"Calling Email API sendEmail(): Fail, message: {responseString}. Payload: [{JsonConvert.SerializeObject(payload)}]";

            _logger.LogError("{Error}", errorMessage);
            if (throwIfNotSuccess)
            {
                throw new Exception(errorMessage);
            }
        }

        _logger.LogInformation("Successfully send a message");
    }

    private static Dictionary<string, object> GetContent(EmailDataInputVM input)
    {
        var baseContent = input.Params != null && input.Params.Any()
            ? new Dictionary<string, object> { { input.LangCode, input.Params } }
            : new Dictionary<string, object> { { input.LangCode, true } };

        var content = baseContent
            .Concat(input.Params ?? new Dictionary<string, object>())
            .GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.First().Value);

        if (input.Params?.ContainsKey(CEmailDataEnum.Student.GetDescription()) is true)
        {
            return new Dictionary<string, object>
                {
                    { CEmailDataEnum.Student.GetDescription(), content }
                }.Concat(input.Params)
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First().Value);
        }

        if (input.Params?.ContainsKey(CEmailDataEnum.Tutor.GetDescription()) is true)
        {
            return new Dictionary<string, object>
                {
                    { CEmailDataEnum.Tutor.GetDescription(), content }
                }.Concat(input.Params)
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First().Value);
        }

        return content;
    }
}