using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class RUserTokenRepository : BaseRepository, IRUserTokenRepository
{
    private readonly RepositoryContext _db;

    public RUserTokenRepository(RepositoryContext db) : base(db)
    {
        _db = db;
    }

    public async Task Upsert(int userId, IEnumerable<UserTokenIM> tokenIMs)
    {
        var newRUserTokens = tokenIMs.Select(
            x => new RUserToken
            {
                UserId = userId,
                TokenTypeId = (int)x.CTokenTypeEnum,
                TokenProviderId = (int)x.CTokenProviderEnum,
                TokenValue = x.TokenValue
            }
        ).ToList();

        var dbRUserTokens = await Query(false)
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var compareResultVm = CompareUtil.Compare(
            newRUserTokens,
            dbRUserTokens,
            x => new { x.TokenProviderId, x.TokenTypeId }
        );

        await ApplyChanges(compareResultVm, (x, y) => x.TokenValue = y.TokenValue);
    }

    private IQueryable<RUserToken> Query(bool asNoTracking = true)
    {
        var query = _db.RUserTokens;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}