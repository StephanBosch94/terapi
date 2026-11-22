using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Enums;
using ULearn.Be.File.Data.Enums;

namespace ULearn.Be.File.Data.Seeders;

public class CCompressionTypeSeeder
{
    public static readonly List<CCompressionType> Entities = CodetableUtil
        .GetEnumValues<CCompressionTypeEnum>(CCodetablePrefixesEnum.CFileState)
        .Select(
            x => new CCompressionType
            {
                CompressionTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}