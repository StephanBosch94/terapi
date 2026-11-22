using ULearn.Be.Common.Attributes;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Common.Utils;

public static class CodetableUtil
{
    public static string GetNameKey(this CCodetablePrefixesEnum prefixesEnum, string enumKey)
    {
        return $"{prefixesEnum.GetEnumKey()}.{enumKey}.name";
    }

    public static string GetDescriptionKey(this CCodetablePrefixesEnum prefixesEnum, string enumKey)
    {
        return $"{prefixesEnum.GetEnumKey()}.{enumKey}.description";
    }

    public static ICollection<(int id, string enumKey, string nameKey, string descriptionKey)> GetEnumValues<T>(
        CCodetablePrefixesEnum prefixesEnum
    )
    {
        var type = typeof(T);
        if (typeof(T).BaseType != typeof(Enum))
        {
            throw new ArgumentException($"{typeof(T).BaseType?.Name} is not enum");
        }

        var result = new List<(int, string, string, string)>();
        foreach (int val in Enum.GetValues(type))
        {
            var memInfo = type.GetMember(type.GetEnumName(val));

            if (memInfo[0]
                    .GetCustomAttributes(typeof(EnumKeyAttribute), false)
                    .FirstOrDefault() is EnumKeyAttribute enumAttribute)
            {
                var enumKey = enumAttribute.EnumKey;
                result.Add((val, enumKey, GetNameKey(prefixesEnum, enumKey), GetDescriptionKey(prefixesEnum, enumKey)));
            }
        }

        return result;
    }
}