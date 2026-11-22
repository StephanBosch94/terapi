using ULearn.Be.Models.ViewModels.Wallet;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services;

public class AppUserWalletSharedService : IAppUserWalletSharedService
{
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserWalletRepository _appUserWalletRepository;

    public AppUserWalletSharedService(IAppTransactionRepository appTransactionRepository,
        IAppUserWalletRepository appUserWalletRepository)
    {
        _appTransactionRepository = appTransactionRepository;
        _appUserWalletRepository = appUserWalletRepository;
    }
    
    public async Task UpdateWalletsStatistics(params int[] userIds)
    {
        var userIdsSet = userIds.ToHashSet();
        var currentBalanceByUserId = await _appTransactionRepository.GetCurrentBalanceByUserId(userIdsSet);
        var scheduledBalanceByUserId = await _appTransactionRepository.GetScheduledBalanceByUserId(userIdsSet);
        var totalRevenueByUserId = await _appTransactionRepository.GetTotalRevenueByUserId(userIdsSet);

        var inputVms = userIdsSet.Select(userId =>
            new UserWalletIM
            {
                UserId = userId,
                ScheduledAmount = scheduledBalanceByUserId.ContainsKey(userId) ? scheduledBalanceByUserId[userId] : 0,
                Amount = currentBalanceByUserId.ContainsKey(userId) ? currentBalanceByUserId[userId] : 0,
                TotalRevenue = totalRevenueByUserId.ContainsKey(userId) ? totalRevenueByUserId[userId] : 0,
            });
        await _appUserWalletRepository.UpdateRange(inputVms);
    }

    public Task UpdateWalletsStatistics(IEnumerable<int> userIds)
    {
        return UpdateWalletsStatistics(userIds.ToArray());
    }
}