using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class UserAttributeIM
{
    public CUserAttributeTypeEnum CUserAttributeTypeEnum { get; set; }
    public string UserAttributeValue { get; set; }
}