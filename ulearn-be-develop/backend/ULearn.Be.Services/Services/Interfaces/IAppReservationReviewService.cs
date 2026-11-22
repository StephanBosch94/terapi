using System.Security.Claims;
using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppReservationReviewService
{
    Task Add(AppReservationReviewIM input, ClaimsPrincipal user);
    Task Edit(AppReservationReviewEditIM input, ClaimsPrincipal user);
}