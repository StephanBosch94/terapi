using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Yaspeak.Monitoring.Status.Constants;

namespace Yaspeak.Monitoring.Status.Utils;

public class RetryPolicyUtil
{
    public static AsyncRetryPolicy GetRetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                CommonConstants.NumberOfRetries,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, _, retryCount, _) =>
                {
                    logger.LogError("Retry #{RetryCount} exception: [{ExceptionMessage}]", retryCount,
                        exception.Message);
                }
            );
    }
}