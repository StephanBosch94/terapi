using System.Security.Claims;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Models.ViewModels.Statistics;
using ULearn.Be.Models.ViewModels.UserStatistics;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Services.Utils;

namespace ULearn.Be.Services.Services;

public class AppUserStatisticsService : IAppUserStatisticsService
{
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppUserBehaviourRepository _appUserBehaviourRepository;
    private readonly IAppUserRepository _appUserRepository;

    public AppUserStatisticsService(
        IAppUserRepository appUserRepository,
        IAppReservationRepository appReservationRepository,
        IAppUserBehaviourRepository appUserBehaviourRepository
    )
    {
        _appUserRepository = appUserRepository;
        _appReservationRepository = appReservationRepository;
        _appUserBehaviourRepository = appUserBehaviourRepository;
    }

    public Task<UserPriceStatisticsVM> GetPriceStatistics()
    {
        return _appUserRepository.GetPriceStatistics();
    }

    public async Task<TutorPublicStatisticsVM> GetTutorPublicStatistics(int userId)
    {
        return new TutorPublicStatisticsVM
        {
            IsOnline = await _appUserBehaviourRepository.IsOnline(userId),
            LastVisited = await _appUserBehaviourRepository.GetLastVisited(userId),
            AttendancePercentage = await _appReservationRepository.GetAttendancePercentage(userId),
            NumberOfLessons = await _appReservationRepository.GetTutorNumberOfCompletedLessons(userId),
            NumberOfStudents = await _appReservationRepository.GetNumberOfStudents(userId)
        };
    }

    public async Task<TutorStatisticsVM> GetTutorPrivateStatistics(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetUserId();
        return new TutorStatisticsVM
        {
            NumberOfUpcomingLessons = await _appReservationRepository.GetTutorNumberOfUpcomingLessons(userId),
            NumberOfNewStudentsThisMonth = await _appReservationRepository.GetNumberOfNewStudentsThisMonth(userId),
            NumberOfNewStudentsPreviousMonth =
                await _appReservationRepository.GetNumberOfNewStudentsPreviousMonth(userId),
        };
    }

    public async Task<StudentStatisticsVM> GetStudentPrivateStatistics(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetUserId();
        var appUser = await _appUserRepository.GetRawDbEntity(userId);
        var (numberOfCompletedLessonsThisWeek, currentWeeklyGoalProgress) =
            await GetCurrentWeeklyGoalProgress(userId, appUser.WeeklyGoalId);

        return new StudentStatisticsVM
        {
            NumberOfCompletedLessonsThisWeek = numberOfCompletedLessonsThisWeek,
            CurrentWeeklyGoalProgress = currentWeeklyGoalProgress,
            NumberOfCompletedLessonsPreviousWeek =
                await _appReservationRepository.GetStudentNumberOfLessonsPreviousWeek(userId),
            TotalWeeklyGoalsCompleted = await GetTotalWeeklyGoalsCompeted(userId, appUser.WeeklyGoalId),
            NumberOfCompletedLessons = await _appReservationRepository.GetStudentNumberOfCompetedLessons(userId),
            NumberOfUpcomingLessons = await _appReservationRepository.GetStudentNumberOfUpcomingLessons(userId)
        };
    }

    public async Task<CompanyStatisticsVM> GetCompanyPrivateStatistics(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetUserId();
        var employeesGoalsByUserId = await _appUserRepository.GetEmployeesGoalsByUserId(userId);
        var employeesNumberOfLessonsThisWeek =
            await _appReservationRepository.GetAttendersCurrentWeeklyGaolsProgress(employeesGoalsByUserId.Keys);
        var employeesNumberOfLessonsPreviousWeek =
            await _appReservationRepository.GetAttendersPreviousWeeklyGaolsProgress(employeesGoalsByUserId.Keys);

        return new CompanyStatisticsVM
        {
            NumberOfEmployees = await _appUserRepository.GetNumberOfEmployees(userId),
            NumberOfLessonsThisMonth = await _appReservationRepository.GetCompanyNumberOfLessonsThisMonth(userId),
            NumberOfCompletedLessons = await _appReservationRepository.GetCompanyNumberOfCompletedLessons(userId),
            NumberOfLessonPreviousMonth =
                await _appReservationRepository.GetCompanyNumberOfLessonsPreviousMonth(userId),
            EmployeesWeeklyGoalThisWeek =
                CalculateCompanyWeeklyGoalScore(employeesGoalsByUserId, employeesNumberOfLessonsThisWeek),
            EmployeesWeeklyGoalPreviousWeek =
                CalculateCompanyWeeklyGoalScore(employeesGoalsByUserId, employeesNumberOfLessonsPreviousWeek)
        };
    }

    private static int CalculateCompanyWeeklyGoalScore(
        IReadOnlyDictionary<int, int> employeesGoalsByUserId,
        IReadOnlyDictionary<int, int> employeesNumberOfLessonsThisWeek
    )
    {
        var score = 0;
        foreach (var (userId, goalId) in employeesGoalsByUserId)
        {
            employeesNumberOfLessonsThisWeek.TryGetValue(userId, out var numberOfLessons);
            score += numberOfLessons >= goalId ? 1 : 0;
        }

        return score;
    }

    private async Task<(int numberOfLessonsThisWeek, int progress)> GetCurrentWeeklyGoalProgress(
        int userId,
        int? userGoalId
    )
    {
        if (userGoalId is null or 0)
        {
            return (0, 0);
        }

        var numberOfLessonsThisWeek = await _appReservationRepository.GetStudentNumberOfLessonsThisWeek(userId);
        return (numberOfLessonsThisWeek,
            UserStatisticUtil.GetCurrentWeeklyGoalProgress(numberOfLessonsThisWeek, userGoalId));
    }

    private async Task<int> GetTotalWeeklyGoalsCompeted(int userId, int? userGoalId)
    {
        if (userGoalId is null or 0)
        {
            return 0;
        }

        var results = await _appReservationRepository.GetStudentCompletedLessonsCountPerWeek(userId);
        return results
            .Count(x => x.Value >= userGoalId.Value);
    }
}