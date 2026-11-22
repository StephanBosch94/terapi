using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CReservationPackageStateEnum
{
    [EnumKey("created")] Created = 1,
    [EnumKey("cancelled")] Cancelled = 2
}