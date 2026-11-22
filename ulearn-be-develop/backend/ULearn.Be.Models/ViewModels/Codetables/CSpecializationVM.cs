namespace ULearn.Be.Models.ViewModels.Codetables;

public class CSpecializationVM : CodetableBaseVM
{
    public int SpecializationId { get; set; }
    public int SpecializationCategoryId { get; set; }
    public string SpecializationCategoryNameKey { get; set; }
    public int SubjectId { get; set; }
    public string IconName { get; set; }
    public string HeroTitleKey { get; set; }
    public string HeroDescriptionKey { get; set; }
    public string PageTitleKey { get; set; }
    public string PageDescriptionKey { get; set; }
}