using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CReservationIssueTypeMapper
{
    public static readonly Expression<Func<CReservationIssueType, CReservationIssueTypeVM>> AsVM =
        dataItem => new CReservationIssueTypeVM
        {
            IssueTypeId = dataItem.ReservationIssueTypeId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            HasDescription = dataItem.HasDescription
        };

    public static CReservationIssueTypeVM ToVm(CReservationIssueType dataItem)
    {
        return new CReservationIssueTypeVM
        {
            IssueTypeId = dataItem.ReservationIssueTypeId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            HasDescription = dataItem.HasDescription
        };
    }

    public static IQueryable<CReservationIssueTypeVM> ToApiList(this IQueryable<CReservationIssueType> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}