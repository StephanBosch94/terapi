using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Static.Data;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Providers.Currency;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services;

public class CCurrencySharedService : ICCurrencySharedService
{
    private readonly ICCurrencyRepository _cCurrencyRepository;
    private readonly ICurrencyProvider _currencyProvider;

    public CCurrencySharedService(ICurrencyProvider currencyProvider, ICCurrencyRepository cCurrencyRepository)
    {
        _currencyProvider = currencyProvider;
        _cCurrencyRepository = cCurrencyRepository;
    }

    public async Task UpdateCurrencies()
    {
        var currencies = await _currencyProvider.Get();
        await _cCurrencyRepository.Update(currencies);
    }

    public Task<CCurrencyVM> GetCurrencyByTimezone(string timezone)
    {
        var currencyCode = CTimezoneWithCurrencyStaticData.CurrencyCodeByTimezone.TryGetValue(timezone, out var value)
            ? value
            : CommonConstants.DefaultCurrencyCode;

        return GetCurrencyWithFallback(currencyCode);
    }

    public async Task<CCurrencyVM> GetCurrencyByIp(string ip)
    {
        var locationData = await _currencyProvider.GetLocationInfo(ip);
        return await GetCurrencyWithFallback(locationData?.Currency ?? CommonConstants.DefaultCurrencyCode);
    }

    private async Task<CCurrencyVM> GetCurrencyWithFallback(string currencyCode)
    {
        return await _cCurrencyRepository.GetByCode(currencyCode, false)
               ?? await _cCurrencyRepository.GetByCode(CommonConstants.DefaultCurrencyCode);
    }
}