using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Be.Providers.Currency.Models;

namespace ULearn.Be.Providers.Currency;

public class CurrencyProvider : ICurrencyProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CurrencyProvider> _logger;

    public CurrencyProvider(IHttpClientFactory httpClientFactory, ILogger<CurrencyProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<CurrencyData> Get()
    {
        _logger.LogInformation("Calling Currency API");
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri =
                new Uri("http://api.exchangeratesapi.io/v1/latest?access_key=70da99e0e52324dc6b03c71fca99cac2"),
            Headers =
            {
                { "Accept", "application/json" }
            }
        };
        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            _logger.LogError("Calling CurrencyAPI get(): Fail, message: {ResponseMessage}",
                responseString);
            throw;
        }

        _logger.LogInformation("Successfully fetch currency data");
        return JsonConvert.DeserializeObject<CurrencyData>(responseString);
    }

    public async Task<LocationInfo> GetLocationInfo(string ip)
    {
        _logger.LogInformation("Calling Location Info API");
        var httpClient = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://ip-api.com/json/{ip}?fields=status,country,currency,query"),
            Headers =
            {
                { "Accept", "application/json" }
            }
        };
        var response = await httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            _logger.LogError("Calling LocationAPI get(): Fail, message: {ResponseMessage}",
                responseString);
            throw;
        }

        _logger.LogInformation("Successfully fetch location data");
        return JsonConvert.DeserializeObject<LocationInfo>(responseString);
    }
}