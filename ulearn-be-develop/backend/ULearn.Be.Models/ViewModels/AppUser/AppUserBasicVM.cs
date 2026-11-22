using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Models.ViewModels.AppUser;

public record AppUserBasicVM : AppUserBaseVM
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public string Phone { get; set; }
    public int StateId { get; set; }
    public double? HourRate { get; set; }
    public string PlaceOfBusiness { get; set; }
    public string CompanyName { get; set; }
    public string BillingEmail { get; set; }
    public int CountOfUnreadMessages { get; set; }
    public int CountOfUnreadNotifications { get; set; }
    public int? InviterId { get; set; }
    public int? SubjectId { get; set; }
    public bool IsCountryTaxPayer => !string.IsNullOrEmpty(Dic);

    public new string GetUserName() => UserUtil.GetUserName(FirstName, LastName, Login);
}