using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Models.Filter;

public class TranslationsFilter
{
    [Required] public ICollection<string> LanguageCodes { get; set; }
}