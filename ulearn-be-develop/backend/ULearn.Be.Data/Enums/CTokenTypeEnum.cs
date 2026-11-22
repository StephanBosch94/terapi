using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CTokenTypeEnum
{
    [EnumKey("refresh_token")] RefreshToken = 1,
    [EnumKey("id_token")] IdToken = 2,
    [EnumKey("access_token")] AccessToken = 3,
    [EnumKey("bearer")] InternalBearer = 4
}