using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Common.Constants;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/env-configuration/")]
public class EnvConfigurationController : CustomControllerBase
{
    private readonly IEnvConfigurationProvider _envConfigurationProvider;
    private readonly ILogger<EnvConfigurationController> _logger;

    public EnvConfigurationController(
        ILogger<EnvConfigurationController> logger,
        IEnvConfigurationProvider envConfigurationProvider,
        ITokenService tokenService
    ) : base(tokenService)
    {
        _logger = logger;
        _envConfigurationProvider = envConfigurationProvider;
    }

    [AllowAnonymous]
    [HttpGet]
    public Task<Dictionary<string, object>> GetAll()
    {
        SetCacheHeaders();
        return _envConfigurationProvider.GetConfigurationAllFe();
    }

    [Authorize(Policy = PolicyConstants.OnlyAdmin)]
    [HttpPut("reload")]
    public IActionResult ReloadSettings()
    {
        _envConfigurationProvider.ReloadEnvSettings();

        return Ok();
    }
}