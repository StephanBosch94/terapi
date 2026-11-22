using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.Mappers.QueryMappers;
using ULearn.Be.Models.ViewModels.AppTransaction;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.Common;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppReservationRepository : BaseService, IAppReservationRepository
{
    public static readonly List<int> ConfirmedStates = new()
    {
        (int)CReservationStateEnum.Confirmed,
        (int)CReservationStateEnum.ScheduledForPayment,
        (int)CReservationStateEnum.Payed,
        (int)CReservationStateEnum.Refunded,
        (int)CReservationStateEnum.WaitingForReview
    };

    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppReservationRepository(RepositoryContext db, ITimeUtility timeUtility)
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task<PagedVM<AppReservationVM>> Filter(FilterView<AppReservationFilter> filter, int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var query = QueryConfirmed()
            .Where(x => x.HostId == userId || x.AttenderId == userId);

        if (filter.Filter?.Time is ReservationTimeEnum.Incoming)
        {
            query = query.Where(x => x.StartTime >= now.AddHours(-1));
        }

        if (filter.Filter?.Time is ReservationTimeEnum.Past)
        {
            query = query.Where(x => x.StartTime < now.AddHours(-1));
        }

        return query
            .ToApiList()
            .ToPagedVMAsync(filter);
    }

    public Task<AppReservationVM> GetRunningActivity(int currentUserId)
    {
        var now = _timeUtility.GetUtcNow();
        return QueryConfirmed()
            .Where(x => x.HostId == currentUserId || x.AttenderId == currentUserId)
            .FilterActive(now)
            .ToApiList()
            .OrderBy(x => x.StartTime)
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<AppReservation>> GetAllRaw(int hostId, DateTime from, DateTime to)
    {
        return await QueryConfirmed()
            .Where(x => x.HostId == hostId)
            .Where(x => x.StartTime >= from)
            .Where(x => x.EndTime <= to)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<AppReservation>> GetAllRawByPackageId(
        int packageId,
        CReservationStateEnum? state = null
    )
    {
        return await QueryAll()
            .Where(x => x.ReservationPackageId == packageId)
            .Where(x => state == null || x.ReservationStateId == (int)state)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<AppReservationVM>> GetAll(IEnumerable<int> reservationIds)
    {
        return await QueryConfirmed()
            .Where(x => reservationIds.Contains(x.ReservationId))
            .ToApiList()
            .ToListAsync();
    }

    public async Task<int> Add(AppReservationIM input)
    {
        var now = _timeUtility.GetUtcNow();
        var newReservation = new AppReservation
        {
            ReservationPackageId = input.ReservationPackageId,
            HostId = input.TutorId,
            AttenderId = input.StudentId,
            Price = input.Price,
            PriceWithoutSales = input.PriceWithoutSales,
            StartTime = input.StartTime,
            EndTime = input.EndTime,
            ExpectedEndTime = input.ExpectedEndTime,
            SubjectId = input.SubjectId,
            ReservationStateId = (int)input.ReservationStateEnum,
            Created = now,
            Updated = now,
            RoomId = input.RoomId,
            HostEventIanaTimezone = input.HostEventIanaTimezone,
            CurrencyId = (int)CCurrencyEnum.Czk,
            SourceReservationId = input.SourceReservationId,
            ClassDurationId = (int)input.ClassDurationEnum,
            CouponId = input.CouponId,
            CouponSaleAmount = input.CouponSaleAmount
        };

        _db.AppReservations.Add(newReservation);
        await _db.SaveChangesAsync();

        return newReservation.ReservationId;
    }

    public async Task<AppReservation> GetRawDbEntity(int reservationId)
    {
        var entity = await QueryAll()
            .Where(x => x.ReservationId == reservationId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(reservationId, entity);

        return entity;
    }

    public Task Remove(int reservationId)
    {
        var reservation = new AppReservation { ReservationId = reservationId };
        _db.AppReservations.Remove(reservation);
        return _db.SaveChangesAsync();
    }

    public async Task<PagedVM<AppUserCommonVM>> GetTutors(int studentId)
    {
        var result = await QueryConfirmed()
            .Where(x => x.AttenderId == studentId)
            .Select(x => x.Host)
            .Distinct()
            .ToCommonList()
            .ToListAsync();

        return new PagedVM<AppUserCommonVM>(result);
    }

    public async Task<PagedVM<AppUserCommonVM>> GetStudents(int tutorId)
    {
        var result = await QueryConfirmed()
            .Where(x => x.HostId == tutorId)
            .Select(x => x.Attender)
            .Distinct()
            .ToCommonList()
            .ToListAsync();

        return new PagedVM<AppUserCommonVM>(result);
    }

    public async Task<int> GetAttendancePercentage(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var totalPastReservationsCount = await QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Where(x => x.EndTime < now)
            .CountAsync();

        var nonVisitedCount = await QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Where(x => x.EndTime < now)
            .Where(x => !x.AppRoom.AppRoomActivities.Any())
            .CountAsync();

        return GetAttendanceScore(totalPastReservationsCount, nonVisitedCount);
    }

    public Task<int> GetTutorNumberOfCompletedLessons(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Where(x => x.EndTime < now)
            .CountAsync();
    }

    public Task<int> GetStudentNumberOfCompetedLessons(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .Where(x => x.AttenderId == userId)
            .Where(x => x.EndTime < now)
            .CountAsync();
    }

    public Task<int> GetTutorNumberOfUpcomingLessonsThisMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var monthStart = new DateTime(now.Year, now.Month, 1).ToUniversalTime();
        var monthEnd = monthStart.AddMonths(1);

        return QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Where(x => x.EndTime < monthEnd)
            .CountAsync();
    }

    public Task<int> GetStudentNumberOfUpcomingLessonsThisMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var monthStart = new DateTime(now.Year, now.Month, 1).ToUniversalTime();
        var monthEnd = monthStart.AddMonths(1);

        return QueryConfirmed()
            .Where(x => x.AttenderId == userId)
            .Where(x => x.EndTime < monthEnd)
            .CountAsync();
    }

    public Task<int> GetNumberOfNewStudentsThisMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var startOfMonth = new DateTime(now.Year, now.Month, 1).ToUniversalTime();
        return QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Where(x => x.StartTime >= startOfMonth)
            .Select(x => x.AttenderId)
            .Distinct()
            .CountAsync();
    }

    public Task<int> GetNumberOfNewStudentsPreviousMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var startOfMonth = new DateTime(now.Year, now.Month, 1).ToUniversalTime().AddMonths(-1);
        var endOfMonth = new DateTime(now.Year, now.Month, 1).ToUniversalTime();
        return QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Where(x => x.StartTime >= startOfMonth)
            .Where(x => x.StartTime < endOfMonth)
            .Select(x => x.AttenderId)
            .Distinct()
            .CountAsync();
    }

    public Task<int> GetTutorNumberOfUpcomingLessons(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Where(x => x.StartTime >= now)
            .CountAsync();
    }

    public Task<int> GetStudentNumberOfUpcomingLessons(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .Where(x => x.AttenderId == userId)
            .Where(x => x.StartTime >= now)
            .CountAsync();
    }

    public Task<int> GetNumberOfStudents(int userId)
    {
        return QueryConfirmed()
            .Where(x => x.HostId == userId)
            .Select(x => x.AttenderId)
            .Distinct()
            .CountAsync();
    }

    public async Task<IReadOnlyDictionary<int, int>> GetAttendancePercentageByUserId(IReadOnlyCollection<int> userIds)
    {
        var now = _timeUtility.GetUtcNow();

        var totalPastReservationsByUserId = await QueryConfirmed()
            .Where(x => userIds.Contains(x.HostId))
            .Where(x => x.EndTime < now)
            .GroupBy(x => x.HostId)
            .Select(x => new
            {
                UserId = x.Key,
                Count = x.Count()
            }).ToListAsync();

        var nonVisitedCountByUserId = (await QueryConfirmed()
            .Where(x => userIds.Contains(x.HostId))
            .Where(x => x.EndTime < now)
            .Where(x => !x.AppRoom.AppRoomActivities.Any())
            .GroupBy(x => x.HostId)
            .Select(x => new
            {
                UserId = x.Key,
                Count = x.Count()
            }).ToListAsync()).ToDictionary(x => x.UserId, x => x.Count);

        return totalPastReservationsByUserId
            .Select(x => new
            {
                x.UserId,
                TotalCount = x.Count,
                NonVisitedCount = nonVisitedCountByUserId.TryGetValue(x.UserId, out var value) ? value : 0
            })
            .ToDictionary(
                x => x.UserId,
                x => GetAttendanceScore(x.TotalCount, x.NonVisitedCount)
            );
    }

    public async Task<IReadOnlyDictionary<int, int>> GetTutorNumberOfCompletedLessonsByUserId(
        IReadOnlyCollection<int> userIds
    )
    {
        var now = _timeUtility.GetUtcNow();
        var results = await QueryConfirmed()
            .Where(x => userIds.Contains(x.HostId))
            .Where(x => x.EndTime < now)
            .GroupBy(x => x.HostId)
            .Select(x => new
            {
                UserId = x.Key,
                Count = x.Count()
            })
            .ToListAsync();

        return results.ToDictionary(x => x.UserId, x => x.Count);
    }

    public async Task<IReadOnlyDictionary<int, int>> GetNumberOfStudentsByUserId(IReadOnlyCollection<int> userIds)
    {
        var results = await QueryConfirmed()
            .Where(x => userIds.Contains(x.HostId))
            .Select(x => new { x.HostId, x.AttenderId })
            .Distinct()
            .GroupBy(x => x.HostId)
            .Select(x => new { UserId = x.Key, Count = x.Count() })
            .ToListAsync();

        return results.ToDictionary(x => x.UserId, x => x.Count);
    }

    public Task<int> GetStudentNumberOfLessonsThisWeek(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .FilterCurrentWeek(now)
            .Where(x => x.AttenderId == userId)
            .CountAsync();
    }

    public Task<int> GetStudentNumberOfLessonsPreviousWeek(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .FilterPreviousWeek(now)
            .Where(x => x.AttenderId == userId)
            .CountAsync();
    }

    public Task<int> GetCompanyNumberOfCompletedLessons(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .Where(x => x.Attender.InviterId == userId)
            .Where(x => x.EndTime < now)
            .CountAsync();
    }

    public Task<int> GetCompanyNumberOfLessonsThisMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var startTime = new DateTime(now.Year, now.Month, 1).ToUniversalTime();
        var endTime = startTime.AddMonths(1);

        return QueryConfirmed()
            .Where(x => x.AttenderId == userId)
            .Where(x => x.StartTime >= startTime)
            .Where(x => x.EndTime < endTime)
            .CountAsync();
    }

    public Task<int> GetCompanyNumberOfLessonsPreviousMonth(int userId)
    {
        var now = _timeUtility.GetUtcNow();
        var endTime = new DateTime(now.Year, now.Month, 1).ToUniversalTime();
        var startTime = endTime.AddMonths(-1);

        return QueryConfirmed()
            .Where(x => x.AttenderId == userId)
            .Where(x => x.StartTime >= startTime)
            .Where(x => x.EndTime < endTime)
            .CountAsync();
    }

    public async Task<IReadOnlyDictionary<DateTime, int>> GetStudentCompletedLessonsCountPerWeek(int userId)
    {
        var now = _timeUtility.GetUtcNow();

        var results = await QueryConfirmed()
            .Where(x => x.AttenderId == userId)
            .Where(x => x.StartTime <= now)
            .Select(x => x.StartTime)
            .ToListAsync();

        return results
            .Select(x => x.Date.AddDays(-TimeStaticUtility.ToIsoWeekday(x.DayOfWeek)))
            .GroupBy(x => x.Date)
            .ToDictionary(x => x.Key, x => x.Count());
    }

    public async Task<IReadOnlyDictionary<int, int>> GetStudentNumberOfCompletedLessonsByTutorId(int currentUserId)
    {
        var now = _timeUtility.GetUtcNow();
        var results = await QueryConfirmed()
            .Where(x => x.AttenderId == currentUserId)
            .Where(x => x.StartTime <= now)
            .GroupBy(x => x.HostId)
            .Select(x => new
            {
                TutorId = x.Key,
                Count = x.Count()
            })
            .ToListAsync();

        return results.ToDictionary(x => x.TutorId, x => x.Count);
    }

    public async Task<IReadOnlyDictionary<int, int>> GetTutorNumberOfCompletedLessonsByStudentId(int currentUserId)
    {
        var now = _timeUtility.GetUtcNow();

        var results = await QueryConfirmed()
            .Where(x => x.HostId == currentUserId)
            .Where(x => x.StartTime <= now)
            .GroupBy(x => x.AttenderId)
            .Select(x => new
            {
                AttenderId = x.Key,
                Count = x.Count()
            })
            .ToListAsync();

        return results.ToDictionary(x => x.AttenderId, x => x.Count);
    }

    public async Task<IReadOnlyDictionary<int, int>> GetNumberOfReservationsByPackageId(IEnumerable<int> packageIds)
    {
        var results = await QueryConfirmed()
            .Where(x => packageIds.Contains(x.ReservationPackageId))
            .GroupBy(x => x.ReservationPackageId)
            .Select(x => new
            {
                PackageId = x.Key,
                Count = x.Count()
            }).ToListAsync();

        return results.ToDictionary(e => e.PackageId, e => e.Count);
    }

    public async Task<IReadOnlyDictionary<int, int>> GetOrderNumberByReservationId(IEnumerable<int> packageIds)
    {
        var results = await QueryConfirmed()
            .Where(x => packageIds.Contains(x.ReservationPackageId))
            .OrderBy(x => x.StartTime)
            .Select(x => new
            {
                x.ReservationPackageId,
                x.ReservationId
            })
            .ToListAsync();

        return results.GroupBy(x => x.ReservationPackageId)
            .SelectMany(x => x.Select((y, yIdx) => new
            {
                y.ReservationId,
                OrderNumber = yIdx + 1
            }))
            .ToDictionary(x => x.ReservationId, x => x.OrderNumber);
    }

    public async Task<IReadOnlyDictionary<int, int>> GetNumberOfRemainingLessonsByPackageId(IEnumerable<int> packageIds)
    {
        var span = new TimeSpan(0, CommonConstants.MinimumTimeForClassCancellationForStudentInHours, 0);
        var time = _timeUtility.GetUtcNow() + span;

        var results = await QueryConfirmed()
            .Where(x => packageIds.Contains(x.ReservationPackageId))
            .Where(x => x.StartTime > time)
            .GroupBy(x => x.ReservationPackageId)
            .Select(x => new
            {
                PackageId = x.Key,
                Count = x.Count()
            })
            .ToListAsync();

        return results.ToDictionary(x => x.PackageId, x => x.Count);
    }

    public async Task<IReadOnlyCollection<AppReservation>> GetRemainingReservationsByPackageId(int packageId)
    {
        return await QueryRemaining()
            .Where(x => x.ReservationPackageId == packageId)
            .OrderBy(x => x.StartTime)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<AppReservation>> GetCompletedReservationsByPackageId(int packageId)
    {
        var now = _timeUtility.GetUtcNow();

        return await QueryConfirmed()
            .Where(x => x.ReservationPackageId == packageId)
            .Where(x => x.EndTime < now)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<AppReservation>> GetRemainingReservationsByUserId(int userId)
    {
        var query = QueryRemaining();
        var attenderQuery = query.Where(x => x.AttenderId == userId);
        var hostQuery = query.Where(x => x.HostId == userId);

        return await attenderQuery.Concat(hostQuery)
            .OrderBy(x => x.StartTime)
            .ToListAsync();
    }

    public async Task UpdateStateByPackageId(int packageId, CReservationStateEnum newStateEnum)
    {
        var now = _timeUtility.GetUtcNow();
        var reservations = await QueryAll(false)
            .Where(x => x.ReservationPackageId == packageId)
            .ToListAsync();

        foreach (var appReservation in reservations)
        {
            appReservation.Updated = now;
            appReservation.ReservationStateId = (int)newStateEnum;
        }

        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<int>> GetAllReservationIdsByPackageId(int reservationPackageId)
    {
        return await QueryAll()
            .Where(x => x.ReservationPackageId == reservationPackageId)
            .Select(x => x.ReservationId)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<AppReservationForPaymentVM>> GetPastReadyForPaymentScheduleReservations()
    {
        var now = _timeUtility.GetUtcNow();

        return await QueryByState(CReservationStateEnum.Confirmed)
            .Where(x => x.EndTime < now)
            .ToAppReservationForPaymentList()
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<AppReservationForPaymentVM>> GetPastReadyForPaymentReservations()
    {
        var now = _timeUtility.GetUtcNow();

        return await QueryByState(CReservationStateEnum.ScheduledForPayment)
            .Where(x => x.EndTime.AddDays(CommonConstants.MinimumTimeForPaymentInDays) < now)
            .ToAppReservationForPaymentList()
            .ToListAsync();
    }

    public async Task UpdateState(IEnumerable<int> reservationIds, CReservationStateEnum newState)
    {
        var now = _timeUtility.GetUtcNow();
        var reservations = await QueryAll(false)
            .Where(x => reservationIds.Contains(x.ReservationId))
            .ToListAsync();

        foreach (var appReservation in reservations)
        {
            appReservation.ReservationStateId = (int)newState;
            appReservation.Updated = now;
        }
    }

    public async Task<IReadOnlyDictionary<int, AppUserCommonVM>> GetOpponentByReservationId(
        IReadOnlyCollection<int> reservationIds, int userId)
    {
        var query = QueryAll()
            .Where(x => reservationIds.Contains(x.ReservationId));

        var attendersOpponents = await query
            .Where(x => x.HostId == userId)
            .ToOpponentList()
            .ToListAsync();

        var hostOpponents = await query
            .Where(x => x.AttenderId == userId)
            .ToOpponentList()
            .ToListAsync();

        return attendersOpponents
            .Select(x => new
            {
                x.ReservationId,
                Opponent = x.Attender
            })
            .Concat(
                hostOpponents.Select(x => new
                {
                    x.ReservationId,
                    Opponent = x.Host
                })
            )
            .ToDictionary(x => x.ReservationId, x => x.Opponent);
    }

    public async Task<IReadOnlyCollection<AppReservationForNotificationVM>> GetLessonsAboutToStartWithoutNotification(
        int lessonTimePeriodInMinutesToNotify
    )
    {
        var startInterval = _timeUtility.GetUtcNow();
        var endInterval = startInterval.AddMinutes(lessonTimePeriodInMinutesToNotify);

        return await QueryConfirmed()
            .Where(x => x.StartTime > startInterval)
            .Where(x => x.StartTime < endInterval)
            .Where(x => x.Notified == null)
            .ToAppReservationForNotificationList()
            .ToListAsync();
    }

    public async Task<AppReservationForNotificationVM> GetNotificationVmById(int reservationId)
    {
        var result = await QueryAll()
            .Where(x => x.ReservationId == reservationId)
            .ToAppReservationForNotificationList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound(reservationId, result);

        return result;
    }

    public async Task<IReadOnlyCollection<AppReservationForNotificationVM>> GetNotificationVmsByPackageId(
        int reservationPackageId)
    {
        return await QueryAll()
            .Where(x => x.ReservationPackageId == reservationPackageId)
            .ToAppReservationForNotificationList()
            .ToListAsync();
    }

    public async Task SetNotified(IEnumerable<int> reservationIds)
    {
        var now = _timeUtility.GetUtcNow();

        var entities = await QueryAll(false)
            .Where(x => reservationIds.Contains(x.ReservationId))
            .ToListAsync();

        foreach (var appReservation in entities)
        {
            appReservation.Notified = now;
        }

        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyDictionary<int, AppTransactionReservationVM>> GetTransactionReservations(
        IEnumerable<int> reservationIds)
    {
        return await QueryAll()
            .Where(x => reservationIds.Contains(x.ReservationId))
            .ToAppTransactionReservationList()
            .ToDictionaryAsync(x => x.ReservationId, x => x);
    }

    public async Task<IReadOnlyDictionary<int, AppUserEmployeeLessonStatisticsVM>> GetEmployeesLessonStatistics(
        IEnumerable<int> attenderIds
    )
    {
        var now = _timeUtility.GetUtcNow();

        var results = await QueryConfirmed()
            .FilterAttenders(attenderIds)
            .Where(x => x.EndTime < now)
            .GroupBy(x => x.AttenderId)
            .Select(x => new AppUserEmployeeLessonStatisticsVM
            {
                UserId = x.Key,
                LastLessonFromDate = x.Max(y => y.StartTime),
                LastLessonToDate = x.Max(y => y.EndTime)
            }).ToListAsync();

        return results.ToDictionary(x => x.UserId);
    }

    public async Task<IReadOnlyDictionary<int, int>> GetAttendersCurrentWeeklyGaolsProgress(
        IEnumerable<int> attenderIds
    )
    {
        var now = _timeUtility.GetUtcNow();

        return await QueryConfirmed()
            .FilterCurrentWeek(now)
            .FilterAttenders(attenderIds)
            .GroupBy(x => x.AttenderId)
            .Select(x => new
            {
                UserId = x.Key,
                Count = x.Count()
            })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);
    }

    public async Task<IReadOnlyDictionary<int, int>> GetAttendersPreviousWeeklyGaolsProgress(
        IEnumerable<int> attenderIds)
    {
        var now = _timeUtility.GetUtcNow();

        return await QueryConfirmed()
            .FilterPreviousWeek(now)
            .FilterAttenders(attenderIds)
            .GroupBy(x => x.AttenderId)
            .Select(x => new
            {
                UserId = x.Key,
                Count = x.Count()
            })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);
    }

    public Task<bool> ExistsFutureBookings(int currentUserId)
    {
        var now = _timeUtility.GetUtcNow();

        return QueryConfirmed()
            .Where(x => x.AttenderId == currentUserId)
            .Where(x => x.StartTime > now)
            .AnyAsync();
    }

    public Task<bool> ExistsAnyBookings(int currentUserId)
    {
        return QueryConfirmed()
            .Where(x => x.AttenderId == currentUserId)
            .AnyAsync();
    }

    private static int GetAttendanceScore(int totalCount, int noVisitedCount)
    {
        return totalCount > 0 ? (int)((1 - ((double)noVisitedCount / totalCount)) * 100) : 100;
    }

    private IQueryable<AppReservation> FilterAttenderAndCoAttender(IQueryable<AppReservation> query, int userId)
    {
        var attender = query.Where(x => x.AttenderId == userId);
        var coAttender = attender.Where(x => x.Attender.InviterId == userId);
        return attender.Concat(coAttender);
    }

    private IQueryable<AppReservation> QueryRemaining()
    {
        var span = new TimeSpan(0, CommonConstants.MinimumTimeForClassCancellationForStudentInHours, 0);
        var time = _timeUtility.GetUtcNow() + span;

        return QueryConfirmed()
            .Where(x => x.StartTime > time);
    }

    private IQueryable<AppReservation> QueryByState(CReservationStateEnum state, bool asNoTracking = true)
    {
        return QueryAll(asNoTracking)
            .Where(x => x.ReservationStateId == (int)state);
    }

    private IQueryable<AppReservation> QueryConfirmed(bool asNoTracking = true)
    {
        return QueryAll(asNoTracking)
            .Where(x => ConfirmedStates.Contains(x.ReservationStateId));
    }

    private IQueryable<AppReservation> QueryAll(bool asNoTracking = true)
    {
        var query = _db.AppReservations;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}