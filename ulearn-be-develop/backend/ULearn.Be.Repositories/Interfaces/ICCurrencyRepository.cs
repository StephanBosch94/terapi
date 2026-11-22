using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Providers.Currency.Models;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICCurrencyRepository
{
    Task<double> ConvertToInternalCurrency(double price, string currencyCode);
    Task<double> ConvertToUserCurrency(double price, string currencyCode);
    Task Update(CurrencyData currencies);
    Task<CCurrencyVM> GetByCode(string currencyCode, bool throwIfNotFound = true);
}