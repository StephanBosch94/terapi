using ULearn.Be.Common.Attributes;

// Internal registration WF: EmailVerification(1) => SettingPassword(2) => SetRole(8) => Registration(3) => ProfileValidation(4) => Confirmed(5) | Refused(7)
// External registration WF: SettingRole(8) => Registration(3) => Validation(4) => Confirmed(5) 

namespace ULearn.Be.Data.Enums;

public enum CUserStateEnum
{
    [EnumKey("email_verification")] EmailVerification = 1,
    [EnumKey("password_setup")] PasswordSetup = 2,
    [EnumKey("registration")] Registration = 3,
    [EnumKey("profile_validation")] ProfileValidation = 4,
    [EnumKey("confirmed")] Confirmed = 5,
    [EnumKey("deleted")] Deleted = 6,
    [EnumKey("refused")] Refused = 7,

    [EnumKey("role_setup")] RoleSetup = 8,
}