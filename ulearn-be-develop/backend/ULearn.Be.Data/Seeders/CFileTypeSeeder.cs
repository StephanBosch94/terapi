using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;
using ULearn.Be.Common.Utils;

namespace ULearn.Be.Data.Seeders;

public static class CFileTypeSeeder
{
    public static readonly List<CFileType> Entities = CodetableUtil
        .GetEnumValues<CFileTypeEnum>(CCodetablePrefixesEnum.CFileType)
        .Select(
            x => new CFileType
            {
                FileTypeId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}