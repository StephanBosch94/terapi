using System.Security.Claims;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Shared.Services.Interfaces;

public interface IAppReservationSharedService
{
    Task<PagedVM<AppReservationVM>> Filter(
        FilterView<AppReservationFilter> filter,
        ClaimsPrincipal user
    );

    Task<Dictionary<int, AppReservationVM>> GetAll(IEnumerable<int> reservationIds, ClaimsPrincipal user);
    bool CanReschedule(ScheduledEntity entity);
    Task CreateTimeslot(int reservationId, int userId);

    Task CancelRemainingReservations(
        int packageId,
        int userId,
        string reasonText,
        bool checkCancellationAmount = true
    );
}