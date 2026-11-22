using System.ComponentModel.DataAnnotations.Schema;

namespace ULearn.Be.Data.Codetables.Base;

public class ScheduledEntity
{
    [Column("start_time")] public DateTime StartTime { get; set; }
    [Column("end_time")] public DateTime EndTime { get; set; }
}