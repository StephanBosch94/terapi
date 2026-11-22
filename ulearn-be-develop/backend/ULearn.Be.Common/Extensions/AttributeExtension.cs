using System.Globalization;
using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Common.Extensions;

public static class AttributeExtension
{
    public static string GetEnumKey<T>(this T e) where T : IConvertible
    {
        if (e is not Enum)
        {
            return null; // could also return string.Empty
        }

        var type = e.GetType();
        var values = Enum.GetValues(type);

        foreach (int val in values)
        {
            if (val != e.ToInt32(CultureInfo.InvariantCulture))
            {
                continue;
            }

            var memInfo = type.GetMember(type.GetEnumName(val));

            if (memInfo[0]
                    .GetCustomAttributes(typeof(EnumKeyAttribute), false)
                    .FirstOrDefault() is EnumKeyAttribute descriptionAttribute)
            {
                return descriptionAttribute.EnumKey;
            }
        }

        return null; // could also return string.Empty
    }


    public static TAttributeType GetAttribute<TEnum, TAttributeType>(this TEnum e)
        where TEnum : IConvertible where TAttributeType : class
    {
        if (e is not Enum)
        {
            return null; // could also return string.Empty
        }

        var type = e.GetType();
        var values = Enum.GetValues(type);

        foreach (int val in values)
        {
            if (val != e.ToInt32(CultureInfo.InvariantCulture))
            {
                continue;
            }

            var memInfo = type.GetMember(type.GetEnumName(val));
            var attribute = memInfo[0]
                .GetCustomAttributes(typeof(TAttributeType), false)
                .FirstOrDefault();
            if (attribute is TAttributeType tAttribute)
            {
                return tAttribute;
            }
        }

        return null;
    }
}