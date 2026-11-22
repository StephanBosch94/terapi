using System.Security.Claims;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslot;
using ULearn.Be.Models.ViewModels.Calendar;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppRecurrentTimeslotService
{
    Task Add(AppRecurrentTimeslotInputVM input, ClaimsPrincipal user);

    Task Delete(
        int recurrentTimeslotId,
        DateTime eventDate,
        ClaimsPrincipal user
    );

    Task<ICollection<CalendarRecurrentTimeslotVM>> Get(DateTime date, string timezone, ClaimsPrincipal user);
}