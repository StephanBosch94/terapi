using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Models.ViewModels.Room;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppRoomRepository : BaseService, IAppRoomRepository
{
    private readonly RepositoryContext _db;

    public AppRoomRepository(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<AppRoomVM> GetByKey(string key, bool throwIfNotFound = true)
    {
        var result = await Query()
            .Where(x => x.RoomKey == key)
            .ToApiList()
            .FirstOrDefaultAsync();

        if (throwIfNotFound && result == null)
        {
            throw new NotFoundException($"Could not find a room with key [{key}]");
        }

        return result;
    }

    public Task<bool> HasRoomAccess(int roomId, int currentUserId)
    {
        return Query()
            .Where(x => x.RoomId == roomId)
            .Where(x => x.HostId == currentUserId || x.AttenderId == currentUserId)
            .AnyAsync();
    }

    public Task<AppRoom> GetRawDbEntity(int roomId, bool throwIfNotFound = true)
    {
        return Query()
            .Where(x => x.RoomId == roomId)
            .FirstOrDefaultAsync();
    }

    public async Task Update(int roomId, AppRoomUpdateVM input)
    {
        var entity = await Query(false)
            .Where(x => x.RoomId == roomId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(roomId, entity);

        entity.RoomStateId = (int)input.RoomStateId;

        await _db.SaveChangesAsync();
    }

    public async Task<AppReservationRoomVM> GetById(int roomId, bool throwIfNotFound = true)
    {
        var room = await Query()
            .Where(x => x.RoomId == roomId)
            .ToReservationRoomList()
            .FirstOrDefaultAsync();

        if (throwIfNotFound)
        {
            ThrowIfNotFound(roomId, room);
        }

        return room;
    }

    public async Task<Dictionary<int, AppReservationRoomVM>> GetRoomDictionary(IEnumerable<int> roomIds)
    {
        var results = await Query()
            .Where(x => roomIds.Contains(x.RoomId))
            .ToReservationRoomList()
            .ToListAsync();

        return results.ToDictionary(x => x.RoomId);
    }

    public async Task<int> GetRoomId(string roomKey)
    {
        var roomId = await Query()
            .Where(x => x.RoomKey == roomKey)
            .Select(x => x.RoomId)
            .FirstOrDefaultAsync();

        if (roomId == 0)
        {
            throw new NotFoundException($"Could not find a room with key ${roomKey}");
        }

        return roomId;
    }

    public async Task<AppRoom> Add(int tutorId, int userId, DateTime from, DateTime to, string roomKey)
    {
        var appRoom = new AppRoom
        {
            HostId = tutorId,
            AttenderId = userId,
            RoomKey = roomKey,
            ScheduledFromTime = from,
            ScheduledToTime = to,
            RoomStateId = (int)CRoomStateEnum.Created,
        };

        _db.AppRooms.Add(appRoom);

        await _db.SaveChangesAsync();
        _db.Entry(appRoom).State = EntityState.Detached;

        return appRoom;
    }

    private IQueryable<AppRoom> Query(bool asNoTracking = true)
    {
        var query = _db.AppRooms;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}