using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/checkout")]
[Authorize(Policy = PolicyConstants.AnyRole, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;
    private readonly IDbContextTransactionHandler _transactionHandler;

    public CheckoutController(
        IDbContextTransactionHandler transactionHandler,
        ICheckoutService checkoutService
    )
    {
        _transactionHandler = transactionHandler;
        _checkoutService = checkoutService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        await _transactionHandler.WithTransaction(() => _checkoutService.WebhookHandle(json, signature));

        return NoContent();
    }
}