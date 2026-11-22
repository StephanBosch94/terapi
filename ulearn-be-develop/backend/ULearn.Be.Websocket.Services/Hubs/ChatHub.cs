using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Websocket.Services.Hubs;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class ChatHub : Hub
{
    private const string UrlParameter = "url";
    private readonly ILogger<ChatHub> _logger;
    private readonly IAppUserBehaviourRepository _userBehaviourRepository;

    public ChatHub(ILogger<ChatHub> logger, IAppUserBehaviourRepository userBehaviourRepository)
    {
        _logger = logger;
        _userBehaviourRepository = userBehaviourRepository;
    }


    public override async Task OnConnectedAsync()
    {
        string url = Context.GetHttpContext()!.Request.Query[UrlParameter];

        await _userBehaviourRepository.AddConnected(
            Context.ConnectionId,
            url,
            Context.User.GetUserId()
        );

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _userBehaviourRepository.UpdateSetDisconnected(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}