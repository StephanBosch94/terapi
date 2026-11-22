namespace ULearn.Db.Tools.Models;

public class TranslationVM
{
    public string TranslationKey { get; set; }
    public List<TranslationValueVM> Values { get; set; }
}

public class TranslationValueVM
{
    public string LanguageCode { get; set; }
    public string Value { get; set; }
}