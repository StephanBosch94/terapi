using ULearn.Be.Models.ViewModels.AppUser;

namespace ULearn.Be.Models.ViewModels.Auth;

public record AuthTokenVM : AppUserBasicVM
{
    public string TokenType { get; set; } = "Bearer";
    public string Token { get; set; }
}