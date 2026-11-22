namespace ULearn.Be.Shared.Services.Interfaces;

public interface IAppUserWalletSharedService
{
    Task UpdateWalletsStatistics(params int[] userIds);
    Task UpdateWalletsStatistics(IEnumerable<int> toHashSet);
}