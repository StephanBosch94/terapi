using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class ForgottenPasswordInputVM
{
    [Required] public string Email { get; set; }
}