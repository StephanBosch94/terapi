namespace ULearn.Be.Models.ViewModels.Codetables;

public class CSubjectVM : CodetableBaseVM
{
    public int SubjectId { get; set; }
    public string IconName { get; set; }
    public string HeroTitleKey { get; set; }
    public string HeroTitleDescriptionKey { get; set; }
    public string PageTitleKey { get; set; }
    public string TutorPublicPageTitleKey { get; set; }
    public string TutorPublicPageDescriptionKey { get; set; }
    public string FooterSubjectTeachersPageLinkKey { get; set; }
    public string FooterSubjectLandingPageLinkKey { get; set; }
    public string UserGoalTitleKey { get; set; }
}