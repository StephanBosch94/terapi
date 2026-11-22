using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CUserAttributeTypeMapper
{
    public static readonly Expression<Func<CUserAttributeType, CUserAttributeTypeVM>> AsVM =
        dataItem => new CUserAttributeTypeVM
        {
            AttributeTypeId = dataItem.UserAttributeTypeId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static CUserAttributeTypeVM ToVm(CUserAttributeType dataItem)
    {
        return new CUserAttributeTypeVM
        {
            AttributeTypeId = dataItem.UserAttributeTypeId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };
    }

    public static IQueryable<CUserAttributeTypeVM> ToApiList(this IQueryable<CUserAttributeType> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}