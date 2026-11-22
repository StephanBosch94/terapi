using System.Security.Claims;
using ULearn.Be.Models.ViewModels.Wallet;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppUserWalletService
{
    Task<UserWalletDepositVM> DepositMoney(WalletDepositIM input, ClaimsPrincipal user);
    Task WithdrawMoney(WalletWithdrawalIM input, ClaimsPrincipal user);
    Task<UserWalletVM> Get(ClaimsPrincipal user);
    Task TopUpPayment(TopUpPaymentIM input, ClaimsPrincipal user);
    Task TopUpUserPayment(TopUpUserPaymentIM input, ClaimsPrincipal user);
    Task ExtractMoney(ExtractPaymentIM input, ClaimsPrincipal user);
}