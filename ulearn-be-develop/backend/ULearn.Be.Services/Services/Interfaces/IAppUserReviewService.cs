using System.Security.Claims;
using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppUserReviewService
{
    Task Add(AppUserReviewIM input, ClaimsPrincipal user);
    Task Edit(AppUserReviewUpdateIM input, ClaimsPrincipal user);
}