using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppUserReservationReviewRepository
{
    Task Add(AppReservationReviewIM input, int userId);
    Task<ILookup<int, AppReservationReviewVM>> GetReviewsByReservationId(IEnumerable<int> reservationIds);
    Task Edit(AppReservationReviewEditIM input, int userId);
}