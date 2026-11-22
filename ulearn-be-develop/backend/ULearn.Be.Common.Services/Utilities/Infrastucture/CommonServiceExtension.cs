using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities.TimeZone;

namespace ULearn.Be.Common.Services.Utilities.Infrastucture;

public static class CommonServiceExtension
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IGuidUtility, GuidUtility>();
        services.AddSingleton<ITimeUtility, TimeUtility>();
        services.AddSingleton<ITimeZoneProvider, UtcTimeZoneProvider>();
    }
}