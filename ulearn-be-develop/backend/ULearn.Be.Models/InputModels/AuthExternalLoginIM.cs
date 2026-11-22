using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class AuthExternalLoginIM
{
    [Required] public string RedirectUrl { get; set; }
    [Required] public string TimeZone { get; set; }
    [Required] public string AuthenticationCode { get; set; }
}