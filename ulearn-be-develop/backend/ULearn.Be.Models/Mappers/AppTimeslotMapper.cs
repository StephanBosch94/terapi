using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppTimeslot;

namespace ULearn.Be.Models.Mappers;

public static class AppTimeslotMapper
{
    private static readonly Expression<Func<AppTimeslot, AppTimeslotBaseVM>> AsBaseVM =
        dataItem => new AppTimeslotBaseVM
        {
            TimeslotId = dataItem.TimeslotId,
            OwnerId = dataItem.OwnerId,
            FromLocalYear = dataItem.FromLocalYear,
            FromLocalMonth = dataItem.FromLocalMonth,
            FromLocalDay = dataItem.FromLocalDay,
            FromLocalHour = dataItem.FromLocalHour,
            FromLocalMinute = dataItem.FromLocalMinute,
            DurationInMinutes = dataItem.DurationInMinutes,
            IanaTimezone = dataItem.IanaTimezone
        };
    
    public static IQueryable<AppTimeslotBaseVM> ToBaseVMs(this IQueryable<AppTimeslot> dataCollection)
    {
        return dataCollection.Select(AsBaseVM);
    }
}