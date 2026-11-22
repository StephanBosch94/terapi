using ULearn.Be.Common.Attributes;
using ULearn.Be.Common.Constants;

namespace ULearn.Be.Data.Enums;

public enum CSpecializationEnum
{
    [EnumKey("support_education")] [IconName(IconConstants.Education)]
    SupportEducation = 1,

    [EnumKey("boost_career")] [IconName(IconConstants.Career)]
    BoostCareer = 2,

    [EnumKey("move_abroad")] [IconName(IconConstants.MoveAbroad)]
    MoveAbroad = 3,

    [EnumKey("connect_with_people")] [IconName(IconConstants.ConnectPeople)]
    ConnectWithPeople = 4,

    [EnumKey("language_for_kids")] [IconName(IconConstants.Kids)]
    LanguageForKids = 5,

    [EnumKey("self_improvement")] [IconName(IconConstants.Productivity)]
    SelfImprovement = 6,

    [EnumKey("just_for_fun")] [IconName(IconConstants.Fun)]
    JustForFun = 7
}