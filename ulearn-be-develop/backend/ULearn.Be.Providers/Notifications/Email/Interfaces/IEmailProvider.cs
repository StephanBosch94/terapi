using ULearn.Be.Models.ViewModels.Email;

namespace ULearn.Be.Providers.Notifications.Email.Interfaces;

public interface IEmailProvider
{
    Task SendEmail(EmailDataInputVM input, bool throwIfNotSuccess = false);
}