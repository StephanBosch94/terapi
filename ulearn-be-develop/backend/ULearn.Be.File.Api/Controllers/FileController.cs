using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Responses;
using ULearn.Be.File.Service.Services;
using ULearn.Be.File.Service.ViewModels;

namespace ULearn.Be.File.Api.Controllers;

[ApiController]
[Route("api/file")]
[Authorize(Policy = PolicyConstants.AnyRole)]
public class FileController : ControllerBase
{
    private readonly IFileService _service;

    public FileController(IFileService service)
    {
        _service = service;
    }

    /// <summary>
    ///     get file, path is expected to be urlencoded
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [RequestSizeLimit(1_073_741_824)]
    [HttpGet("environment/{environment}/filepath/{*filepath}")]
    public async Task<IActionResult> GetFile(string environment, string filepath)
    {
        var (stream, rawEntity) = await _service.GetFileStream(environment, WebUtility.UrlDecode(filepath));

        return File(stream, rawEntity.ContentType, rawEntity.FileName);
    }

    /// <summary>
    ///     get file archive in GZIP, path is expected to be urlencoded
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [RequestSizeLimit(1_073_741_824)]
    [HttpGet("archive/environment/{environment}/filepath/{*filepath}")]
    public async Task<IActionResult> GetArchive(string environment, string filepath)
    {
        var (stream, contentType, fileName) =
            await _service.GetArchiveStream(environment, WebUtility.UrlDecode(filepath));

        return File(stream, contentType, fileName);
    }

    /// <summary>
    ///     save file, directory path is expected to be urlencoded
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(ActionResult<AppFileRaw>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPost("environment/{environment}/filepath/{*filepath}")]
    [RequestSizeLimit(1_073_741_824)]
    public async Task<ActionResult<IReadOnlyCollection<AppFileRaw>>> SaveFile(string environment, string filepath)
    {
        var files = Request.Form.Files;
        if (!files.Any()) throw new BadRequestException();

        var fileInfos = await _service.SaveFile(environment, WebUtility.UrlDecode(filepath), files, User);

        return Ok(fileInfos);
    }

    /// <summary>
    ///     edit file, file path is expected to be urlencoded
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPut("environment/{environment}/filepath/{*filepath}")]
    [RequestSizeLimit(1_073_741_824)]
    public async Task<IActionResult> EditFile(string environment, string filepath, AppFileEditVM edit)
    {
        await _service.EditFile(environment, WebUtility.UrlDecode(filepath), edit, User);

        return NoContent();
    }

    /// <summary>
    ///     delete file, file path is expected to be urlencoded
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [HttpDelete("environment/{environment}/filepath/{*filepath}")]
    [RequestSizeLimit(1_073_741_824)]
    public async Task<IActionResult> DeleteFile(string environment, string filepath)
    {
        await _service.DeleteFile(environment, WebUtility.UrlDecode(filepath), User);

        return NoContent();
    }

    /// <summary>
    ///     check if file exists, file path is expected to be urlencoded
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [HttpHead("environment/{environment}/filepath/{*filepath}")]
    [RequestSizeLimit(1_073_741_824)]
    public async Task<IActionResult> Exists(string environment, string filepath)
    {
        var exists = await _service.ExistsFile(environment, WebUtility.UrlDecode(filepath));

        return exists ? Ok() : NotFound();
    }

    /// <summary>
    ///     check if file exists, file path is expected to be urlencoded
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [HttpGet("info/environment/{environment}/filepath/{*filepath}")]
    public async Task<ActionResult<AppFileRaw>> GetFileInfo(string environment, string filepath)
    {
        var rawEntity = await _service.GetFileInfo(environment, WebUtility.UrlDecode(filepath));

        return Ok(rawEntity);
    }

    /// <summary>
    ///     check if file exists
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [HttpGet("info/{fileKey}")]
    public async Task<ActionResult<AppFileRaw>> GetFileInfo(string fileKey)
    {
        var rawEntity = await _service.GetFileInfo(fileKey);

        return Ok(rawEntity);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPost("{fileId}/move")]
    public async Task<IActionResult> MoveFile(string environment, string filepath, string fileId)
    {
        await _service.DeleteFile(environment, WebUtility.UrlDecode(filepath), User);

        return NoContent();
    }
}