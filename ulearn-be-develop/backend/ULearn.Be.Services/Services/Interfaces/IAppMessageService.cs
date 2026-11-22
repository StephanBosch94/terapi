using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ULearn.Be.Common.Models;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppMessage;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppMessageService
{
    Task<PagedVM<AppMessageVM>> Filter(int conversationId, FilterView<MessageFilterVM> filter, ClaimsPrincipal user);
    Task Create(int opponentId, string messageText, IFormFileCollection formFiles, ClaimsPrincipal user);
}