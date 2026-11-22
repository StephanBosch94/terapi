using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppTransaction;

namespace ULearn.Be.Models.Mappers;

public static class AppTransactionMapper
{
    public static readonly Expression<Func<AppTransaction, AppTransactionVM>> AsVM =
        dataItem => new AppTransactionVM
        {
            Amount = dataItem.Amount,
            Created = dataItem.Created,
            TransactionId = dataItem.TransactionId,
            TransactionStateId = dataItem.TransactionStateId,
            ReservationId = dataItem.ReservationId,
            ReservationPackageId = dataItem.ReservationPackageId,
            TransactionTypeId = dataItem.TransactionTypeId,
            ReceiptUrl = dataItem.ReceiptUrl,
            Currency = CCurrencyMapper.ToVm(dataItem.Currency),
            TransactionState = CTransactionStateMapper.ToVm(dataItem.TransactionState),
            TransactionType = CTransactionTypeMapper.ToVm(dataItem.TransactionType)
        };

    public static readonly Expression<Func<AppTransaction, AppTransactionForNotificationVM>>
        AsAppTransactionForNotificationVM =
            dataItem => new AppTransactionForNotificationVM
            {
                TransactionId = dataItem.TransactionId,
                TransactionTypeId = dataItem.TransactionTypeId,
                Amount = dataItem.Amount,
                Approver = dataItem.Approver != null 
                    ? AppUserMapper.ToCommonVm(dataItem.Approver) 
                    : null,
                Creator = AppUserMapper.ToCommonVm(dataItem.Creator),
            };

    public static readonly Expression<Func<AppTransaction, AppTransactionWithdrawVM>>
        AsAppTransactionWithdrawVM =
            dataItem => new AppTransactionWithdrawVM
            {
                TransactionId = dataItem.TransactionId,
                RequestedAmount = dataItem.Amount,
                Created = dataItem.Created,
                TransactionStateId = dataItem.TransactionStateId,
                BankAccountNumber = dataItem.BankAccountNumber,
                BankCode = dataItem.BankCode,
                AmountCurrency = CCurrencyMapper.ToVm(dataItem.Currency),
                State = CTransactionStateMapper.ToVm(dataItem.TransactionState),
                Creator = AppUserMapper.ToCommonVm(dataItem.Creator),
                Wallet = AppUserWalletMapper.ToTransactionWalletVm(dataItem.UserWallet)
            };

    public static readonly Expression<Func<AppTransaction, AppTransactionForExportVM>> AsAppTransactionExportVM =
            dataItem => new AppTransactionForExportVM
            {
                TransactionId = dataItem.TransactionId,
                TransactionGroupId = dataItem.TransactionGroupId,
                Amount = dataItem.Amount,
                Created = dataItem.Created,
                State = CTransactionStateMapper.ToVm(dataItem.TransactionState),
                Type = CTransactionTypeMapper.ToVm(dataItem.TransactionType),
                Reservation = dataItem.AppReservation != null 
                    ? AppReservationMapper.ToAppTransactionReservationVM(dataItem.AppReservation)
                    : null,
                Vendor = AppUserMapper.ToCommonVm(dataItem.Vendor),
                Customer = AppUserMapper.ToCommonVm(dataItem.Customer),
            };
    
    public static IQueryable<AppTransactionVM> ToApiList(this IQueryable<AppTransaction> dataItems)
    {
        return dataItems
            .Include(x => x.Currency)
            .Include(x => x.TransactionState)
            .Include(x => x.TransactionType)
            .Select(AsVM);
    }

    public static IQueryable<AppTransactionWithdrawVM> ToWithdrawList(this IQueryable<AppTransaction> dataItems)
    {
        return dataItems
            .Include(x => x.Vendor)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Customer)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsAppTransactionWithdrawVM);
    }

    public static IQueryable<AppTransactionForNotificationVM> ToAppTransactionForNotificationVmList(
        this IQueryable<AppTransaction> dataItems)
    {
        return dataItems
            .Include(x => x.Creator)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Approver)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsAppTransactionForNotificationVM);
    }
    
    public static IQueryable<AppTransactionForExportVM> ToAppTransactionExportList(
        this IQueryable<AppTransaction> dataItems)
    {
        return dataItems
            .Include(x => x.AppReservation)
            .Include(x => x.Vendor)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Customer)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsAppTransactionExportVM);
    }
}