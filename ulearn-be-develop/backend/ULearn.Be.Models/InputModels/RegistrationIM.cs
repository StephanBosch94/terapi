using System.ComponentModel.DataAnnotations;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class RegistrationIM
{
    [Required] public string Email { get; set; }

    [Required] public CUserRoleEnum UserRoleId { get; set; }

    [Required] public string LanguageCode { get; set; }
    
    [Required] public string Timezone { get; set; }
}