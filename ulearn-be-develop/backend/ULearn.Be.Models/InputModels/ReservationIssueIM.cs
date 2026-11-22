using System.ComponentModel.DataAnnotations;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class ReservationIssueIM
{
    [Required] public int ReservationId { get; set; }
    [Required] public CReservationIssueTypeEnum IssueTypeId { get; set; }
    public string Description { get; set; }
}