using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.Wallet;

namespace ULearn.Be.Models.Mappers;

public static class AppUserWalletMapper
{
    private static readonly Expression<Func<AppUserWallet, UserWalletVM>> AsVM =
        dataItem => new UserWalletVM
        {
            UserId = dataItem.UserId,
            UserWalletId = dataItem.UserWalletId,
            Amount = dataItem.Amount,
            ScheduledAmount = dataItem.ScheduledAmount,
            TotalRevenue = dataItem.TotalRevenue,
            RevenueThisMonth = 0,
            RevenuePreviousMonth = 0,
            RevenueThisYear = 0,
            RevenuePreviousYear = 0,
            WithdrawThisMonth = 0,
            WithdrawThisYear = 0,
            Currency = CCurrencyMapper.ToVm(dataItem.Currency),
            BankAccountNumber = dataItem.BankAccountNumber,
            BankCode = dataItem.BankCode,
        };

    private static readonly Expression<Func<AppUserWallet, AppUserEmployeeWalletVM>> AsAppTransactionWalletVM =
        dataItem => new AppUserEmployeeWalletVM
        {
            WalletId = dataItem.UserWalletId,
            UserId = dataItem.UserId,
            Amount = dataItem.Amount,
            Currency = CCurrencyMapper.ToVm(dataItem.Currency),
            LastMoneyTransferDate = dataItem.AppTransactions
                .Where(x => x.TransactionTypeId == (int)CTransactionTypeEnum.MoneyTransfer)
                .Select(x => x.Created)
                .OrderByDescending(x => x)
                .Select(x => (DateTime?)x)
                .FirstOrDefault()
        };

    private static readonly Expression<Func<AppUserWallet, AppUserBillingInfoVM>> AsBillingInfoVM =
        dataItem => new AppUserBillingInfoVM
        {
            AccountNumber = dataItem.BankAccountNumber,
            BankCode = dataItem.BankCode,
        };

    public static AppTransactionWalletVM ToTransactionWalletVm(AppUserWallet dataItem)
    {
        return new AppTransactionWalletVM
        {
            Amount = dataItem.Amount,
            Currency = CCurrencyMapper.ToVm(dataItem.Currency)
        };
    }

    public static IQueryable<UserWalletVM> ToApiList(this IQueryable<AppUserWallet> dataItems)
    {
        return dataItems
            .Select(AsVM);
    }

    public static IQueryable<AppUserBillingInfoVM> ToBillingInfoList(this IQueryable<AppUserWallet> dataItems)
    {
        return dataItems
            .Select(AsBillingInfoVM);
    }

    public static IQueryable<AppUserEmployeeWalletVM> ToEmployeesWalletList(this IQueryable<AppUserWallet> dataItems)
    {
        return dataItems
            .Select(AsAppTransactionWalletVM);
    }
}