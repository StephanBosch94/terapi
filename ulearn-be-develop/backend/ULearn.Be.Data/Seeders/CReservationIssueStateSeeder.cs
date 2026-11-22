using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public class CReservationIssueStateSeeder
{
    private const CCodetablePrefixesEnum Pefix = CCodetablePrefixesEnum.CReservationIssueState;

    public static readonly List<CReservationIssueState> Entities = new()
    {
        new CReservationIssueState
        {
            ReservationIssueStateId = (int)CReservationIssueStateEnum.Waiting,
            EnumKey = CReservationIssueStateEnum.Waiting.GetEnumKey(),
            NameKey = Pefix.GetNameKey(CReservationIssueStateEnum.Waiting.GetEnumKey()),
            DescriptionKey = Pefix.GetDescriptionKey(CReservationIssueStateEnum.Waiting.GetEnumKey()),
            IconName = IconConstants.WaitingIcon,
        },
        new CReservationIssueState
        {
            ReservationIssueStateId = (int)CReservationIssueStateEnum.Approved,
            EnumKey = CReservationIssueStateEnum.Approved.GetEnumKey(),
            NameKey = Pefix.GetNameKey(CReservationIssueStateEnum.Approved.GetEnumKey()),
            DescriptionKey = Pefix.GetDescriptionKey(CReservationIssueStateEnum.Approved.GetEnumKey()),
            IconName = IconConstants.ApprovedIcon,
        },
        new CReservationIssueState
        {
            ReservationIssueStateId = (int)CReservationIssueStateEnum.Declined,
            EnumKey = CReservationIssueStateEnum.Declined.GetEnumKey(),
            NameKey = Pefix.GetNameKey(CReservationIssueStateEnum.Declined.GetEnumKey()),
            DescriptionKey = Pefix.GetDescriptionKey(CReservationIssueStateEnum.Declined.GetEnumKey()),
            IconName = IconConstants.DeclinedIcon,
        }
    };
}