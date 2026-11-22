namespace ULearn.Be.Models.ViewModels.Auth;

public class AuthUserFromExternalSourceVM
{
    public string Email { get; set; }
    public bool VerifiedEmail { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PictureUrl { get; set; }
    public string Locale { get; set; }
    public string Phone { get; set; }
    public ICollection<ExternalUserTokenVM> Tokens { get; set; }
}