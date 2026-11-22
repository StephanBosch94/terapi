using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CReservationIssueTypeEnum
{
    [EnumKey("technical_problems")] TechnicalProblems = 1,
    [EnumKey("tutor_was_absent")] TutorWasAbsent = 2,
    [EnumKey("other")] Other = 3
}