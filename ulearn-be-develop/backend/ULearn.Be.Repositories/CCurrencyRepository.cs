using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Providers.Currency.Models;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class CCurrencyRepository : BaseService, ICCurrencyRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public CCurrencyRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task Update(CurrencyData currencies)
    {
        var now = _timeUtility.GetUtcNow();
        var allDbCurrencies = await Query(false).ToListAsync();

        foreach (var dbCurrency in allDbCurrencies.Where(x => currencies.Rates.ContainsKey(x.EnumKey)))
        {
            dbCurrency.Rate = currencies.Rates[dbCurrency.EnumKey];
            dbCurrency.LastSync = now;
        }

        await _db.SaveChangesAsync();
    }

    public async Task<double> ConvertToInternalCurrency(double price, string currencyCode)
    {
        var currency = await GetByCode(currencyCode);
        var internalCurrency = await GetInternalCurrency();
        return PriceUtil.GetRoundedPrice(price * internalCurrency.Rate / currency.Rate);
    }

    public async Task<double> ConvertToUserCurrency(double price, string currencyCode)
    {
        var currency = await GetByCode(currencyCode);
        var internalCurrency = await GetInternalCurrency();
        return PriceUtil.GetRoundedPrice(price * currency.Rate / internalCurrency.Rate);
    }

    public async Task<CCurrencyVM> GetByCode(string currencyCode, bool throwIfNotFound = true)
    {
        var currencyLower = currencyCode?.ToLower() ?? "";
        var currency = await Query()
            .Where(x => x.EnumKey == currencyLower)
            .ToApiList()
            .FirstOrDefaultAsync();

        if (throwIfNotFound && currency == null)
        {
            throw new NotFoundException($"Could not find a currency with code [{currencyCode}]");
        }

        return currency;
    }

    public Task<CCurrencyVM> GetInternalCurrency()
    {
        return Query()
            .Where(x => x.EnumKey == CommonConstants.InternalCurrencyCode)
            .ToApiList()
            .FirstOrDefaultAsync();
    }

    private IQueryable<CCurrency> Query(bool asNoTracking = true)
    {
        var query = _db.CCurrencies;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}