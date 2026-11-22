using Stripe;

namespace ULearn.Be.Api.Configuration;

public static class StripeConfigurationExtension
{
    public static void AddStripeConfiguration(this IServiceCollection services, string secretKey)
    {
        StripeConfiguration.ApiKey = secretKey;
    }
}