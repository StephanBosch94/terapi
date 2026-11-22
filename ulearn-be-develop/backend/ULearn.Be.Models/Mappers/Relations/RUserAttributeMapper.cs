using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppUser.Attribute;

namespace ULearn.Be.Models.Mappers.Relations;

public static class RUserAttributeMapper
{
    public static readonly Expression<Func<RUserAttribute, UserAttributeVM>> AsVM =
        dataItem => new UserAttributeVM
        {
            UserId = dataItem.UserId,
            AttributeType = CUserAttributeTypeMapper.ToVm(dataItem.UserAttributeType),
            Value = dataItem.Value
        };

    public static IQueryable<UserAttributeVM> ToApiList(this IQueryable<RUserAttribute> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}