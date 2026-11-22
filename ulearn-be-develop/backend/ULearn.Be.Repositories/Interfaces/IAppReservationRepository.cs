using ULearn.Be.Common.Models;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.Common;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppReservationRepository
{
    Task<AppReservationVM> GetRunningActivity(int currentUserId);
    Task<ICollection<AppReservation>> GetAllRaw(int hostId, DateTime from, DateTime to);
    Task<IReadOnlyCollection<AppReservation>> GetAllRawByPackageId(int packageId, CReservationStateEnum? state = null);
    Task<IReadOnlyCollection<AppReservationVM>> GetAll(IEnumerable<int> reservationIds);
    Task<int> Add(AppReservationIM input);
    Task<AppReservation> GetRawDbEntity(int reservationId);
    Task Remove(int reservationId);
    Task<PagedVM<AppReservationVM>> Filter(FilterView<AppReservationFilter> filter, int userId);
    Task<PagedVM<AppUserCommonVM>> GetTutors(int studentId);
    Task<PagedVM<AppUserCommonVM>> GetStudents(int tutorId);
    Task<int> GetAttendancePercentage(int userId);
    Task<int> GetTutorNumberOfCompletedLessons(int userId);
    Task<int> GetNumberOfStudents(int userId);
    Task<int> GetNumberOfNewStudentsThisMonth(int userId);
    Task<int> GetNumberOfNewStudentsPreviousMonth(int userId);
    Task<int> GetTutorNumberOfUpcomingLessons(int userId);
    Task<int> GetStudentNumberOfUpcomingLessons(int userId);
    Task<int> GetStudentNumberOfCompetedLessons(int userId);
    Task<int> GetTutorNumberOfUpcomingLessonsThisMonth(int userId);
    Task<int> GetStudentNumberOfUpcomingLessonsThisMonth(int userId);
    Task<IReadOnlyDictionary<int, int>> GetAttendancePercentageByUserId(IReadOnlyCollection<int> userIds);
    Task<IReadOnlyDictionary<int, int>> GetTutorNumberOfCompletedLessonsByUserId(IReadOnlyCollection<int> userIds);
    Task<IReadOnlyDictionary<int, int>> GetNumberOfStudentsByUserId(IReadOnlyCollection<int> userIds);
    Task<int> GetStudentNumberOfLessonsThisWeek(int userId);
    Task<int> GetStudentNumberOfLessonsPreviousWeek(int userId);
    Task<int> GetCompanyNumberOfCompletedLessons(int userId);
    Task<int> GetCompanyNumberOfLessonsThisMonth(int userId);
    Task<int> GetCompanyNumberOfLessonsPreviousMonth(int userId);
    Task<IReadOnlyDictionary<DateTime, int>> GetStudentCompletedLessonsCountPerWeek(int userId);
    Task<IReadOnlyDictionary<int, int>> GetStudentNumberOfCompletedLessonsByTutorId(int currentUserId);
    Task<IReadOnlyDictionary<int, int>> GetTutorNumberOfCompletedLessonsByStudentId(int currentUserId);
    Task<IReadOnlyDictionary<int, int>> GetNumberOfReservationsByPackageId(IEnumerable<int> packageIds);
    Task<IReadOnlyDictionary<int, int>> GetOrderNumberByReservationId(IEnumerable<int> packageIds);
    Task<IReadOnlyDictionary<int, int>> GetNumberOfRemainingLessonsByPackageId(IEnumerable<int> packageIds);
    Task<IReadOnlyCollection<AppReservation>> GetRemainingReservationsByPackageId(int packageId);
    Task<IReadOnlyCollection<AppReservation>> GetCompletedReservationsByPackageId(int packageId);
    Task<IReadOnlyCollection<AppReservation>> GetRemainingReservationsByUserId(int userId);
    Task UpdateStateByPackageId(int packageId, CReservationStateEnum newStateEnum);
    Task<IReadOnlyCollection<int>> GetAllReservationIdsByPackageId(int reservationPackageId);
    Task<IReadOnlyCollection<AppReservationForPaymentVM>> GetPastReadyForPaymentScheduleReservations();
    Task<IReadOnlyCollection<AppReservationForPaymentVM>> GetPastReadyForPaymentReservations();
    Task UpdateState(IEnumerable<int> reservationIds, CReservationStateEnum newState);

    Task<IReadOnlyDictionary<int, AppUserCommonVM>> GetOpponentByReservationId(
        IReadOnlyCollection<int> reservationIds,
        int userId
    );

    Task<IReadOnlyCollection<AppReservationForNotificationVM>> GetLessonsAboutToStartWithoutNotification(
        int lessonTimePeriodInMinutesToNotify
    );

    Task<AppReservationForNotificationVM> GetNotificationVmById(int reservationId);
    Task<IReadOnlyCollection<AppReservationForNotificationVM>> GetNotificationVmsByPackageId(int reservationPackageId);
    Task SetNotified(IEnumerable<int> reservationIds);

    Task<IReadOnlyDictionary<int, AppTransactionReservationVM>> GetTransactionReservations(
        IEnumerable<int> reservationIds
    );

    Task<IReadOnlyDictionary<int, AppUserEmployeeLessonStatisticsVM>> GetEmployeesLessonStatistics(
        IEnumerable<int> attenderIds
    );

    Task<IReadOnlyDictionary<int, int>> GetAttendersCurrentWeeklyGaolsProgress(IEnumerable<int> attenderIds);
    Task<IReadOnlyDictionary<int, int>> GetAttendersPreviousWeeklyGaolsProgress(IEnumerable<int> attenderIds);
    Task<bool> ExistsFutureBookings(int currentUserId);
    Task<bool> ExistsAnyBookings(int currentUserId);
}