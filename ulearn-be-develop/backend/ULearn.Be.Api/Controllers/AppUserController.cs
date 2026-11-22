using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Api.Base;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Responses;
using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.AppUser.Sitemap;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Utils;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user")]
public class AppUserController : CustomControllerBase
{
    private readonly IAppUserService _appUserService;
    private readonly ILogger<AppUserController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppUserController(
        ILogger<AppUserController> logger,
        IAppUserService appUserService,
        IDbContextTransactionHandler transactionHandler,
        ITokenService tokenService
    ) : base(tokenService)
    {
        _logger = logger;
        _appUserService = appUserService;
        _transactionHandler = transactionHandler;
    }

    [HttpGet("short-info")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(AppShortUserInfoVM), StatusCodes.Status200OK)]
    public async Task<AppShortUserInfoVM> GetShortInfo()
    {
        var result = await _appUserService.GetShortInfo(User);
        await SignInAsync(AuthUtil.GetClaims(result));

        return result;
    }

    [HttpGet]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(AppUserVM), StatusCodes.Status200OK)]
    public Task<AppUserVM> Get()
    {
        return _appUserService.GetPrivateProfile(User);
    }

    [HttpGet("admin/{userId:int}")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(AppTutorAdminInfoVM), StatusCodes.Status200OK)]
    public Task<AppTutorAdminInfoVM> GetTutorAdminInfo(int userId)
    {
        return _appUserService.GetTutorAdminInfo(userId, User);
    }

    [HttpGet("{userId:int}")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AppUserPublicVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<AppUserPublicVM> GetById(
        int userId
    )
    {
        return _appUserService.GetTutorPublicProfile(userId, User);
    }

    [HttpPut]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(AppUserIM appUserIm)
    {
        var claims = await _transactionHandler.WithTransaction(
            () =>
                _appUserService.Update(appUserIm, User.GetUserId(), User)
        );

        await SignInAsync(claims);

        return Ok();
    }

    [HttpPut("{userId:int}/approve")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(int userId)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.Approve(userId, User)
        );
        return Ok();
    }

    [HttpPut("{userId:int}/refuse")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Refuse(int userId)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.Refuse(userId, User)
        );
        return Ok();
    }

    [HttpPut("{userId:int}")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int userId, AppUserIM appUserIm)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.Update(appUserIm, userId, User)
        );
        return Ok();
    }

    [HttpPut("employee")]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEmployeeAccount(AppEmployeeIM inout)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.UpdateEmployeeAccount(inout, User)
        );
        return NoContent();
    }

    [HttpPut("admin")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AdminUpdate(AppUserAdminIM appUserIm)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.AdminUpdate(appUserIm, User)
        );
        return NoContent();
    }

    [HttpGet("students")]
    [Authorize(Policy = PolicyConstants.OnlyTutor,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppRelatedUserVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<PagedVM<AppRelatedUserVM>> GetStudents()
    {
        return _appUserService.GetStudents(User);
    }

    [HttpGet("tutors")]
    [Authorize(Policy = PolicyConstants.OnlyStudent,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppRelatedUserVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<PagedVM<AppRelatedUserVM>> GetTutors()
    {
        return _appUserService.GetTutors(User);
    }

    [HttpPost("employee/filter")]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppUserEmployeeVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<PagedVM<AppUserEmployeeVM>> EmployeesFilter(FilterView<AppUserEmployeeFilterVM> filter)
    {
        return _appUserService.EmployeesFilter(User, filter);
    }

    [HttpPost("tutor/filter")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppUserOverviewVM>), StatusCodes.Status200OK)]
    public Task<PagedVM<AppUserOverviewVM>> TutorFilter(
        FilterView<UserFilterVM> filter
    )
    {
        return _appUserService.UserFilter(filter, CUserRoleEnum.Tutor, User);
    }

    [HttpPost("student/filter")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppUserOverviewVM>), StatusCodes.Status200OK)]
    public Task<PagedVM<AppUserOverviewVM>> StudentFilter(
        FilterView<UserFilterVM> filter
    )
    {
        return _appUserService.UserFilter(filter, CUserRoleEnum.Student, User);
    }

    [HttpPut("change-language/{languageCode}")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeLanguage(string languageCode)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.ChangeTranslationLanguage(languageCode, User)
        );

        return Ok();
    }

    [HttpPut("password")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdatePassword(PasswordUpdateIM input)
    {
        var user = await _transactionHandler.WithTransaction(
            () =>
                _appUserService.UpdatePassword(input, User)
        );

        await SignInAsync(AuthUtil.GetClaims(user));

        return Ok();
    }

    [HttpPut("reset-password")]
    [Authorize(Policy = PolicyConstants.RegistrationCodeConfirmation)]
    public async Task<IActionResult> ResetPassword(PasswordUpdateIM input)
    {
        var user = await _transactionHandler.WithTransaction(
            () =>
                _appUserService.UpdatePassword(input, User)
        );

        await SignInAsync(AuthUtil.GetClaims(user));

        return Ok();
    }


    [HttpDelete("employee")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Policy = PolicyConstants.OnlyCompany,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteEmployeeAccount(AppUserDeleteEmployeeIM input)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.DeleteEmployeeAccount(input, User)
        );


        return NoContent();
    }

    [HttpDelete("batch")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin)]
    public async Task<IActionResult> BatchHardAdminDelete(AppUserAdminBatchDeleteIM input)
    {
        await _transactionHandler.WithTransaction(
            () =>
                _appUserService.BatchHardAdminDelete(input, User)
        );


        return NoContent();
    }

    [HttpGet("tutor/sitemap-info")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(List<AppUserSitemapVM>), StatusCodes.Status200OK)]
    public Task<List<AppUserSitemapVM>> GetTutorsSitemapInfo()
    {
        return _appUserService.GetTutorsSitemapInfo(User);
    }
}