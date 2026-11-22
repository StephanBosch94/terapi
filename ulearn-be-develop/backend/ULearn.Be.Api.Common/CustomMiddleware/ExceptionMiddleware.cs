using System.Net;
using Microsoft.AspNetCore.Http;
using Serilog;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Responses;

namespace ULearn.Be.Api.Common.CustomMiddleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    public static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var outParameters = new Dictionary<string, string>();
        context.Response.ContentType = "application/json";
        var isError = false;

        switch (exception)
        {
            case BadRequestException badRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                outParameters = badRequestException.Parameters;
                break;
            case ConflictException conflictException:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                outParameters = conflictException.Parameters;
                break;
            case ForbiddenException forbiddenException:
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                outParameters = forbiddenException.Parameters;
                break;
            case NotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                outParameters = notFoundException.Parameters;
                break;
            case NotImplementedException _:
                context.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
                break;
            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                outParameters = unauthorizedException.Parameters;
                break;
            case UnprocessableEntityException unprocessableEntityException:
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                outParameters = unprocessableEntityException.Parameters;
                break;
            default:
                isError = true;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        if (isError)
        {
            Log.Error(exception, "Exception: {@Message}", exception.Message);
        }
        else
        {
            Log.Warning(exception, "Exception: {@Message}", exception.Message);
        }

        return context.Response.WriteAsync(
            new ErrorResponse
            {
                Status = context.Response.StatusCode,
                Message = exception.Message,
                Parameters = outParameters
            }.ToString() ?? string.Empty
        );
    }
}