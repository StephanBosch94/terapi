using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Yaspeak.Monitoring.Status.Models;
using Yaspeak.Monitoring.Status.Utils;

namespace Yaspeak.Monitoring.Status;

public class FeWarmup
{
    private static async Task SendRequest(string endpoint)
    {
        using var httpClient = new HttpClient();
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(endpoint),
            Headers = { { "accept", "text/html" } }
        };

        await httpClient.SendAsync(httpRequest);
    }

    private static async Task SendRequestAndLogIfFail(string endpoint, ILogger log, EnvironmentEnum environment)
    {
        try
        {
            await RetryPolicyUtil.GetRetryPolicy(log).ExecuteAsync(async () => { await SendRequest(endpoint); });
        }
        catch (Exception e)
        {
            await SlackUtils.SendSlackMessage(e.Message, endpoint, log, environment);
        }

        log.LogInformation("Url [{Endpoint}] OK", endpoint);
    }

    [FunctionName("FeWarmup")]
    public static async Task Run([TimerTrigger("*/3 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation("Start sending request");

        await SendRequestAndLogIfFail("https://int.yaspeak.com/tutor/english/5/Alice%20Anderson", log,
            EnvironmentEnum.Int);
        await SendRequestAndLogIfFail("https://www.yaspeak.com/tutor/english/9/Mirjana%20%20Sredojevic%20", log,
            EnvironmentEnum.Prod);
    }
}