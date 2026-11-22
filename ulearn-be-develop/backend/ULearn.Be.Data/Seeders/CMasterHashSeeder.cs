using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CMasterHashSeeder
{
    public static readonly List<CMasterHash> Entities = CodetableUtil
        .GetEnumValues<CMasterHashEnum>(CCodetablePrefixesEnum.CMasterHash)
        .Select(
            x => new CMasterHash
            {
                MasterHashId = x.id,
                EnumKey = x.enumKey,
                NameKey = x.nameKey,
                DescriptionKey = x.descriptionKey,
                Hash = new Guid(((CMasterHashEnum)x.id).GetDescription()),
                TableName = x.enumKey
            }
        ).ToList();
}