namespace ULearn.Be.Data.Static.Data;

public static class CTimezoneWithCurrencyStaticData
{
    public static List<CTimezoneWithCurrency> TimezoneWithCurrencies =
        CTimezoneStaticData.Timezones
            .Join(
                CCountryCurrencyStaticData.Currencies,
                timezone => timezone.CountryName.ToLower(),
                currency => currency.CountryName.ToLower(),
                (timezone, currency) => new CTimezoneWithCurrency
                {
                    TimezoneName = timezone.TimezoneName,
                    CurrencyCode = currency.CurrencyCode
                }
            )
            .GroupBy(x => new { x.CurrencyCode, x.TimezoneName })
            .Select(x => x.First())
            .ToList();

    public static Dictionary<string, string> CurrencyCodeByTimezone = TimezoneWithCurrencies
        .GroupBy(x => x.TimezoneName)
        .ToDictionary(x => x.Key, x => x.First().CurrencyCode, StringComparer.OrdinalIgnoreCase);
}

public record CTimezoneWithCurrency
{
    public string TimezoneName { get; set; }
    public string CurrencyCode { get; set; }
}