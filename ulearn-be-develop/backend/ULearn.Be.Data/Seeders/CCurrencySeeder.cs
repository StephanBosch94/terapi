using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CCurrencySeeder
{
    private const CCodetablePrefixesEnum PrefixesEnum = CCodetablePrefixesEnum.CCurrency;

    public static readonly List<CCurrency> Entities = new List<CTmpCurrency>
    {
        new()
        {
            CurrencyEnum = CCurrencyEnum.Czk,
            Symbol = "Kč"
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Eur,
            Symbol = "€",
            IsBase = true
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Usd,
            Symbol = "$"
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Gbp,
            Symbol = "£"
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Rsd,
            Symbol = "Din"
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Pln,
            Symbol = "zł"
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Uah,
            Symbol = "₴"
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Kzt,
            Symbol = "₸"
        },
        new()
        {
            CurrencyEnum = CCurrencyEnum.Rub,
            Symbol = "₽"
        },
    }.Select(x => new CCurrency
    {
        CurrencyId = (int)x.CurrencyEnum,
        IsBase = x.IsBase,
        EnumKey = x.CurrencyEnum.GetEnumKey(),
        NameKey = PrefixesEnum.GetNameKey(x.CurrencyEnum.GetEnumKey()),
        DescriptionKey = PrefixesEnum.GetDescriptionKey(x.CurrencyEnum.GetEnumKey()),
        Symbol = x.Symbol,
        Rate = 0
    }).ToList();
}

public record CTmpCurrency
{
    public CCurrencyEnum CurrencyEnum { get; set; }
    public string Symbol { get; set; }
    public bool IsBase { get; set; }
};