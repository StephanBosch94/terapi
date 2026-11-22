using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class EnvConfigurationDataTypeSeeder
{
    public static readonly List<EnvConfigurationDataType> Entities = CodetableUtil
        .GetEnumValues<EnvConfigurationDataTypeEnum>(CCodetablePrefixesEnum.EnvConfigurationDataType)
        .Select(
            x => new EnvConfigurationDataType
            {
                DataTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}