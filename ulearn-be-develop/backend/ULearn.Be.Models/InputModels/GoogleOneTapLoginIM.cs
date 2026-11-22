using System.ComponentModel.DataAnnotations;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class GoogleOneTapLoginIM
{
    [Required] public CUserRoleEnum UserRoleId { get; set; }
    [Required] public string LanguageCode { get; set; }
    [Required] public string Token { get; set; }
    [Required] public string TimeZone { get; set; }
}