using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CUserRoleEnum
{
    [EnumKey("admin")] Admin = 1,

    [EnumKey("tutor")] Tutor = 2,

    [EnumKey("student")] Student = 3,
    
    [EnumKey("company")] Company = 4
}