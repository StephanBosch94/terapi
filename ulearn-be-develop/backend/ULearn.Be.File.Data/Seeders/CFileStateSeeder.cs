using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Enums;
using ULearn.Be.File.Data.Enums;

namespace ULearn.Be.File.Data.Seeders;

public static class CFileStateSeeder
{
    public static readonly List<CFileState> Entities = CodetableUtil
        .GetEnumValues<CFileStateEnum>(CCodetablePrefixesEnum.CFileState)
        .Select(
            x => new CFileState
            {
                FileStateId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey
            }
        ).ToList();
}