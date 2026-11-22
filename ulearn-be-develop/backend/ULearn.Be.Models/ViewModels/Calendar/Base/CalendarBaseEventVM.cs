using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace ULearn.Be.Models.ViewModels.Calendar.Base;

[SwaggerSubType(typeof(CalendarRecurrentTimeslotVM))]
[SwaggerSubType(typeof(CalendarReservationVM))]
[SwaggerSubType(typeof(CalendarTimeslotVM))]
public abstract class CalendarBaseEventVM
{
    // ReSharper disable once UnusedMember.Global
    [Required] public abstract string Type { get; }
    [JsonIgnore] public string HostIanaTimezone { get; set; }
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
}