namespace ULearn.Be.Models.ViewModels.AppTransaction;

public class AppTransactionPdfFileVM
{
    public static string ContentType => "application/pdf";
    public Stream Stream { get; set; }
    public string Name { get; set; }
}