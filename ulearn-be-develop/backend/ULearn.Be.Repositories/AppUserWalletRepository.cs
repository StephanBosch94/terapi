using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.Wallet;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppUserWalletRepository : BaseRepository, IAppUserWalletRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppUserWalletRepository(RepositoryContext db, ITimeUtility timeUtility) : base(db)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public async Task<AppUserWallet> GetRawDbEntity(int userWalletId)
    {
        var result = await Query()
            .Where(x => x.UserWalletId == userWalletId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userWalletId, result);

        return result;
    }

    public async Task<int> GetUserWalletId(int userId)
    {
        var userWalletId = await Query()
            .Where(x => x.UserId == userId)
            .Select(x => x.UserWalletId)
            .FirstOrDefaultAsync();

        if (userWalletId == 0)
        {
            throw new NotFoundException($"Could not find wallet with userId = [{userId}]");
        }

        return userWalletId;
    }

    public Task<UserWalletVM> Get(int userId)
    {
        return Query()
            .Where(x => x.UserId == userId)
            .ToApiList()
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyDictionary<int, int>> GetUserWalletByUserId(IEnumerable<int> userIds)
    {
        var userWallets = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .Select(x => new
            {
                x.UserId,
                x.UserWalletId
            })
            .ToListAsync();

        return userWallets.ToDictionary(x => x.UserId, x => x.UserWalletId);
    }

    public async Task UpdateRange(IEnumerable<UserWalletIM> inputVms)
    {
        var now = _timeUtility.GetUtcNow();
        var inputDictionaryByUserId = inputVms.ToDictionary(x => x.UserId);
        var userIds = inputDictionaryByUserId.Keys.ToHashSet();

        var userWallets = await Query(false)
            .Where(x => userIds.Contains(x.UserId))
            .ToListAsync();

        foreach (var appUserWallet in userWallets)
        {
            var newData = inputDictionaryByUserId[appUserWallet.UserId];
            appUserWallet.Amount = newData.Amount;
            appUserWallet.ScheduledAmount = newData.ScheduledAmount;
            appUserWallet.TotalRevenue = newData.TotalRevenue;
            appUserWallet.Updated = now;
        }

        await _db.SaveChangesAsync();
    }

    public async Task UpdateBankAccountInformation(int walletId, string bankAccountNumber, string bankCode)
    {
        var wallet = await Query(false)
            .Where(x => x.UserWalletId == walletId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(walletId, wallet);

        wallet!.BankCode = bankCode;
        wallet.BankAccountNumber = bankAccountNumber;

        await _db.SaveChangesAsync();
    }

    public async Task<AppUserBillingInfoVM> GetBillingInfo(int userId)
    {
        var info = await Query()
            .Where(x => x.UserId == userId)
            .ToBillingInfoList()
            .FirstOrDefaultAsync();

        if (info == null)
        {
            throw new NotFoundException($"Could not find a user wallet with userId: {userId}");
        }

        return info;
    }

    public async Task<IReadOnlyDictionary<int, AppUserEmployeeWalletVM>> GetEmployeesWalletInfo(
        IEnumerable<int> userIds)
    {
        var results = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .ToEmployeesWalletList()
            .ToListAsync();

        return results.ToDictionary(x => x.UserId);
    }

    private IQueryable<AppUserWallet> Query(bool asNoTracking = true)
    {
        var query = _db.AppUserWallets;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}