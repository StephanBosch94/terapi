namespace ULearn.Be.Common.Attributes;

public class EnumKeyAttribute : Attribute
{
    public EnumKeyAttribute(string enumKey)
    {
        EnumKey = enumKey;
    }

    public string EnumKey { get; }
}