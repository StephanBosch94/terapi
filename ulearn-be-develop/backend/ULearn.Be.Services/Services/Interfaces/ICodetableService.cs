using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Services.Services.Interfaces;

public interface ICodetableService
{
    Task<ICollection<CMasterCacheVM>> GetCMasterCache();
    Task<ICollection<CCategoryVM>> GetCCategories();
    Task<ICollection<CCountryVM>> GetCCounties();
    Task<ICollection<CCurrencyVM>> GetCCurrencies();
    Task<ICollection<CFileTypeVM>> GetCFileTypes();
    Task<ICollection<CLanguageLevelVM>> GetCLanguageLevels();
    Task<ICollection<CLanguageVM>> GetCLanguages();
    Task<ICollection<CPaymentServiceProviderVM>> GetCPaymentServiceProviders();
    Task<ICollection<CReservationStateVM>> GetCReservationStates();
    Task<ICollection<CTeachingExperienceVM>> GetCTeachingExperiences();
    Task<ICollection<CSubjectLevelVM>> GetCSubjectLevels();
    Task<ICollection<CSubjectVM>> GetCSubjects(bool onlyActive);
    Task<ICollection<CUserAttributeTypeVM>> GetCUserAttributeTypes();
    Task<ICollection<CUserRoleVM>> GetCUserRoles();
    Task<ICollection<CUserStateVM>> GetCUserStates();
    Task<ICollection<CTranslationLanguageVM>> GetTranslationLanguages();
    Task<ICollection<CSpecializationVM>> GetSpecializations(bool onlyActive);
    Task<ICollection<CWeeklyGoalVM>> GetWeeklyGoals();
    Task<ICollection<CInterestVM>> GetInterests();
    Task<ICollection<CReservationIssueTypeVM>> GetReservationIssueTypes();
}