using Microsoft.Extensions.Primitives;

namespace ULearn.Be.Services.Services.Interfaces;

public interface ICheckoutService
{
    Task WebhookHandle(string json, StringValues signature);
}