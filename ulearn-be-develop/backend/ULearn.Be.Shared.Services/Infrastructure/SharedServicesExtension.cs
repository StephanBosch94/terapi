using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services.Infrastructure;

public static class SharedServicesExtension
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<ICCurrencySharedService, CCurrencySharedService>();
        services.AddScoped<ICalendarSharedService, CalendarSharedService>();
        services.AddScoped<IAppReservationSharedService, AppReservationSharedService>();
        services.AddScoped<IAppUserSharedService, AppUserSharedService>();
        services.AddScoped<IAppUserWalletSharedService, AppUserWalletSharedService>();
        services.AddScoped<INotificationSharedService, NotificationSharedService>();
        services.AddScoped<IHubspotSharedService, HubspotSharedService>();
        services.AddScoped<IAppCouponSharedService, AppCouponSharedService>();
    }
}