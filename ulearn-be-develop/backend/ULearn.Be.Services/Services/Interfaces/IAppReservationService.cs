using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppReservationService
{
    Task<AppReservationCreateVM> Create(AppReservationInputVM inputVm, ClaimsPrincipal user);
    Task Edit(int reservationId, AppReservationEditVM inputVm, ClaimsPrincipal user);
    Task<PagedVM<AppReservationVM>> Filter(FilterView<AppReservationFilter> filter, ClaimsPrincipal user);
}