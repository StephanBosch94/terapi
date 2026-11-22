using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Yaspeak.Monitoring.Status.Models;
using Yaspeak.Monitoring.Status.Utils;

namespace Yaspeak.Monitoring.Status
{
    public static class BeMonitoring
    {
        private static async Task SendBeRequest(
            string endpoint
        )
        {
            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(endpoint),
                Headers = { { "accept", "application/json" } }
            };

            var response = await httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
        }

        private static async Task SendRequestAndLogIfFail(string endpoint, ILogger log, EnvironmentEnum environment)
        {
            try
            {
                await RetryPolicyUtil.GetRetryPolicy(log).ExecuteAsync(async () => { await SendBeRequest(endpoint); });
            }
            catch (Exception e)
            {
                await SlackUtils.SendSlackMessage(e.Message, endpoint, log, environment);
            }

            log.LogInformation("Url [{Endpoint}] OK", endpoint);
        }

        [FunctionName("BeMonitoring")]
        public static async Task Run([TimerTrigger("* * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("Start sending request");

            await SendRequestAndLogIfFail("https://beapi.int.yaspeak.com/api/health/check", log,
                EnvironmentEnum.Int);
            await SendRequestAndLogIfFail("https://prod.beapi.yaspeak.com/api/health/check", log,
                EnvironmentEnum.Prod);
        }
    }
}