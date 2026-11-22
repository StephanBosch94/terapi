namespace ULearn.Be.Providers.Configuration.Models.Settings;

public class StripeSettings
{
    public string PublishableKey { get; set; }
    public string SecretKey { get; set; }
    public string WebhookKey { get; set; }
    public string ReservationSuccessUrl { get; set; }
    public string ReservationCancelUrl { get; set; }
    public string MoneyRefillSuccessUrl { get; set; }
    public string MoneyRefillCancelUrl { get; set; }
}