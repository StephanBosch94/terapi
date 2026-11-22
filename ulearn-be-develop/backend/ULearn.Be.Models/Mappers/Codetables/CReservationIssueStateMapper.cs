using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CReservationIssueStateMapper
{
    public static CReservationIssueStateVM ToVm(CReservationIssueState dataItem)
    {
        return new CReservationIssueStateVM
        {
            ReservationIssueStateId = dataItem.ReservationIssueStateId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            IconName = dataItem.IconName
        };
    }
}