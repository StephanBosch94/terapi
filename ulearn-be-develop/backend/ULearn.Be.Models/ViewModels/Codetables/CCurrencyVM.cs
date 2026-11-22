namespace ULearn.Be.Models.ViewModels.Codetables;

public class CCurrencyVM : CodetableBaseVM
{
    public int CurrencyId { get; set; }
    public string Symbol { get; set; }
    public bool IsBase { get; set; }
    public double Rate { get; set; }
}