using CsvHelper.Configuration.Attributes;

namespace ULearn.Db.Tools.Models;

[Delimiter("\t")]
public class TranslationsFlatVM
{
    [Name("TranslationKey")] public string TranslationKey { get; set; }

    [Name("en")] public string EnglishTranslation { get; set; }

    [Name("cs")] [Optional] public string CzechTranslation { get; set; }

    [Name("ru")] [Optional] public string RussianTranslation { get; set; }

    [Name("uk")] [Optional] public string UkrainianTranslation { get; set; }

    [Name("sr")] [Optional] public string SerbianTranslation { get; set; }
}