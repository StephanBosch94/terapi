using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CFileTypeMapper
{
    public static readonly Expression<Func<CFileType, CFileTypeVM>> AsVM =
        dataItem => new CFileTypeVM
        {
            FileTypeId = dataItem.FileTypeId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static IQueryable<CFileTypeVM> ToApiList(this IQueryable<CFileType> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}