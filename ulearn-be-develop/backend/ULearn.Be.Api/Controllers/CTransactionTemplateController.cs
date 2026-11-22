using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/transaction-template")]
public class CTransactionTemplateController : ControllerBase
{
    private readonly ILogger<CTransactionTemplateController> _logger;
    private readonly IDbContextTransactionHandler _transactionHandler;
    private readonly ICTransactionTemplateService _transactionTemplateService;

    public CTransactionTemplateController(
        IDbContextTransactionHandler transactionHandler,
        ILogger<CTransactionTemplateController> logger,
        ICTransactionTemplateService transactionTemplateService
    )
    {
        _transactionHandler = transactionHandler;
        _logger = logger;
        _transactionTemplateService = transactionTemplateService;
    }

    [HttpPost]
    [Authorize(Policy = PolicyConstants.OnlyAdmin)]
    public async Task<IActionResult> UploadTransactionTemplate(
        [FromForm] [Required] string transactionTypeKey
    )
    {
        await _transactionHandler.WithTransaction(
            () => _transactionTemplateService.Upload(Request.Form.Files, transactionTypeKey)
        );
        return NoContent();
    }
}