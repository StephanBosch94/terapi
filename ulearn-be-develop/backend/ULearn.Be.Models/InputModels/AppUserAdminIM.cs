using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class AppUserAdminIM
{
    [Required] public int UserId { get; set; }
    public bool CertificationsVerified { get; set; }
    public int OrderNumber { get; set; }
    public bool Excluded { get; set; }
}