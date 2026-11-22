using Microsoft.AspNetCore.Builder;

namespace ULearn.Be.Api.Common.CustomMiddleware.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}