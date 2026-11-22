using System.ComponentModel;
using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CAuthenticationMethodEnum
{
    [Description("Internal Registration")] [EnumKey("internal")]
    Internal = 1,

    [Description("Google")] [EnumKey("google")]
    Google = 2,

    [Description("Facebook")] [EnumKey("facebook")]
    Facebook = 3,

    [Description("Seznam")] [EnumKey("seznam")]
    Seznam = 4,

    [Description("Google One Tap")] [EnumKey("google_one_tap")]
    GoogleOneTap = 5,
}