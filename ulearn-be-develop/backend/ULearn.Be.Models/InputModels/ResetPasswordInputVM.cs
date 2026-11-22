using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class ResetPasswordInputVM
{
    [Required] [MinLength(6)] public string NewPassword { get; set; }
}