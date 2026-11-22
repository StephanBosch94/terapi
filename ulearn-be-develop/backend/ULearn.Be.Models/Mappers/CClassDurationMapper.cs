using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers;

public static class CClassDurationMapper
{
    private static readonly Expression<Func<CClassDuration, CClassDurationVM>> AsVM =
        dataItem => new CClassDurationVM
        {
            ClassDurationId = dataItem.ClassDurationId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            ExpectedDuration = dataItem.ExpectedDuration,
            DurationWithMargin = dataItem.DurationWithMargin
        };

    public static CClassDurationVM ToVm(CClassDuration dataItem)
    {
        return new CClassDurationVM
        {
            ClassDurationId = dataItem.ClassDurationId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            ExpectedDuration = dataItem.ExpectedDuration,
            DurationWithMargin = dataItem.DurationWithMargin
        };
    }

    public static IQueryable<CClassDurationVM> ToApiList(this IQueryable<CClassDuration> dataItems)
    {
        return dataItems
            .Select(AsVM);
    }
}