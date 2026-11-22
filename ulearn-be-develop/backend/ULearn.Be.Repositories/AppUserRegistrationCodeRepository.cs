using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppUserRegistration;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppUserRegistrationCodeRepository : BaseService, IAppUserRegistrationCodeRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppUserRegistrationCodeRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task Add(int userId, string code)
    {
        var entity = new AppUserRegistrationCode
        {
            UserId = userId,
            RegistrationCode = code,
            Created = _timeUtility.GetUtcNow(),
            Used = null
        };
        _db.AppUserRegistrationCodes.Add(entity);
        return _db.SaveChangesAsync();
    }

    public Task<AppUserRegistrationCode> GetLatestDbEntity(int userId, bool asNoTracking = true)
    {
        return QueryAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.Used == null)
            .OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync();
    }

    public async Task SetUsed(int userRegistrationCodeId)
    {
        var dbEntity = await QueryAll(false)
            .Where(x => x.UserRegistrationCodeId == userRegistrationCodeId)
            .FirstOrDefaultAsync();
        ThrowIfNotFound(userRegistrationCodeId, dbEntity);
        dbEntity!.Used = _timeUtility.GetUtcNow();
        await _db.SaveChangesAsync();
    }

    public Task<UserRegistrationCodeInfoVM> GetLatestCodeInfo(int userId)
    {
        return QueryAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.Used == null)
            .OrderByDescending(x => x.Created)
            .ToApiList()
            .FirstOrDefaultAsync();
    }

    public Task<AppUserRegistrationCode> GetRawDbEntity(int userId, string registrationCode)
    {
        return QueryAll()
            .Where(x => x.Used == null)
            .Where(x => x.UserId == userId)
            .Where(x => x.RegistrationCode == registrationCode)
            .FirstOrDefaultAsync();
    }

    private IQueryable<AppUserRegistrationCode> QueryAll(bool asNoTracking = true)
    {
        var query = _db.AppUserRegistrationCodes;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}