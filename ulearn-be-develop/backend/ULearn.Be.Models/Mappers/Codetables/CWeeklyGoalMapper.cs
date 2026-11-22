using System.Linq.Expressions;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Models.Mappers.Codetables;

public static class CWeeklyGoalMapper
{
    public static readonly Expression<Func<CWeeklyGoal, CWeeklyGoalVM>> AsVM =
        dataItem => new CWeeklyGoalVM
        {
            WeeklyGoalId = dataItem.WeeklyGoalId,
            EnumKey = dataItem.EnumKey,
            NameKey = dataItem.NameKey,
            DescriptionKey = dataItem.DescriptionKey,
        };

    public static CWeeklyGoalVM ToVm(CWeeklyGoal dataItem)
    {
        return new CWeeklyGoalVM
        {
            WeeklyGoalId = dataItem.WeeklyGoalId,
            EnumKey = dataItem.EnumKey,
            DescriptionKey = dataItem.DescriptionKey,
            NameKey = dataItem.NameKey
        };
    }

    public static IQueryable<CWeeklyGoalVM> ToApiList(this IQueryable<CWeeklyGoal> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}