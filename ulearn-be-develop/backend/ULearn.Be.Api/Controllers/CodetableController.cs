using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/codetable")]
public class CodetableController : CustomControllerBase
{
    private readonly ICodetableService _codetableService;
    private readonly ILogger<CodetableController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public CodetableController(
        ILogger<CodetableController> logger,
        ICodetableService codetableService,
        ITokenService tokenService,
        IDbContextTransactionHandler transactionHandler
    ) : base(tokenService)
    {
        _logger = logger;
        _codetableService = codetableService;
        _transactionHandler = transactionHandler;
    }

    [HttpGet("master-cache")]
    public Task<ICollection<CMasterCacheVM>> GetCMasterCache()
    {
        return _codetableService.GetCMasterCache();
    }

    [HttpGet("category")]
    public Task<ICollection<CCategoryVM>> GetCCategories()
    {
        SetCacheHeaders();
        return _codetableService.GetCCategories();
    }

    [HttpGet("country")]
    public Task<ICollection<CCountryVM>> GetCCounties()
    {
        SetCacheHeaders();
        return _codetableService.GetCCounties();
    }

    [HttpGet("currency")]
    public Task<ICollection<CCurrencyVM>> GetCCurrencies()
    {
        SetCacheHeaders();
        return _codetableService.GetCCurrencies();
    }

    [HttpGet("file-type")]
    public Task<ICollection<CFileTypeVM>> GetCFileTypes()
    {
        SetCacheHeaders();
        return _codetableService.GetCFileTypes();
    }

    [HttpGet("language-level")]
    public Task<ICollection<CLanguageLevelVM>> GetCLanguageLevels()
    {
        SetCacheHeaders();
        return _codetableService.GetCLanguageLevels();
    }

    [HttpGet("language")]
    public Task<ICollection<CLanguageVM>> GetCLanguages()
    {
        SetCacheHeaders();
        return _codetableService.GetCLanguages();
    }

    [HttpGet("payment-service-provider")]
    public Task<ICollection<CPaymentServiceProviderVM>> GetCPaymentServiceProviders()
    {
        SetCacheHeaders();
        return _codetableService.GetCPaymentServiceProviders();
    }

    [HttpGet("reservation-state")]
    public Task<ICollection<CReservationStateVM>> GetCReservationStates()
    {
        SetCacheHeaders();
        return _codetableService.GetCReservationStates();
    }

    [HttpGet("teaching-experience")]
    public Task<ICollection<CTeachingExperienceVM>> GetCTeachingExperience()
    {
        SetCacheHeaders();
        return _codetableService.GetCTeachingExperiences();
    }

    [HttpGet("subject-level")]
    public Task<ICollection<CSubjectLevelVM>> GetCSubjectLevels()
    {
        SetCacheHeaders();
        return _codetableService.GetCSubjectLevels();
    }

    [HttpGet("subject")]
    public Task<ICollection<CSubjectVM>> GetCSubjects([FromQuery] bool onlyActive = false)
    {
        SetCacheHeaders();
        return _codetableService.GetCSubjects(onlyActive);
    }

    [HttpGet("attribute")]
    public Task<ICollection<CUserAttributeTypeVM>> GetCUserAttributeTypes()
    {
        SetCacheHeaders();
        return _codetableService.GetCUserAttributeTypes();
    }

    [HttpGet("user-role")]
    public Task<ICollection<CUserRoleVM>> GetCUserRoles()
    {
        SetCacheHeaders();
        return _codetableService.GetCUserRoles();
    }

    [HttpGet("user-state")]
    public Task<ICollection<CUserStateVM>> GetCUserStates()
    {
        SetCacheHeaders();
        return _codetableService.GetCUserStates();
    }

    [HttpGet("translation-language")]
    public Task<ICollection<CTranslationLanguageVM>> GetTranslationLanguages()
    {
        SetCacheHeaders();
        return _codetableService.GetTranslationLanguages();
    }

    [HttpGet("specialization")]
    public Task<ICollection<CSpecializationVM>> GetSpecializations([FromQuery] bool onlyActive = false)
    {
        SetCacheHeaders();
        return _codetableService.GetSpecializations(onlyActive);
    }

    [HttpGet("weekly-goal")]
    public Task<ICollection<CWeeklyGoalVM>> GetWeeklyGoals()
    {
        SetCacheHeaders();
        return _codetableService.GetWeeklyGoals();
    }

    [HttpGet("interest")]
    public Task<ICollection<CInterestVM>> GetInterests()
    {
        SetCacheHeaders();
        return _codetableService.GetInterests();
    }

    [HttpGet("reservation-issue-types")]
    public Task<ICollection<CReservationIssueTypeVM>> GetIssueTypes()
    {
        SetCacheHeaders();
        return _codetableService.GetReservationIssueTypes();
    }
}