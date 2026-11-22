namespace Yaspeak.Be.Scheduler.Jobs;

public static class JobsExtension
{
    public static void AddJobs(this IServiceCollection services)
    {
        services.AddScoped<AppTransactionScheduledJob>();
        services.AddScoped<NotificationScheduledJob>();
        services.AddScoped<AppCouponScheduledJob>();
        services.AddScoped<CurrencyFetchScheduledJob>();
    }
}