using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.Mappers;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.RepositoryExtensions;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.AppUser.Sitemap;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.Common;
using ULearn.Be.Models.ViewModels.Statistics;
using ULearn.Be.Repositories.Base;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppUserRepository : BaseRepository, IAppUserRepository
{
    private readonly HashSet<CUserRoleEnum> _allowedRoles = new()
        { CUserRoleEnum.Tutor, CUserRoleEnum.Student, CUserRoleEnum.Company };

    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;
    private readonly ICTranslationLanguageRepository _translationLanguageRepository;

    public AppUserRepository(
        RepositoryContext db,
        ITimeUtility timeUtility,
        ICTranslationLanguageRepository translationLanguageRepository
    ) : base(db)
    {
        _db = db;
        _timeUtility = timeUtility;
        _translationLanguageRepository = translationLanguageRepository;
    }

    public Task<PagedVM<AppUserOverviewVM>> Filter(
        FilterView<UserFilterVM> filter,
        CUserRoleEnum userRoleEnum,
        bool isAdmin
    )
    {
        var query = Query(true, filter.Filter.StateId == null)
            .FilterHasFutureReservations(_timeUtility.GetUtcNow(), isAdmin)
            .Where(x => x.UserRoleId == (int)userRoleEnum)
            .Where(x => isAdmin || !x.Excluded);

        if (filter.Filter.LanguageIds != null)
        {
            query = query.Where(x => x.RUserLanguages.Any(y => filter.Filter.LanguageIds.Contains(y.LanguageId)));
        }

        if (filter.Filter.SpecializationIds != null)
        {
            query = query.Where(x =>
                x.RUserSpecializations.Any(y => filter.Filter.SpecializationIds.Contains(y.SpecializationId)));
        }

        if (filter.Filter.SubjectLevelIds != null)
        {
            query = query.Where(x =>
                x.RUserSubjectLevels.Any(y => filter.Filter.SubjectLevelIds.Contains(y.SubjectLevelId)));
        }

        if (filter.Filter.InterestIds != null)
        {
            query = query.Where(x =>
                x.RUserInterests.Any(y => filter.Filter.InterestIds.Contains(y.InterestId)));
        }

        if (filter.Filter.FromTrialPrice != null)
        {
            query = query.Where(x => x.HourRate >= filter.Filter.FromTrialPrice);
        }

        if (filter.Filter.ToTrialPrice != null)
        {
            query = query.Where(x => x.HourRate <= filter.Filter.ToTrialPrice * 2);
        }

        if (filter.Filter.FromRating != null)
        {
            query = query.Where(x => x.HourRate >= filter.Filter.FromTrialPrice * 2);
        }

        if (filter.Filter.SubjectId != null)
        {
            query = query.Where(x => x.SubjectId == filter.Filter.SubjectId);
        }

        if (filter.Filter.CountryId != null)
        {
            query = query.Where(x => x.CountryId == filter.Filter.CountryId);
        }

        if (filter.Filter.Rating != null)
        {
            query = query.Where(x => AppUserMapper.GetRating(x.AppUserReviewReviewees) >= filter.Filter.Rating);
        }

        if (filter.Filter.NumberOfLessons != null)
        {
            query = query.Where(x => x.HostAppReservations.Count >= filter.Filter.NumberOfLessons);
        }

        if (filter.Filter.NumberOfRatings != null)
        {
            query = query.Where(x => x.AppUserReviewReviewees.Count >= filter.Filter.NumberOfRatings);
        }

        return query
            .ToOverviewApiList()
            .ToPagedVMAsync(filter);
    }

    public async Task<PagedVM<AppTutorCardVm>> PopularTutorsFilter(FilterView<AppTutorCardFilterVM> filter)
    {
        var queryBase = QueryConfirmed()
            .FilterHasFutureReservations(_timeUtility.GetUtcNow(), false)
            .Where(x => x.UserRoleId == (int)CUserRoleEnum.Tutor)
            .Where(x => !x.Excluded);

        var duplicateNamesUserIds = queryBase
            .ToAppTutorCardList()
            .Filter(filter)
            .GroupBy(x => new { x.FirstName, x.LastName })
            .Where(x => x.Count() > 1)
            .Select(x => x.Max(y => y.UserId))
            .ToList();

        return await queryBase
            .Where(x => !duplicateNamesUserIds.Contains(x.UserId))
            .ToAppTutorCardList()
            .ToPagedVMAsync(filter);
    }

    public async Task<PagedVM<AppTutorCardVm>> SimilarTutorsFilter(int userId, FilterView<AppTutorCardFilterVM> filter)
    {
        var userSubjectId = await QueryAll()
            .FilterHasFutureReservations(_timeUtility.GetUtcNow(), false)
            .Where(x => x.UserId == userId)
            .Select(x => x.SubjectId)
            .FirstOrDefaultAsync();

        return await QueryConfirmed()
            .Where(x => x.UserRoleId == (int)CUserRoleEnum.Tutor)
            .Where(x => !x.Excluded)
            .Where(x => x.UserId != userId)
            .Where(x => x.SubjectId == userSubjectId)
            .ToAppTutorCardList()
            .ToPagedVMAsync(filter);
    }

    public async Task<AppUserPublicVM> GetPublicInfo(int userId, bool onlyConfirmed = true)
    {
        var entity = await Query(true, onlyConfirmed)
            .Where(x => x.UserId == userId)
            .Where(x => x.UserRoleId == (int)CUserRoleEnum.Tutor)
            .ToPublicApiList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, entity);

        return entity;
    }

    public async Task<AppUserVM> Get(
        int userId,
        CUserRoleEnum? role = null,
        bool onlyConfirmed = false,
        bool throwIfNotFound = true
    )
    {
        var entity = await Query(true, onlyConfirmed)
            .Where(x => x.UserId == userId)
            .Where(x => role == null || x.UserRoleId == (int)role)
            .ToApiList()
            .FirstOrDefaultAsync();

        if (throwIfNotFound)
        {
            ThrowIfNotFound(userId, entity);
        }

        return entity;
    }

    public async Task<AppUserBasicVM> GetByLogin(string login, bool onlyConfirmed = false, bool throwIfNotFound = true)
    {
        var entity = await Query(true, onlyConfirmed)
            .Where(x => x.Login == login)
            .ToBasicApiList()
            .FirstOrDefaultAsync();

        if (throwIfNotFound)
        {
            ThrowIfNotFound("Could not find the user", entity);
        }

        return entity;
    }

    public async Task<AppUserBasicVM> GetById(
        int userId,
        bool onlyConfirmed = false,
        bool throwIfNotFound = true,
        bool includeDeleted = false
    )
    {
        var entity = await Query(onlyConfirmed: onlyConfirmed, includeDeleted: includeDeleted)
            .Where(x => x.UserId == userId)
            .ToBasicApiList()
            .FirstOrDefaultAsync();

        if (throwIfNotFound)
        {
            ThrowIfNotFound(userId, entity);
        }

        return entity;
    }

    public Task<bool> ExistsEmployee(int adminId, int employeeId)
    {
        return QueryAll()
            .Where(x => x.InviterId == adminId)
            .Where(x => x.UserId == employeeId)
            .AnyAsync();
    }

    public async Task<int> AddIfNotExists(RegistrationIM registrationIm)
    {
        ThrowIfRoleIsNotAllowed(registrationIm.UserRoleId);
        var user = await QueryAll(false)
            .Where(x => x.Login == registrationIm.Email)
            .FirstOrDefaultAsync();

        if (user?.UserStateId == (int)CUserStateEnum.Deleted)
        {
            user.UserStateId = (int)CUserStateEnum.EmailVerification;
            await _db.SaveChangesAsync();
            return user.UserId;
        }

        if (user != null)
        {
            return user.UserId;
        }

        var translationLanguageId =
            await _translationLanguageRepository.GetTranslationLanguageId(registrationIm.LanguageCode);
        var now = _timeUtility.GetUtcNow();
        var appUser = new AppUser
        {
            Email = registrationIm.Email,
            Login = registrationIm.Email,
            Password = null,
            UserStateId = (int)CUserStateEnum.EmailVerification,
            UserRoleId = (int)registrationIm.UserRoleId,
            RegistrationMethodId = (int)CAuthenticationMethodEnum.Internal,
            TranslationLanguageId = translationLanguageId,
            IanaTimezone = registrationIm.Timezone,
            Created = now,
            Excluded = false,
            CurrencyId = (int)CCurrencyEnum.Czk,
            AppUserWallets = new List<AppUserWallet>
            {
                new()
                {
                    CurrencyId = (int)CCurrencyEnum.Czk,
                    Amount = 0,
                    Updated = now
                }
            }
        };
        _db.AppUsers.Add(appUser);
        await _db.SaveChangesAsync();
        return appUser.UserId;
    }

    public async Task<AppUser> Upsert(
        AppUserIM userIm,
        int userId,
        int? inviterId = null
    )
    {
        var exists = await QueryAll(includeDeleted: true)
            .Where(x => x.UserId == userId)
            .AnyAsync();

        var newerAppUser = ToAppUser(userIm, inviterId);

        if (!exists)
        {
            _db.AppUsers.Add(newerAppUser);
            _db.AppUserWallets.Add(new AppUserWallet
            {
                User = newerAppUser,
                CurrencyId = (int)CCurrencyEnum.Czk,
                Amount = 0,
                Updated = newerAppUser.Created
            });
            await _db.SaveChangesAsync();
            _db.Entry(newerAppUser).State = EntityState.Detached;
            return newerAppUser;
        }

        return await Update(newerAppUser, userId);
    }

    public async Task<(AppUser user, bool created)> AddIfNotExists(
        AppUserIM registrationIm,
        string login,
        int? inviterId = null
    )
    {
        var user = await QueryAll(false, includeDeleted: true)
            .Where(x => x.Login == login)
            .FirstOrDefaultAsync();

        if (user?.UserStateId == (int)CUserStateEnum.Deleted)
        {
            user.UserStateId = registrationIm.UserStateId;
            await _db.SaveChangesAsync();
        }

        if (user != null)
        {
            return (user, false);
        }

        var newerAppUser = ToAppUser(registrationIm, inviterId);

        _db.AppUsers.Add(newerAppUser);
        _db.AppUserWallets.Add(new AppUserWallet
        {
            User = newerAppUser,
            CurrencyId = (int)CCurrencyEnum.Czk,
            Amount = 0,
            Updated = newerAppUser.Created
        });
        await _db.SaveChangesAsync();
        _db.Entry(newerAppUser).State = EntityState.Detached;
        return (newerAppUser, true);
    }

    public Task<bool> Exists(string login)
    {
        return QueryAll()
            .Where(x => x.Login == login)
            .AnyAsync();
    }

    public async Task UpdateState(int userId, CUserStateEnum stateEnum)
    {
        var user = await QueryAll(false)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();
        ThrowIfNotFound(userId, user);
        user!.UserStateId = (int)stateEnum;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateHubspotInfo(int userId, string hubspotContactId)
    {
        var user = await QueryAll(false, includeDeleted: true)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, user);
        user.HubspotContactId = hubspotContactId;
        user.LastSyncToHubspot = _timeUtility.GetUtcNow();

        await _db.SaveChangesAsync();
    }

    public async Task<AppUser> GetRawDbEntity(string login, bool throwIfNotFound = true)
    {
        var user = await QueryAll()
            .Where(x => x.Login == login)
            .FirstOrDefaultAsync();

        if (throwIfNotFound && user == null)
        {
            throw new NotFoundException();
        }

        return user;
    }

    public async Task<AppUser> GetRawDbEntity(int userId, bool throwIfNotFound = true, bool includeDeleted = false)
    {
        var user = await QueryAll(includeDeleted: includeDeleted)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (throwIfNotFound && user == null)
        {
            throw new NotFoundException();
        }

        return user;
    }

    public async Task<UserPriceStatisticsVM> GetPriceStatistics()
    {
        var tutorsQuery = QueryConfirmed()
            .Where(x => x.UserRoleId == (int)CUserRoleEnum.Tutor)
            .Where(x => !x.Excluded)
            .FilterHasFutureReservations(_timeUtility.GetUtcNow(), false);

        return new UserPriceStatisticsVM
        {
            MaxTrialPrice =
                (int)Math.Ceiling(PriceUtil.GetTrialPrice(await tutorsQuery.MaxAsync(x => x.HourRate ?? 0.0))),
            MinTrialPrice = (int)Math.Floor(PriceUtil.GetTrialPrice(await tutorsQuery.MinAsync(x => x.HourRate ?? 0.0)))
        };
    }

    public async Task<Dictionary<int, AppUserCommonVM>> GetUserDictionary(IEnumerable<int> userIds)
    {
        var results = await QueryAll(includeDeleted: true)
            .Where(x => userIds.Contains(x.UserId))
            .ToCommonList()
            .ToListAsync();

        return results.ToDictionary(x => x.UserId);
    }

    public async Task<double> GetHourRate(int tutorId)
    {
        var hourRate = await Query()
            .Where(x => x.UserId == tutorId)
            .Where(x => x.HourRate != null)
            .Select(x => x.HourRate)
            .FirstOrDefaultAsync();

        if (hourRate is null or 0)
        {
            throw new NotFoundException($"Could not find user with id {tutorId} or hour rate is not set");
        }

        return hourRate.Value;
    }

    public async Task<int> GetSubjectId(int tutorId)
    {
        var subjectId = await Query()
            .Where(x => x.UserId == tutorId)
            .Select(x => x.SubjectId)
            .FirstOrDefaultAsync();

        if (subjectId is null or 0)
        {
            throw new NotFoundException($"Could not find user with id {tutorId} or subject is not set");
        }

        return subjectId.Value;
    }

    public async Task<CUserRoleEnum> GetRoleId(int userId)
    {
        var userRoleId = await QueryAll()
            .Where(x => x.UserId == userId)
            .Select(x => x.UserRoleId)
            .FirstOrDefaultAsync();

        if (userRoleId == 0)
        {
            throw new NotFoundException($"Could not find a user with ID {userId}");
        }

        return (CUserRoleEnum)userRoleId;
    }

    public async Task Approve(int userId)
    {
        var user = await QueryAll(false)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, user);
        user.UserStateId = (int)CUserStateEnum.Confirmed;
        await _db.SaveChangesAsync();
    }

    public async Task Refuse(int userId)
    {
        var user = await QueryAll(false)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, user);
        user.UserStateId = (int)CUserStateEnum.Refused;
        await _db.SaveChangesAsync();
    }

    public async Task ChangeLanguage(int translationLanguageId, int userId)
    {
        var user = await QueryAll(false)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, user);
        user.TranslationLanguageId = translationLanguageId;
        await _db.SaveChangesAsync();
    }

    public async Task UpdatePassword(PasswordUpdateIM input, int userId)
    {
        var user = await QueryAll(false)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, user);

        user.Password = input.NewPassword;
        await _db.SaveChangesAsync();
    }

    public async Task<Dictionary<int, CSubjectVM>> GetUserSubjects(IEnumerable<int> userIds)
    {
        var userSubjects = await Query()
            .Where(x => userIds.Contains(x.UserId))
            .Select(x => new
            {
                x.UserId,
                Subject = CSubjectMapper.ToVm(x.TaughtSubject)
            }).ToListAsync();

        return userSubjects.ToDictionary(x => x.UserId, x => x.Subject);
    }

    public async Task<AppUserInfoForNotificationVM> GetNotificationInfo(int userId)
    {
        var userEmail = await QueryAll()
            .Where(x => x.UserId == userId)
            .ToNotificationInfoList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound(userId, userEmail);

        return userEmail;
    }

    public Task<PagedVM<AppUserEmployeeVM>> GetEmployeeFilter(
        int adminId,
        FilterView<AppUserEmployeeFilterVM> filter
    )
    {
        return QueryAll()
            .Where(x => x.InviterId == adminId)
            .ToAppUserEmployeeList()
            .ToPagedVMAsync(filter);
    }

    public async Task<IReadOnlyCollection<int>> GetEmployeeIds(int adminId)
    {
        return await QueryAll()
            .Where(x => x.InviterId == adminId)
            .Select(x => x.UserId)
            .ToListAsync();
    }

    public async Task BatchHardDelete(AppUserAdminBatchDeleteIM input)
    {
        var userIds = await QueryAll(includeDeleted: true)
            .Where(x => input.Emails.Contains(x.Login))
            .Select(x => x.UserId)
            .ToListAsync();

        var employees = await Query(includeDeleted: true, asNoTracking: false)
            .Where(x => x.InviterId != null)
            .Where(x => userIds.Contains(x.InviterId.Value))
            .ToListAsync();

        foreach (var appUser in employees)
        {
            appUser.InviterId = null;
        }

        await _db.SaveChangesAsync();
        foreach (var appUser in employees)
        {
            _db.Entry(appUser).State = EntityState.Detached;
        }

        var users = await QueryAll(includeDeleted: true)
            .Where(x => userIds.Contains(x.UserId))
            .Include(x => x.AttenderAppReservations)
            .ThenInclude(x => x.AppReservationIssues)
            .Include(x => x.AttenderAppReservations)
            .ThenInclude(x => x.AppTransactions)
            .Include(x => x.HostAppReservations)
            .ThenInclude(x => x.AppReservationIssues)
            .Include(x => x.HostAppReservations)
            .ThenInclude(x => x.AppTransactions)
            .Include(x => x.AttenderAppRooms)
            .Include(x => x.HostAppRooms)
            .Include(x => x.AppRecurrentTimeslots)
            .ThenInclude(x => x.Blacklists)
            .Include(x => x.AppTimeslots)
            .Include(x => x.AppUserReviewReviewees)
            .Include(x => x.AppUserReviewReviewers)
            .Include(x => x.AppUserWallets)
            .Include(x => x.RUserAttributes)
            .Include(x => x.RUserFiles)
            .Include(x => x.RUserFilePermissions)
            .Include(x => x.RUserLanguages)
            .Include(x => x.RUserTokens)
            .Include(x => x.RUserNotifications)
            .Include(x => x.AppUserRegistrationCodes)
            .Include(x => x.CreatorCTranslationNamespaces)
            .Include(x => x.UpdaterCTranslationNamespaces)
            .Include(x => x.CreatorCTranslationKeys)
            .Include(x => x.UpdaterCTranslationKeys)
            .Include(x => x.CreatorCTranslations)
            .Include(x => x.UpdaterCTranslations)
            .Include(x => x.SentAppMessages)
            .ThenInclude(x => x.AppMessageAttachments)
            .Include(x => x.ReceivedAppMessages)
            .ThenInclude(x => x.AppMessageAttachments)
            .Include(x => x.InitiatorAppConversations)
            .Include(x => x.OpponentAppConversations)
            .Include(x => x.AppRoomActivities)
            .Include(x => x.RUserRoomStatistics)
            .Include(x => x.RUserSpecializations)
            .Include(x => x.RUserSubjectLevels)
            .Include(x => x.RUserInterests)
            .Include(x => x.AppUserBehaviours)
            .Include(x => x.AppReservationReviews)
            .Include(x => x.AttenderReservationPackages)
            .Include(x => x.HostReservationPackages)
            .Include(x => x.CreatedAppReservationIssues)
            .Include(x => x.ResolvedAppReservationIssues)
            .Include(x => x.SentAppNotifications)
            .Include(x => x.OwnedAppNotifications)
            .Include(x => x.VisitedUserAppUserProfileBehaviours)
            .Include(x => x.UserAppUserProfileBehaviours)
            .Include(x => x.RUserNotificationBlacklistSettings)
            .Include(x => x.RUserActivatedCoupons)
            .Include(x => x.AppCoupons)
            .AsSplitQuery()
            .ToListAsync();

        var transactionsQuery = _db.AppTransactions;
        var transactions = await transactionsQuery
            .Where(x => userIds.Contains(x.UserWallet.UserId))
            .Union(transactionsQuery
                .Where(x => userIds.Contains(x.CreatorId))
            )
            .Union(transactionsQuery
                .Where(x => userIds.Contains(x.VendorId))
            )
            .Union(transactionsQuery
                .Where(x => userIds.Contains(x.CustomerId))
            ).ToListAsync();

        _db.RemoveRange(transactions);
        _db.RemoveRange(users.SelectMany(x => x.ReceivedAppMessages.SelectMany(y => y.AppMessageAttachments)));
        _db.RemoveRange(users.SelectMany(x => x.SentAppMessages.SelectMany(y => y.AppMessageAttachments)));
        _db.RemoveRange(users.SelectMany(x => x.AppRecurrentTimeslots.SelectMany(y => y.Blacklists)));
        _db.RemoveRange(users.SelectMany(x => x.AttenderAppReservations.SelectMany(y => y.AppReservationIssues)));
        _db.RemoveRange(users.SelectMany(x => x.AttenderAppReservations.SelectMany(y => y.AppTransactions)));
        _db.RemoveRange(users.SelectMany(x => x.HostAppReservations.SelectMany(y => y.AppReservationIssues)));
        _db.RemoveRange(users.SelectMany(x => x.HostAppReservations.SelectMany(y => y.AppTransactions)));

        _db.RemoveRange(users.SelectMany(x => x.AttenderAppReservations));
        _db.RemoveRange(users.SelectMany(x => x.HostAppReservations));
        _db.RemoveRange(users.SelectMany(x => x.AttenderAppRooms));
        _db.RemoveRange(users.SelectMany(x => x.HostAppRooms));
        _db.RemoveRange(users.SelectMany(x => x.AppRecurrentTimeslots));
        _db.RemoveRange(users.SelectMany(x => x.AppTimeslots));
        _db.RemoveRange(users.SelectMany(x => x.AppUserReviewReviewees));
        _db.RemoveRange(users.SelectMany(x => x.AppUserReviewReviewers));
        _db.RemoveRange(users.SelectMany(x => x.AppUserWallets));
        _db.RemoveRange(users.SelectMany(x => x.RUserAttributes));
        _db.RemoveRange(users.SelectMany(x => x.RUserFiles));
        _db.RemoveRange(users.SelectMany(x => x.RUserFilePermissions));
        _db.RemoveRange(users.SelectMany(x => x.RUserLanguages));
        _db.RemoveRange(users.SelectMany(x => x.RUserTokens));
        _db.RemoveRange(users.SelectMany(x => x.RUserNotifications));
        _db.RemoveRange(users.SelectMany(x => x.AppUserRegistrationCodes));
        _db.RemoveRange(users.SelectMany(x => x.CreatorCTranslationNamespaces));
        _db.RemoveRange(users.SelectMany(x => x.UpdaterCTranslationNamespaces));
        _db.RemoveRange(users.SelectMany(x => x.CreatorCTranslationKeys));
        _db.RemoveRange(users.SelectMany(x => x.UpdaterCTranslationKeys));
        _db.RemoveRange(users.SelectMany(x => x.CreatorCTranslations));
        _db.RemoveRange(users.SelectMany(x => x.UpdaterCTranslations));
        _db.RemoveRange(users.SelectMany(x => x.SentAppMessages));
        _db.RemoveRange(users.SelectMany(x => x.ReceivedAppMessages));
        _db.RemoveRange(users.SelectMany(x => x.InitiatorAppConversations));
        _db.RemoveRange(users.SelectMany(x => x.OpponentAppConversations));
        _db.RemoveRange(users.SelectMany(x => x.AppRoomActivities));
        _db.RemoveRange(users.SelectMany(x => x.RUserRoomStatistics));
        _db.RemoveRange(users.SelectMany(x => x.RUserSpecializations));
        _db.RemoveRange(users.SelectMany(x => x.RUserSubjectLevels));
        _db.RemoveRange(users.SelectMany(x => x.RUserInterests));
        _db.RemoveRange(users.SelectMany(x => x.AppUserBehaviours));
        _db.RemoveRange(users.SelectMany(x => x.AppReservationReviews));
        _db.RemoveRange(users.SelectMany(x => x.AttenderReservationPackages));
        _db.RemoveRange(users.SelectMany(x => x.HostReservationPackages));
        _db.RemoveRange(users.SelectMany(x => x.CreatedAppReservationIssues));
        _db.RemoveRange(users.SelectMany(x => x.ResolvedAppReservationIssues));
        _db.RemoveRange(users.SelectMany(x => x.OwnedAppNotifications));
        _db.RemoveRange(users.SelectMany(x => x.SentAppNotifications));
        _db.RemoveRange(users.SelectMany(x => x.InvitedUsers));
        _db.RemoveRange(users.SelectMany(x => x.UserAppUserProfileBehaviours));
        _db.RemoveRange(users.SelectMany(x => x.VisitedUserAppUserProfileBehaviours));
        _db.RemoveRange(users.SelectMany(x => x.RUserNotificationBlacklistSettings));
        _db.RemoveRange(users.SelectMany(x => x.RUserActivatedCoupons));
        _db.RemoveRange(users.SelectMany(x => x.AppCoupons));
        _db.RemoveRange(users);

        await _db.SaveChangesAsync();
    }

    public Task<List<AppUserSitemapVM>> GetTutorSitemapInfo()
    {
        return QueryConfirmed()
            .Where(x => x.UserRoleId == (int)CUserRoleEnum.Tutor)
            .Where(x => !x.Excluded)
            .ToAppUserSitemapList()
            .ToListAsync();
    }

    public Task<AppTutorAdminInfoVM> GetTutorAdminInfo(int userId)
    {
        return QueryAll()
            .Where(x => x.UserId == userId)
            .ToAppTutorAdminStatisticsList()
            .FirstOrDefaultAsync();
    }

    public Task<int> GetNumberOfEmployees(int userId)
    {
        return QueryAll()
            .Where(x => x.InviterId == userId)
            .CountAsync();
    }

    public async Task<IReadOnlyDictionary<int, int>> GetEmployeesGoalsByUserId(int userId)
    {
        return await QueryAll()
            .Where(x => x.InviterId == userId)
            .Where(x => x.WeeklyGoalId != null)
            .Select(x => new
            {
                x.UserId,
                GoalId = x.WeeklyGoalId.Value
            })
            .ToDictionaryAsync(x => x.UserId, x => x.GoalId);
    }

    public async Task UpdateEmployeeAccount(AppEmployeeIM input)
    {
        var appUser = await QueryAll(false)
            .Where(x => x.UserId == input.UserId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(input.UserId, appUser);

        appUser.WeeklyGoalId = input.WeeklyGoalId;
        await _db.SaveChangesAsync();
    }

    public async Task AdminUpdate(AppUserAdminIM input)
    {
        var appUser = await QueryAll(false)
            .Where(x => x.UserId == input.UserId)
            .FirstOrDefaultAsync();

        ThrowIfNotFound(input.UserId, appUser);

        appUser.CertificationsVerified = input.CertificationsVerified;
        appUser.OrderNumber = input.OrderNumber;
        appUser.Excluded = input.Excluded;

        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<int>> GetWelcomeEmailUserIdsToSend()
    {
        var time = _timeUtility.GetUtcNow() - TimeSpan.FromHours(CommonConstants.WelcomeEmailHourDuration);

        return await QueryConfirmed()
            .Where(x => x.UserRoleId == (int)CUserRoleEnum.Student)
            .Where(x => x.Created <= time)
            .Where(x =>
                x.RUserNotifications
                    .Where(y => y.NotificationTypeId == (int)CNotificationTypeEnum.Welcome)
                    .All(y => y.NotificationChannelId != (int)CNotificationChannelEnum.Email))
            .Select(x => x.UserId)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<int>> GetDiscountCouponUserIdsToSend(
        CCouponTypeEnum couponTypeEnum,
        int discountPercentage,
        int durationInDaysAfterRegistration
    )
    {
        var time = _timeUtility.GetUtcNow() - TimeSpan.FromDays(durationInDaysAfterRegistration);

        return await QueryAll()
            .Where(x => x.InviterId == null)
            .Where(x => x.UserRoleId == (int)CUserRoleEnum.Student)
            .Where(x => !x.AttenderAppReservations.Any())
            .Where(x => x.Created <= time)
            .Where(x => x.CouponLastSentTime == null || x.CouponLastSentTime <= time)
            .Where(x => !x.AppCoupons.Any(y =>
                y.CouponTypeId == (int)couponTypeEnum && y.DiscountPercentage == discountPercentage))
            .Where(x => x.RUserActivatedCoupons.All(y => y.AppCoupon.CouponTypeId != (int)couponTypeEnum))
            .Select(x => x.UserId)
            .ToListAsync();
    }

    public async Task UpdateCouponLastSendTime(IReadOnlyCollection<int> userIds)
    {
        var now = _timeUtility.GetUtcNow();

        var users = await QueryAll(false)
            .Where(x => userIds.Contains(x.UserId))
            .ToListAsync();

        foreach (var appUser in users)
        {
            appUser.CouponLastSentTime = now;
        }

        await _db.SaveChangesAsync();
    }

    private async Task<AppUser> Update(AppUser newerAppUser, int userId)
    {
        var dbAppUser = await QueryAll(false)
            .Where(x => x.UserId == userId)
            .Include(x => x.RUserAttributes)
            .Include(x => x.RUserLanguages)
            .Include(x => x.RUserFiles)
            .Include(x => x.AppRecurrentTimeslots)
            .Include(x => x.RUserSpecializations)
            .Include(x => x.RUserSubjectLevels)
            .Include(x => x.RUserInterests)
            .FirstAsync();

        var (deletedUserAttributes, updatedUserAttributes) = CompareUtil.GetUpdatedCollection(
            dbAppUser.RUserAttributes,
            newerAppUser.RUserAttributes,
            x => x.UserAttributeTypeId,
            (x, y) => { x.Value = y.Value; }
        );
        var (deletedUserLanguages, updatedUserLanguages) = CompareUtil.GetUpdatedCollection(
            dbAppUser.RUserLanguages,
            newerAppUser.RUserLanguages,
            x => x.LanguageId,
            (x, y) => { x.LanguageLevelId = y.LanguageLevelId; }
        );
        var (deletedUserSpecializations, updatedUserSpecializations) = CompareUtil.GetUpdatedCollection(
            dbAppUser.RUserSpecializations,
            newerAppUser.RUserSpecializations,
            x => x.SpecializationId,
            (_, _) => { }
        );
        var (deletedUserSubjectLevels, updatedUserSubjectLevels) = CompareUtil.GetUpdatedCollection(
            dbAppUser.RUserSubjectLevels,
            newerAppUser.RUserSubjectLevels,
            x => x.SubjectLevelId,
            (_, _) => { }
        );
        var (deletedUserInterests, updatedUserInterests) = CompareUtil.GetUpdatedCollection(
            dbAppUser.RUserInterests,
            newerAppUser.RUserInterests,
            x => x.InterestId,
            (_, _) => { }
        );
        dbAppUser.Email = newerAppUser.Email;
        dbAppUser.Login = newerAppUser.Login;
        dbAppUser.Name = newerAppUser.Name;
        dbAppUser.FirstName = newerAppUser.FirstName;
        dbAppUser.LastName = newerAppUser.LastName;
        dbAppUser.Phone = newerAppUser.Phone;
        dbAppUser.IanaTimezone = newerAppUser.IanaTimezone;
        dbAppUser.CountryId = newerAppUser.CountryId;
        dbAppUser.TeachingExperienceId = newerAppUser.TeachingExperienceId;
        dbAppUser.CurrencyId = newerAppUser.CurrencyId;
        dbAppUser.HourRate = newerAppUser.HourRate;
        dbAppUser.SubjectId = newerAppUser.SubjectId;
        dbAppUser.UserStateId = newerAppUser.UserStateId;
        dbAppUser.UserRoleId = newerAppUser.UserRoleId;
        dbAppUser.WeeklyGoalId = newerAppUser.WeeklyGoalId;
        dbAppUser.Ico = newerAppUser.Ico;
        dbAppUser.Dic = newerAppUser.Dic;
        dbAppUser.TranslationLanguageId = newerAppUser.TranslationLanguageId;
        dbAppUser.PlaceOfBusiness = newerAppUser.PlaceOfBusiness;
        dbAppUser.CompanyName = newerAppUser.CompanyName;
        dbAppUser.BillingEmail = newerAppUser.BillingEmail;
        dbAppUser.IsVatPayer = newerAppUser.IsVatPayer;
        dbAppUser.RUserAttributes = updatedUserAttributes.ToList();
        dbAppUser.RUserLanguages = updatedUserLanguages.ToList();
        dbAppUser.RUserSpecializations = updatedUserSpecializations.ToList();
        dbAppUser.RUserSubjectLevels = updatedUserSubjectLevels.ToList();
        dbAppUser.RUserInterests = updatedUserInterests.ToList();

        _db.RemoveRange(deletedUserAttributes);
        _db.RemoveRange(deletedUserLanguages);
        _db.RemoveRange(deletedUserSpecializations);
        _db.RemoveRange(deletedUserSubjectLevels);
        _db.RemoveRange(deletedUserInterests);
        await _db.SaveChangesAsync();
        _db.Entry(dbAppUser).State = EntityState.Detached;
        return dbAppUser;
    }

    private AppUser ToAppUser(AppUserIM userIm, int? inviterId = null)
    {
        ThrowIfRoleIsNotAllowed(userIm.UserRoleId);
        return new AppUser
        {
            Email = userIm.Email,
            Login = userIm.Login,
            Name = userIm.Name,
            FirstName = userIm.FirstName,
            LastName = userIm.LastName,
            Created = _timeUtility.GetUtcNow(),
            IanaTimezone = userIm.Timezone,
            Ico = userIm.Ico,
            Dic = userIm.Dic,
            PlaceOfBusiness = userIm.PlaceOfBusiness,
            CountryId = userIm.CountryId,
            TeachingExperienceId = userIm.TeachingExperienceId,
            Phone = userIm.Phone,
            UserRoleId = userIm.UserRoleId,
            UserStateId = userIm.UserStateId,
            RegistrationMethodId = userIm.AuthenticationMethodId,
            SubjectId = userIm.SubjectId,
            WeeklyGoalId = userIm.WeeklyGoalId,
            InviterId = inviterId,
            CurrencyId = userIm.CurrencyId,
            HourRate = userIm.HourRate,
            TranslationLanguageId = userIm.TranslationLanguageId,
            CompanyName = userIm.CompanyName,
            BillingEmail = userIm.BillingEmail,
            IsVatPayer = userIm.IsVatPayer,
            RUserAttributes = userIm.Attributes?.Select(
                x => new RUserAttribute
                {
                    UserAttributeTypeId = x.AttributeTypeId,
                    Value = x.Value
                }
            ).ToList(),
            RUserLanguages = userIm.Languages?.Select(
                x => new RUserLanguage
                {
                    LanguageId = x.LanguageId,
                    LanguageLevelId = x.LanguageLevelId
                }
            ).ToList(),
            RUserSpecializations = userIm.SpecializationIds?.Select(
                x => new RUserSpecialization
                {
                    SpecializationId = x
                }).ToList(),
            RUserSubjectLevels = userIm.SubjectLevelIds?.Select(
                x => new RUserSubjectLevel
                {
                    SubjectLevelId = x
                }).ToList(),
            RUserInterests = userIm.InterestIds?.Select(
                x => new RUserInterest
                {
                    InterestId = x
                }).ToList()
        };
    }

    private void ThrowIfRoleIsNotAllowed(int userRoleId)
    {
        var allowedRoleIds = _allowedRoles.Select(x => (int)x);
        if (!allowedRoleIds.Contains(userRoleId))
        {
            throw new ForbiddenException();
        }
    }

    private void ThrowIfRoleIsNotAllowed(CUserRoleEnum userRoleEnum)
    {
        if (!_allowedRoles.Contains(userRoleEnum))
        {
            throw new ForbiddenException();
        }
    }

    private IQueryable<AppUser> Query(bool asNoTracking = true, bool onlyConfirmed = true, bool includeDeleted = false)
    {
        return onlyConfirmed && !includeDeleted
            ? QueryConfirmed(asNoTracking)
            : QueryAll(asNoTracking, includeDeleted: includeDeleted);
    }

    private IQueryable<AppUser> QueryConfirmed(bool asNoTracking = true)
    {
        return QueryAll(asNoTracking)
            .Where(x => x.UserStateId == (int)CUserStateEnum.Confirmed);
    }

    private IQueryable<AppUser> QueryAll(bool asNoTracking = true, bool includeDeleted = false)
    {
        var query = _db.AppUsers
            .Where(x => includeDeleted || x.UserStateId != (int)CUserStateEnum.Deleted);

        return asNoTracking ? query.AsNoTracking() : query;
    }
}