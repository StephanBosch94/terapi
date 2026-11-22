using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class UserTokenIM
{
    public CTokenProviderEnum CTokenProviderEnum { get; set; }
    public CTokenTypeEnum CTokenTypeEnum { get; set; }
    public string TokenValue { get; set; }
}