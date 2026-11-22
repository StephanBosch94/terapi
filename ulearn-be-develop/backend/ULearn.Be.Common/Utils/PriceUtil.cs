using ULearn.Be.Common.Constants;

namespace ULearn.Be.Common.Utils;

public static class PriceUtil
{
    public static double GetRoundedPrice(double price)
    {
        return Math.Round(price, CommonConstants.PriceRoundDecimalPoints);
    }

    public static string GetPriceString(double amount)
    {
        return $"{GetRoundedPrice(amount)} Kč";
    }

    private static double GetTutorsPriceCut(double price)
    {
        return Math.Round((1 - CommonConstants.CommissionRate) * price, CommonConstants.PriceRoundDecimalPoints);
    }

    public static (double priceBase, double tax) GetPrice(double totalPrice, bool isCountryTaxPayer)
    {
        var price = isCountryTaxPayer
            ? GetPriceWithoutCountryTax(totalPrice)
            : totalPrice;
        var tax = isCountryTaxPayer
            ? GetCountryTax(price)
            : 0;

        return (price, tax);
    }

    public static double GetOurPriceCut(double price, double? couponSale)
    {
        return price - GetTutorsPriceCut(price) - (couponSale ?? 0);
    }

    public static double GetOurWithdrawPriceFee(double amount)
    {
        return GetRoundedPrice(CommonConstants.WithdrawCommissionRate * amount);
    }

    public static double GetWithdrawAmountAfterFee(double amount)
    {
        return amount - GetOurWithdrawPriceFee(amount);
    }

    public static double GetPriceWithoutCountryTax(double price)
    {
        return GetRoundedPrice((1 - CommonConstants.CountryTax) * price);
    }

    public static double GetCountryTax(double price)
    {
        return price - GetPriceWithoutCountryTax(price);
    }

    public static double GetTrialPrice(double price)
    {
        return (double)price / 2;
    }
}