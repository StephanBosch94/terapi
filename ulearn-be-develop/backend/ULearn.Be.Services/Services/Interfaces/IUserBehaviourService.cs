using System.Security.Claims;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IUserBehaviourService
{
    Task TutorProfileVisited(int userId, ClaimsPrincipal principal);
}