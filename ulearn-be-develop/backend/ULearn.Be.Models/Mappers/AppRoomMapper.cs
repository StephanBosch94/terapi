using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Models.ViewModels.Room;

namespace ULearn.Be.Models.Mappers;

public static class AppRoomMapper
{
    private static readonly Expression<Func<AppRoom, AppRoomVM>> AsVM =
        dataItem => new AppRoomVM
        {
            RoomId = dataItem.RoomId,
            RoomKey = dataItem.RoomKey,
            ScheduledFromLocalTime = dataItem.ScheduledFromTime,
            ScheduledToLocalTime = dataItem.ScheduledToTime,
            RoomStateId = dataItem.RoomStateId,
            Attender = AppUserMapper.ToCommonVm(dataItem.Attender),
            Host = AppUserMapper.ToCommonVm(dataItem.Host),
        };

    private static readonly Expression<Func<AppRoom, AppReservationRoomVM>> AsAppReservationRoomVM =
        dataItem => new AppReservationRoomVM
        {
            RoomId = dataItem.RoomId,
            RoomKey = dataItem.RoomKey,
            HostId = dataItem.HostId,
            AttenderId = dataItem.AttenderId
        };

    public static IQueryable<AppRoomVM> ToApiList(this IQueryable<AppRoom> dataCollection)
    {
        return dataCollection
            .Include(x => x.Attender)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Host)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsVM);
    }

    public static IQueryable<AppReservationRoomVM> ToReservationRoomList(this IQueryable<AppRoom> dataCollection)
    {
        return dataCollection.Select(AsAppReservationRoomVM);
    }
}