using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace ULearn.Be.Api.Common.Security.Extensions;

public static class IisExtensions
{
    public const int MaxRequestLength = 1024 * 1024 * 128;

    public static void IncreaseMaxRequestLength(this IServiceCollection services)
    {
        services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = MaxRequestLength);
        services.Configure<FormOptions>(
            options =>
            {
                options.ValueLengthLimit = MaxRequestLength;
                options.MultipartBodyLengthLimit = MaxRequestLength;
                options.MultipartHeadersLengthLimit = MaxRequestLength;
            }
        );
    }
}
