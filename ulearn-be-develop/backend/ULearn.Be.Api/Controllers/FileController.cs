using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ULearn.Be.Api.Base;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/file")]
public class FileController : CustomControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService, ITokenService tokenService) : base(tokenService)
    {
        _fileService = fileService;
    }

    [HttpGet("filepath/{*filepath}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    [ResponseCache(Duration = 3600)]
    public async Task<FileStreamResult> DownloadFile(string filePath)
    {
        var file = await _fileService.GetFile(WebUtility.UrlDecode(filePath), GetPrincipalFromCookie());

        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return new FileStreamResult(file.Stream, new MediaTypeHeaderValue(file.ContentType))
        {
            FileDownloadName = file.Name
        };
    }
}