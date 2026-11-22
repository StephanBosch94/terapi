namespace ULearn.Be.Common.Constants;

public static class CommonConstants
{
    public const int TechnicalAdminUserId = 1;
    public const string EnvironmentNameVariable = "ASPNETCORE_ENVIRONMENT";
    public const int FilterViewDefaultTake = 50;
    public const int FilterViewMaxTake = int.MaxValue;
    public const int FilterViewDefaultSkip = 0;
    public const int EmailVerificationTimer = 60;

    public const double MaxRating = 5.0;
    public const int TimeslotDuration = 30;

    public const double SaleForFiveLessons = 0.05;
    public const double SaleForTenLessons = 0.1;
    public const double CommissionRate = 0.21;
    public const double WithdrawCommissionRate = 0.03;

    public const int MinimumTimeForClassRescheduleInHours = 4;
    public const int MinimumTimeForClassCancellationForStudentInHours = 12;
    public const int MinimumTimeForClassReservationForStudentInHours = 12;
    public const int MinimumTimeForPaymentInDays = 7;

    public const string AuthCookieName = "UserLoginCookie";
    public const string BearerCookieName = "BearerCookie";

    public const int PriceRoundDecimalPoints = 2;
    public const double CountryTax = 0.21;

    // Notifications
    public const int WelcomeEmailHourDuration = 24;
    public const string DefaultCurrencyCode = "eur";
    public const string InternalCurrencyCode = "czk";
}