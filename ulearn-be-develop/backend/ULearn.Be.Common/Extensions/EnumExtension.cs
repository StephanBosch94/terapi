using System.ComponentModel;
using System.Globalization;

namespace ULearn.Be.Common.Extensions;

public static class EnumExtension
{
    public static string GetDescription<T>(this T e) where T : IConvertible
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
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
            {
                return descriptionAttribute.Description;
            }
        }

        return null; // could also return string.Empty
    }
}