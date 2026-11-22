using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Providers.Auth;
using ULearn.Be.Providers.Auth.Interfaces;
using ULearn.Be.Providers.Currency;
using ULearn.Be.Providers.HubSpot;
using ULearn.Be.Providers.HubSpot.Interfaces;
using ULearn.Be.Providers.Notifications.Email;
using ULearn.Be.Providers.Notifications.Email.Interfaces;
using ULearn.Be.Providers.Notifications.Slack;
using ULearn.Be.Providers.Notifications.Slack.Inrefaces;
using ULearn.Be.Providers.RemoteFileSystem;
using ULearn.Be.Providers.RemoteFileSystem.Interfaces;
using ULearn.Be.Providers.Video;
using ULearn.Be.Providers.Video.Interfaces;

namespace ULearn.Be.Providers.Infrastructure;

public static class ProvidersExtension
{
    public static void AddProviders(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSingleton<IGoogleApiProvider, GoogleApiProvider>();
        services.AddSingleton<IFacebookProvider, FacebookApiProvider>();
        services.AddSingleton<ISeznamApiProvider, SeznamApiProvider>();
        services.AddSingleton<IEmailProvider, EmailProvider>();
        services.AddSingleton<IRemoteFileSystemProvider, RemoteFileSystemProvider>();
        services.AddSingleton<IVideoCallProvider, VideoCallProvider>();
        services.AddSingleton<ISlackProvider, SlackProvider>();
        services.AddSingleton<IHubSpotProvider, HubSpotProvider>();
        services.AddSingleton<ICurrencyProvider, CurrencyProvider>();

        services.AddScoped<IInternalAuthProvider, InternalAuthProvider>();
        services.AddHttpClient();
    }
}