using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Yaspeak.Monitoring.Status.Constants;
using Yaspeak.Monitoring.Status.Models;

namespace Yaspeak.Monitoring.Status.Utils;

public static class SlackUtils
{
    private const int MaxErrorLength = 100;

    private static SlackMessageVM GetErrorMessage(string endpoint, string errorMessage)
    {
        return new SlackMessageVM
        {
            Text = $"API [{endpoint}] is not responding: [{GeTruncatedErrorMessage(errorMessage)}]"
        };
    }

    private static string GeTruncatedErrorMessage(string errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            return errorMessage;
        }

        return errorMessage.Length > MaxErrorLength
            ? $"{string.Join("", errorMessage.Take(MaxErrorLength))}..."
            : errorMessage;
    }

    public static async Task SendSlackMessage(
        string errorMessage,
        string endpoint,
        ILogger log,
        EnvironmentEnum environmentEnum
    )
    {
        using var httpClient = new HttpClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(
                environmentEnum == EnvironmentEnum.Int
                    ? CommonConstants.SlackIntEndpoint
                    : CommonConstants.SlackProdEndpoint
            ),
            Headers = { { "accept", "application/json" } },
            Content = new StringContent(
                JsonConvert.SerializeObject(GetErrorMessage(endpoint, errorMessage)),
                Encoding.UTF8,
                "application/json"
            )
        };

        var response = await httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();
    }
}