using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Be.Common.Constants;

namespace ULearn.Be.Api.Controllers;

[ApiController]
[Route("api/health")]
[Authorize(Policy = PolicyConstants.AnyRole)]
public sealed class HealthCheckController : ControllerBase
{
    /// <summary>
    ///     health check for service
    /// </summary>
    /// <returns></returns>
    [HttpGet("check")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AllowAnonymous]
    public IActionResult Check()
    {
        return NoContent();
    }
}