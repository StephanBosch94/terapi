using System.ComponentModel.DataAnnotations;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class AuthExternalSignUpVM : AuthExternalLoginIM
{
    [Required] public CUserRoleEnum UserRoleId { get; set; }
    [Required] public string LanguageCode { get; set; }
}