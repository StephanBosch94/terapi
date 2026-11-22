using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/transaction")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AppTransactionController : ControllerBase
{
    private readonly IAppTransactionService _appTransactionService;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public AppTransactionController(
        IAppTransactionService appTransactionService,
        IDbContextTransactionHandler transactionHandler
    )
    {
        _appTransactionService = appTransactionService;
        _transactionHandler = transactionHandler;
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(PagedVM<AppTransactionVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<PagedVM<AppTransactionVM>> Filter(FilterView<AppTransactionFilterVM> filter)
    {
        return _appTransactionService.Filter(filter, User);
    }

    [HttpPost("withdraw/filter")]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedVM<AppTransactionWithdrawVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<PagedVM<AppTransactionWithdrawVM>> WithdrawFilter(FilterView<AppTransactionWithdrawFilterVM> filter)
    {
        return _appTransactionService.WithdrawFilter(filter);
    }

    /// <summary>
    ///     Used for withdrawal approvements
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    [HttpPut("{transactionId:int}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = PolicyConstants.OnlyAdmin,
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CompleteTransaction(int transactionId)
    {
        await _transactionHandler.WithTransaction(() =>
            _appTransactionService.CompleteTransaction(transactionId, User));

        return Ok();
    }

    [HttpGet("{transactionId:int}/export")]
    public async Task<FileStreamResult> Export(int transactionId)
    {
        var file = await _appTransactionService.GetFile(transactionId, User);

        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return new FileStreamResult(file.Stream, new MediaTypeHeaderValue(AppTransactionPdfFileVM.ContentType))
        {
            FileDownloadName = file.Name
        };
    }

    [HttpGet("export")]
    public async Task<FileStreamResult> Export([FromQuery] AppTransactionExportIM options)
    {
        var file = await _appTransactionService.Export(options, User);

        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        
        return new FileStreamResult(new MemoryStream(file.Data), new MediaTypeHeaderValue(AppTransactionCsvFileVM.ContentType))
        {
            FileDownloadName = file.Name
        };
    }
}