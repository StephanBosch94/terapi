using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.Wallet;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppUserWalletRepository
{
    Task<AppUserWallet> GetRawDbEntity(int userWalletId);
    Task<int> GetUserWalletId(int userId);
    Task<UserWalletVM> Get(int userId);
    Task<IReadOnlyDictionary<int, int>> GetUserWalletByUserId(IEnumerable<int> userIds);
    Task UpdateRange(IEnumerable<UserWalletIM> inputVms);
    Task UpdateBankAccountInformation(int userWalletId, string bankAccountNumber, string bankCode);
    Task<AppUserBillingInfoVM> GetBillingInfo(int userId);
    Task<IReadOnlyDictionary<int, AppUserEmployeeWalletVM>> GetEmployeesWalletInfo(IEnumerable<int> userIds);
}