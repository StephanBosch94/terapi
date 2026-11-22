using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.Mappers;

public static class AppReservationReviewMapper
{
    private static readonly Expression<Func<AppReservationReview, AppReservationReviewVM>> AsVM =
        dataItem => new AppReservationReviewVM
        {
            ReservationId = dataItem.ReservationId,
            ReservationReviewId = dataItem.ReservationReviewId,
            Created = dataItem.Created,
            Value = dataItem.Value
        };

    public static IQueryable<AppReservationReviewVM> ToApiList(this IQueryable<AppReservationReview> dataCollection)
    {
        return dataCollection.Select(AsVM);
    }
}