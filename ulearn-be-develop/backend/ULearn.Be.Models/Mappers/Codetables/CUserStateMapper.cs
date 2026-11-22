using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CUserStateMapper
{
    public static readonly Expression<Func<CUserState, CUserStateVM>> AsVM =
        dataItem => new CUserStateVM
        {
            UserStateId = dataItem.UserStateId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            CompanyAdminNameKey = dataItem.CompanyAdminNameKey
        };

    public static CUserStateVM ToVm(CUserState dataItem)
    {
        return new CUserStateVM
        {
            UserStateId = dataItem.UserStateId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey,
            CompanyAdminNameKey = dataItem.CompanyAdminNameKey
        };
    }

    public static IQueryable<CUserStateVM> ToApiList(this IQueryable<CUserState> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}