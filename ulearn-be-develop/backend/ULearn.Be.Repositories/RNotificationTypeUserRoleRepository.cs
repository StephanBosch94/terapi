using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers.Relations;
using ULearn.Be.Models.ViewModels.Notification;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RNotificationTypeUserRoleRepository: IRNotificationTypeUserRoleRepository
{
    private readonly RepositoryContext _db;

    public RNotificationTypeUserRoleRepository(RepositoryContext db)
    {
        _db = db;
    }
    
    public async Task<IReadOnlyCollection<RUserNotificationSettingVM>> GetByRole(CUserRoleEnum userRoleEnum)
    {
        return await Query()
            .Where(x => x.UserRoleId == (int)userRoleEnum)
            .ToUserNotificationSettingList()
            .ToListAsync();
    }
    
    private IQueryable<RNotificationTypeUserRole> Query(bool asNoTracking = true)
    {
        var query = _db.RNotificationTypeUserRole;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}