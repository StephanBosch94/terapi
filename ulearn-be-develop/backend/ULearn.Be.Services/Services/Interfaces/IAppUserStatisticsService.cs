using System.Security.Claims;
using ULearn.Be.Models.ViewModels.Statistics;
using ULearn.Be.Models.ViewModels.UserStatistics;

namespace ULearn.Be.Services.Services.Interfaces;

public interface IAppUserStatisticsService
{
    Task<UserPriceStatisticsVM> GetPriceStatistics();
    Task<TutorPublicStatisticsVM> GetTutorPublicStatistics(int userId);
    Task<TutorStatisticsVM> GetTutorPrivateStatistics(ClaimsPrincipal claimsPrincipal);
    Task<StudentStatisticsVM> GetStudentPrivateStatistics(ClaimsPrincipal claimsPrincipal);
    Task<CompanyStatisticsVM> GetCompanyPrivateStatistics(ClaimsPrincipal claimsPrincipal);
}