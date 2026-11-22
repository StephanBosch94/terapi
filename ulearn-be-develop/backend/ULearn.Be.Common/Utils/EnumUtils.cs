using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Common.Utils;

public static class EnumUtils
{
    public static string GetObjectName(Enum type)
    {
        return type.GetAttribute<ObjectNameAttribute>().Name;
    }

    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var enumType = value.GetType();
        var name = Enum.GetName(enumType, value);
        return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
    }
}