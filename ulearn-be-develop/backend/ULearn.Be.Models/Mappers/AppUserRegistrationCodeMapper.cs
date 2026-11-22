using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppUserRegistration;

namespace ULearn.Be.Models.Mappers;

public static class AppUserRegistrationCodeMapper
{
    public static readonly Expression<Func<AppUserRegistrationCode, UserRegistrationCodeInfoVM>> AsVM =
        dataItem => new UserRegistrationCodeInfoVM
        {
            Created = dataItem.Created
        };

    public static IQueryable<UserRegistrationCodeInfoVM> ToApiList(this IQueryable<AppUserRegistrationCode> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}