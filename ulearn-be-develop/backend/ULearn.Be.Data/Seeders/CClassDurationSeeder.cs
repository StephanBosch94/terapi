using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class CClassDurationSeeder
{
    private const CCodetablePrefixesEnum Pefix = CCodetablePrefixesEnum.CClassDuration;

    public static readonly List<CClassDuration> Entities = new()
    {
        new CClassDuration
        {
            ClassDurationId = (int)CClassDurationEnum.Half,
            EnumKey = CClassDurationEnum.Half.GetEnumKey(),
            NameKey = Pefix.GetNameKey(CClassDurationEnum.Half.GetEnumKey()),
            DescriptionKey = Pefix.GetDescriptionKey(CClassDurationEnum.Half.GetEnumKey()),
            ExpectedDuration = 25,
            DurationWithMargin = 30
        },
        new CClassDuration
        {
            ClassDurationId = (int)CClassDurationEnum.Full,
            EnumKey = CClassDurationEnum.Full.GetEnumKey(),
            NameKey = Pefix.GetNameKey(CClassDurationEnum.Full.GetEnumKey()),
            DescriptionKey = Pefix.GetDescriptionKey(CClassDurationEnum.Full.GetEnumKey()),
            ExpectedDuration = 50,
            DurationWithMargin = 60
        },
    };
}