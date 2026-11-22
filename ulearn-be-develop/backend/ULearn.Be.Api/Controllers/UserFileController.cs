using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Data.Enums;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/user-file")]
public class UserFileController : ControllerBase
{
    private readonly ILogger<UserFileController> _logger;
    private readonly IRUserFileService _userFileService;

    public UserFileController(
        ILogger<UserFileController> logger,
        IRUserFileService userFileService
    )
    {
        _logger = logger;
        _userFileService = userFileService;
    }

    [HttpDelete("filepath/{*filePath}")]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteFile(string filepath)
    {
        await _userFileService.DeleteFile(WebUtility.UrlDecode(filepath), User);

        return Ok();
    }

    [HttpPost("certificate")]
    [RequestSizeLimit(15_000_000)]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UploadCertificates()
    {
        var files = Request.Form.Files;
        if (!files.Any())
        {
            throw new BadRequestException();
        }

        await _userFileService.Upload(files, User, CFileTypeEnum.Certificate);

        return Ok();
    }

    [HttpPost("profile-photo")]
    [RequestSizeLimit(15_000_000)]
    [Authorize(Policy = PolicyConstants.AnyRole,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Upload()
    {
        var files = Request.Form.Files;
        if (!files.Any() || files.Count > 1)
        {
            throw new BadRequestException();
        }

        await _userFileService.UploadProfilePhoto(files[0], User);

        return Ok();
    }

    [HttpPost("profile-photo/admin-upload")]
    [RequestSizeLimit(15_000_000)]
    [Authorize(Policy = PolicyConstants.OnlyAdmin)]
    public async Task<IActionResult> AdminUpload([FromForm] [Required] string email)
    {
        var files = Request.Form.Files;
        if (!files.Any() || files.Count > 1)
        {
            throw new BadRequestException();
        }


        await _userFileService.UploadProfilePhoto(files[0], email);
        return Ok();
    }
}