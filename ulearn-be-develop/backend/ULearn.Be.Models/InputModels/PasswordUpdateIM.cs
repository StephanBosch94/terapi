using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class PasswordUpdateIM
{
    [Required] [MinLength(6)] public string NewPassword { get; set; }
}