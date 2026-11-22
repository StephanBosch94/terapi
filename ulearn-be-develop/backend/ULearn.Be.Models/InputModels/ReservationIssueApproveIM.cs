using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.InputModels;

public class ReservationIssueApproveIM : ReservationIssueDecisionIM
{
    [Required] public double StudentPercentage { get; set; }
}