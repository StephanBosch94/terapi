using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CCurrencyEnum
{
    [EnumKey("czk")] Czk = 1,
    [EnumKey("eur")] Eur = 2,
    [EnumKey("usd")] Usd = 3,
    [EnumKey("gbp")] Gbp = 4,
    [EnumKey("rsd")] Rsd = 5,
    [EnumKey("pln")] Pln = 6,
    [EnumKey("uah")] Uah = 7,
    [EnumKey("kzt")] Kzt = 8,
    [EnumKey("rub")] Rub = 9
}