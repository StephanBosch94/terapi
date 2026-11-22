using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.ViewModels.Auth;

public class ExternalUserTokenVM
{
    public CTokenProviderEnum CTokenProviderEnum { get; set; }
    public CTokenTypeEnum CTokenTypeEnum { get; set; }
    public string TokenValue { get; set; }
}