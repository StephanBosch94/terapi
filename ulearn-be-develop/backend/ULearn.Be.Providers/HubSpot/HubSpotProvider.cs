using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.HubSpot.Interfaces;
using ULearn.Be.Providers.HubSpot.Models;
using ULearn.Be.Providers.HubSpot.Models.Search.Request;
using ULearn.Be.Providers.HubSpot.Models.Search.Response;
using ULearn.Be.Providers.Utils;

namespace ULearn.Be.Providers.HubSpot;

public class HubSpotProvider : IHubSpotProvider
{
    private const string WebhookUrl = "https://api.hubapi.com/crm/v3/objects/contacts";
    private const string SearchWebhookUrl = "https://api.hubapi.com/crm/v3/objects/contacts/search";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HubSpotSettings _hubSpotSettings;
    private readonly ILogger<HubSpotProvider> _logger;

    public HubSpotProvider(IHttpClientFactory httpClientFactory, ILogger<HubSpotProvider> logger,
        IEnvConfigurationProvider envConfigurationProvider)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _hubSpotSettings = envConfigurationProvider.GetSettings<HubSpotSettings>(EnvSettingsKeyEnum.HubSpotSettings);
    }

    public async Task<HubSpotSearchResultVM> SearchUser(string email, bool throwIfNotSuccess = false)
    {
        _logger.LogInformation("Calling HubSpot API search contact()");
        var httpClient = _httpClientFactory.CreateClient();

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(SearchWebhookUrl),
            Headers =
            {
                Authorization = GetToken(),
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Content = JsonContent.Create(
                new HubSpotSearchIM
                {
                    FilterGroups = new List<HubSpotFilterGroup>
                    {
                        new()
                        {
                            Filters = new List<HubSpotFilter>
                            {
                                new()
                                {
                                    Operator = "EQ",
                                    PropertyName = "email",
                                    Value = email,
                                }
                            }
                        }
                    }
                })
        };
        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("HubSpot API failed to search a contact: {ResponseString}", responseString);
        }

        if (throwIfNotSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("HubSpot API: search contact success");
        var responseObj = response.IsSuccessStatusCode
            ? JsonConvert.DeserializeObject<HubSpotSearchPagedVM>(responseString)
            : null;
        return responseObj?.Results?.FirstOrDefault();
    }

    public async Task<HubSpotUpsertResponseVM> CreateContact(HubSpotInputVM input, bool throwIfNotSuccess = false)
    {
        _logger.LogInformation("Calling HubSpot API create contact()");
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(WebhookUrl),
            Headers =
            {
                Authorization = GetToken(),
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Content = HttpUtils.CreateContent(input)
        };

        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("HubSpot API failed to create a contact: {ResponseString}", responseString);
        }

        if (throwIfNotSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("HubSpot API: create contact success");
        return response.IsSuccessStatusCode
            ? JsonConvert.DeserializeObject<HubSpotUpsertResponseVM>(responseString)
            : null;
    }

    public async Task<HubSpotUpsertResponseVM> UpdateContact(
        string contactId,
        HubSpotInputVM input,
        bool throwIfNotSuccess = false
    )
    {
        _logger.LogInformation("Calling HubSpot API update contact()");
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Patch,
            RequestUri = new Uri(GetUpdateEndpointUrl(contactId)),
            Headers =
            {
                Authorization = GetToken(),
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Content = HttpUtils.CreateContent(input)
        };

        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("HubSpot API failed to update a contact: {ResponseString}", responseString);
        }

        if (throwIfNotSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("HubSpot API: update contact success");
        return response.IsSuccessStatusCode
            ? JsonConvert.DeserializeObject<HubSpotUpsertResponseVM>(responseString)
            : null;
    }

    private static string GetUpdateEndpointUrl(string contactId) => $"{WebhookUrl}/{contactId}";

    private AuthenticationHeaderValue GetToken()
    {
        return new AuthenticationHeaderValue("Bearer", _hubSpotSettings.ApiKey);
    }
}