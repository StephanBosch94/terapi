using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Shared.Services.Interfaces;

public interface ICCurrencySharedService
{
    Task UpdateCurrencies();
    Task<CCurrencyVM> GetCurrencyByTimezone(string timezone);
    Task<CCurrencyVM> GetCurrencyByIp(string ip);
}