using System.ComponentModel;
using CsvHelper.Configuration;

namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionExportVM
{
    [Description("invoice_number")] public string InvoiceNumber { get; set; }
    [Description("transaction")] public string Transaction { get; set; }
    [Description("amount")] public string Amount { get; set; }
    [Description("amount_currency")] public string AmountCurrency { get; set; }
    [Description("due_date")] public string DueDate { get; set; }
    [Description("tax_date")] public string TaxDate { get; set; }
    [Description("status")] public string Status { get; set; }
    [Description("reservation_number")] public int? ReservationNumber { get; set; }
    [Description("customer_name")] public string CustomerName { get; set; }
    [Description("customer_ico")] public string CustomerIco { get; set; }
    [Description("customer_dic")] public string CustomerDic { get; set; }
    [Description("vendor_name")] public string VendorName { get; set; }
    [Description("vendor_ico")] public string VendorIco { get; set; }
    [Description("vendor_dic")] public string VendorDic { get; set; }
}

public sealed class AppTransactionExportClassMap : ClassMap<AppTransactionExportVM>
{
    public AppTransactionExportClassMap(IReadOnlyDictionary<string, string> headers)
    {
        Map(m => m.InvoiceNumber).Name(headers[nameof(AppTransactionExportVM.InvoiceNumber)]);
        Map(m => m.Transaction).Name(headers[nameof(AppTransactionExportVM.Transaction)]);
        Map(m => m.Amount).Name(headers[nameof(AppTransactionExportVM.Amount)]);
        Map(m => m.AmountCurrency).Name(headers[nameof(AppTransactionExportVM.AmountCurrency)]);
        Map(m => m.DueDate).Name(headers[nameof(AppTransactionExportVM.DueDate)]);
        Map(m => m.TaxDate).Name(headers[nameof(AppTransactionExportVM.TaxDate)]);
        Map(m => m.Status).Name(headers[nameof(AppTransactionExportVM.Status)]);
        Map(m => m.ReservationNumber).Name(headers[nameof(AppTransactionExportVM.ReservationNumber)]);
        Map(m => m.CustomerName).Name(headers[nameof(AppTransactionExportVM.CustomerName)]);
        Map(m => m.CustomerIco).Name(headers[nameof(AppTransactionExportVM.CustomerIco)]);
        Map(m => m.CustomerDic).Name(headers[nameof(AppTransactionExportVM.CustomerDic)]);
        Map(m => m.VendorName).Name(headers[nameof(AppTransactionExportVM.VendorName)]);
        Map(m => m.VendorIco).Name(headers[nameof(AppTransactionExportVM.VendorIco)]);
        Map(m => m.VendorDic).Name(headers[nameof(AppTransactionExportVM.VendorDic)]);
        // CreateMap(headers);
    }

    public void CreateMap(IReadOnlyDictionary<string, string> headers)
    {
        foreach (var memberMap in MemberMaps.Where(x => x.Data.Member != null))
        {
            if (headers.TryGetValue(memberMap.Data.Member!.Name, out var localizedHeader))
            {
                memberMap.Data.Names.Clear();
                memberMap.Data.Names.Add(localizedHeader);
            }
        }
    }
}