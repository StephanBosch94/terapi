using ULearn.Be.Providers.Currency.Models;

namespace ULearn.Be.Providers.Currency;

public interface ICurrencyProvider
{
    Task<CurrencyData> Get();
    Task<LocationInfo> GetLocationInfo(string ip);
}