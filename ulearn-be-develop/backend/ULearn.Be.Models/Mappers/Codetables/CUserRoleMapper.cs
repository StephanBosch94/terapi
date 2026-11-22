using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CUserRoleMapper
{
    public static readonly Expression<Func<CUserRole, CUserRoleVM>> AsVM =
        dataItem => new CUserRoleVM
        {
            UserRoleId = dataItem.UserRoleId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };

    public static CUserRoleVM ToVm(CUserRole dataItem)
    {
        return new CUserRoleVM
        {
            UserRoleId = dataItem.UserRoleId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };
    }

    public static IQueryable<CUserRoleVM> ToApiList(this IQueryable<CUserRole> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}