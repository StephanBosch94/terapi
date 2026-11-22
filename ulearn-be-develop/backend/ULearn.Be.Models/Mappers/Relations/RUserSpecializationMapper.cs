using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppUser.Specialization;

namespace ULearn.Be.Models.Mappers.Relations;

public static class RUserSpecializationMapper
{
    public static readonly Expression<Func<RUserSpecialization, AppUserSpecializationVM>> AsVM =
        dataItem => new AppUserSpecializationVM
        {
            UserId = dataItem.UserId,
            SubjectId = dataItem.Specialization.SubjectId,
            SpecializationId = dataItem.SpecializationId,
            EnumKey = dataItem.Specialization.EnumKey,
            NameKey = dataItem.Specialization.NameKey,
            DescriptionKey = dataItem.Specialization.DescriptionKey,
            IconName = dataItem.Specialization.Icon
        };

    public static IQueryable<AppUserSpecializationVM> ToApiList(this IQueryable<RUserSpecialization> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}