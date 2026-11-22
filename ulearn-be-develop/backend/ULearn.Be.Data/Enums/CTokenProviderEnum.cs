using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CTokenProviderEnum
{
    [EnumKey("google_api")] GoogleApi = 1,
    [EnumKey("seznam_api")] SeznamApi = 2,
}