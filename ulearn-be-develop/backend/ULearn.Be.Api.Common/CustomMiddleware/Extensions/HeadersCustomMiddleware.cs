using Microsoft.AspNetCore.Builder;

namespace ULearn.Be.Api.Common.CustomMiddleware.Extensions;

public static class HeadersCustomMiddleware
{
    public static void ConfigureCustomHeadersMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomHeaderMiddleware>();
    }
}