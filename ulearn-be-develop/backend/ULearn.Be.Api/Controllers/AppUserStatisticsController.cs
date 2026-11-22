using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Models.ViewModels.Statistics;
using ULearn.Be.Models.ViewModels.UserStatistics;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user-statistics")]
public class AppUserStatisticsController : ControllerBase
{
    private readonly IAppUserStatisticsService _appUserStatisticsService;
    private readonly ILogger<AppUserStatisticsController> _logger;

    public AppUserStatisticsController(ILogger<AppUserStatisticsController> logger,
        IAppUserStatisticsService appUserStatisticsService)
    {
        _logger = logger;
        _appUserStatisticsService = appUserStatisticsService;
    }

    [HttpGet("price")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(UserPriceStatisticsVM), StatusCodes.Status200OK)]
    public Task<UserPriceStatisticsVM> GetPriceStatistics()
    {
        return _appUserStatisticsService.GetPriceStatistics();
    }

    [HttpGet("tutor/{userId:int}")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TutorPublicStatisticsVM), StatusCodes.Status200OK)]
    public Task<TutorPublicStatisticsVM> GetTutorPublicStatistics(int userId)
    {
        return _appUserStatisticsService.GetTutorPublicStatistics(userId);
    }

    [HttpGet("tutor")]
    [Authorize(Policy = PolicyConstants.OnlyTutor,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(TutorStatisticsVM), StatusCodes.Status200OK)]
    public Task<TutorStatisticsVM> GetTutorPrivateStatistics()
    {
        return _appUserStatisticsService.GetTutorPrivateStatistics(User);
    }

    [HttpGet("student")]
    [Authorize(Policy = PolicyConstants.OnlyStudent,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(StudentStatisticsVM), StatusCodes.Status200OK)]
    public Task<StudentStatisticsVM> GetStudentPrivateStatistics()
    {
        return _appUserStatisticsService.GetStudentPrivateStatistics(User);
    }

    [HttpGet("company")]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(CompanyStatisticsVM), StatusCodes.Status200OK)]
    public Task<CompanyStatisticsVM> GetCompanyPrivateStatistics()
    {
        return _appUserStatisticsService.GetCompanyPrivateStatistics(User);
    }
}