namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionCsvFileVM
{
    public static string ContentType => "text/csv";
    public byte[] Data { get; set; }
    public string Name { get; set; }
}