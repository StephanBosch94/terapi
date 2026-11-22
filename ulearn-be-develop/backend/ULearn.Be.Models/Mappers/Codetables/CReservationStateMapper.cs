using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CReservationStateMapper
{
    public static readonly Expression<Func<CReservationState, CReservationStateVM>> AsVM =
        dataItem => new CReservationStateVM
        {
            ReservationStateId = dataItem.ReservationStateId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static IQueryable<CReservationStateVM> ToApiList(this IQueryable<CReservationState> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}