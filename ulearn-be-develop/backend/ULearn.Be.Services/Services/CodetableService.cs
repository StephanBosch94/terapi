using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class CodetableService : ICodetableService
{
    private readonly RepositoryContext _db;

    public CodetableService(RepositoryContext db)
    {
        _db = db;
    }

    public async Task<ICollection<CMasterCacheVM>> GetCMasterCache()
    {
        return await _db.CMasterHashes
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CCategoryVM>> GetCCategories()
    {
        return await _db.CCategories
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CCountryVM>> GetCCounties()
    {
        return await _db.CCountries
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CCurrencyVM>> GetCCurrencies()
    {
        return await _db.CCurrencies
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CFileTypeVM>> GetCFileTypes()
    {
        return await _db.CFileTypes
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CLanguageLevelVM>> GetCLanguageLevels()
    {
        return await _db.CLanguageLevels
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CLanguageVM>> GetCLanguages()
    {
        return await _db.CLanguages
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CPaymentServiceProviderVM>> GetCPaymentServiceProviders()
    {
        return await _db.CPaymentServiceProviders
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CReservationStateVM>> GetCReservationStates()
    {
        return await _db.CReservationStates
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CTeachingExperienceVM>> GetCTeachingExperiences()
    {
        return await _db.CTeachingExperiences
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CSubjectLevelVM>> GetCSubjectLevels()
    {
        return await _db.CSubjectLevels
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CSubjectVM>> GetCSubjects(bool onlyActive)
    {
        return await _db.CSubjects
            .AsNoTracking()
            .Where(x => !onlyActive || x.AppUsers.Any(y => y.UserRoleId == (int)CUserRoleEnum.Tutor))
            .Where(x => x.ObjectStateId == (int)CObjectStateEnum.Active)
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CUserAttributeTypeVM>> GetCUserAttributeTypes()
    {
        return await _db.CUserAttributeTypes
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CUserRoleVM>> GetCUserRoles()
    {
        return await _db.CUserRoles
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CUserStateVM>> GetCUserStates()
    {
        return await _db.CUserStates
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CTranslationLanguageVM>> GetTranslationLanguages()
    {
        return await _db.CTranslationLanguages
            .AsNoTracking()
            .Where(x => x.LanguageCode != "cs")
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CSpecializationVM>> GetSpecializations(bool onlyActive)
    {
        return await _db.CSpecializations
            .AsNoTracking()
            .Where(x => !onlyActive || x.RUserSpecializations.Any(y => y.User.UserRoleId == (int)CUserRoleEnum.Tutor))
            .ToApiList()
            .OrderBy(x => x.SubjectId)
            .ThenBy(x => x.SpecializationCategoryId)
            .ThenBy(x => x.SpecializationId)
            .ToListAsync();
    }

    public async Task<ICollection<CWeeklyGoalVM>> GetWeeklyGoals()
    {
        return await _db.CWeeklyGoals
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CInterestVM>> GetInterests()
    {
        return await _db.CInterests
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }

    public async Task<ICollection<CReservationIssueTypeVM>> GetReservationIssueTypes()
    {
        return await _db.CReservationIssueTypes
            .AsNoTracking()
            .ToApiList()
            .ToListAsync();
    }
}