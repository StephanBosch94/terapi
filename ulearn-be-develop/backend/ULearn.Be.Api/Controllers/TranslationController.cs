using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/translation")]
public class TranslationController : CustomControllerBase
{
    private readonly ILogger<TranslationController> _logger;
    private readonly ITranslationService _service;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public TranslationController(
        ILogger<TranslationController> logger,
        IDbContextTransactionHandler transactionHandler,
        ITranslationService service,
        ITokenService tokenService
    ) : base(tokenService)
    {
        _logger = logger;
        _transactionHandler = transactionHandler;
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("filter")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public Task<IReadOnlyDictionary<string, Dictionary<string, string>>> FilterTranslations(
        FilterView<TranslationsFilter> filter
    )
    {
        return _service.FilterTranslations(filter);
    }

    [AllowAnonymous]
    [HttpGet("language/{translationLanguageId:int}")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IReadOnlyDictionary<string, string>> GetTranslations(int translationLanguageId)
    {
        var result = await _service.GetTranslationDictionary(translationLanguageId);
        SetCacheHeaders();
        return result;
    }

    [AllowAnonymous]
    [HttpGet("language/code/{languageCode}")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IReadOnlyDictionary<string, string>> GetTranslations(string languageCode)
    {
        var result = await _service.GetTranslationDictionary(languageCode);
        SetCacheHeaders();
        return result;
    }

    [Authorize(Policy = PolicyConstants.OnlyAdmin)]
    [HttpPut("batch")]
    public async Task BatchUpsert(ICollection<AppTranslationIM> translations)
    {
        await _transactionHandler.WithTransaction(
            () => _service.BatchUpsert(translations, User)
        );
    }
}