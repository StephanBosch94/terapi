using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class ResendCodeIM
{
    [Required] public string Email { get; set; }
}