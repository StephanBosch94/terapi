using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class AuthInternalLoginIM
{
    [Required] public string Login { get; set; }

    [Required] public string Password { get; set; }
}