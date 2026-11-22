using System.Linq.Expressions;
using ULearn.Be.Common.Constants;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.Mappers.Codetables;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.AppUser.Employee;
using ULearn.Be.Models.ViewModels.AppUser.Sitemap;
using ULearn.Be.Models.ViewModels.Common;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.Mappers;

public static class AppUserMapper
{
    private static readonly Expression<Func<AppUser, AppUserBasicVM>> AsBasicVM =
        dataItem => new AppUserBasicVM
        {
            UserId = dataItem.UserId,
            Login = dataItem.Login,
            Email = dataItem.Email,
            Name = dataItem.Name,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            Created = dataItem.Created,
            Phone = dataItem.Phone,
            UserRoleId = dataItem.UserRoleId,
            StateId = dataItem.UserStateId,
            HourRate = dataItem.HourRate,
            Ico = dataItem.Ico,
            Dic = dataItem.Dic,
            IsVatPayer = dataItem.IsVatPayer,
            PlaceOfBusiness = dataItem.PlaceOfBusiness,
            CompanyName = dataItem.CompanyName,
            BillingEmail = dataItem.BillingEmail,
            TranslationLanguageId = dataItem.TranslationLanguageId,
            Timezone = dataItem.IanaTimezone,
            InviterId = dataItem.InviterId,
            SubjectId = dataItem.SubjectId,
            CountOfUnreadMessages = dataItem.ReceivedAppMessages
                .Count(x => x.MessageStateId == (int)CMessageStateEnum.Created),
            CountOfUnreadNotifications = dataItem.OwnedAppNotifications
                .Count(x => x.NotificationStateId == (int)CNotificationStateEnum.Created),
            CertificationsVerified = dataItem.CertificationsVerified,
            Files = dataItem.RUserFiles
                .OrderBy(x => x.FileTypeId)
                .Select(x => new UserFileVM
                {
                    FileTypeId = x.FileTypeId,
                    ContentType = x.ContentType,
                    FilePath = x.FilePath,
                    FileName = x.FileName
                })
        };

    private static readonly Expression<Func<AppUser, AppUserVM>> AsVM =
        dataItem => new AppUserVM
        {
            UserId = dataItem.UserId,
            Login = dataItem.Login,
            Email = dataItem.Email,
            Name = dataItem.Name,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            Created = dataItem.Created,
            Phone = dataItem.Phone,
            UserRoleId = dataItem.UserRoleId,
            StateId = dataItem.UserStateId,
            Timezone = dataItem.IanaTimezone,
            Ico = dataItem.Ico,
            Dic = dataItem.Dic,
            IsVatPayer = dataItem.IsVatPayer,
            PlaceOfBusiness = dataItem.PlaceOfBusiness,
            CompanyName = dataItem.CompanyName,
            BillingEmail = dataItem.BillingEmail,
            TranslationLanguageId = dataItem.TranslationLanguageId,
            SubjectId = dataItem.SubjectId,
            CertificationsVerified = dataItem.CertificationsVerified,

            Country = dataItem.CCountry != null
                ? CCountryMapper.ToVm(dataItem.CCountry)
                : null,
            TeachingExperience = dataItem.CTeachingExperience != null
                ? CTeachingExperienceMapper.ToVm(dataItem.CTeachingExperience)
                : null,
            WeeklyGoal = dataItem.CWeeklyGoal != null
                ? CWeeklyGoalMapper.ToVm(dataItem.CWeeklyGoal)
                : null,
            Currency = dataItem.CCurrency != null
                ? CCurrencyMapper.ToVm(dataItem.CCurrency)
                : null,
            Subject = dataItem.TaughtSubject != null
                ? CSubjectMapper.ToVm(dataItem.TaughtSubject)
                : null,
            HourRate = dataItem.HourRate,
            Rating = dataItem.AppUserReviewReviewees.Any()
                ? dataItem.AppUserReviewReviewees.Average(x => x.Rate)
                : CommonConstants.MaxRating
        };

    private static readonly Expression<Func<AppUser, AppUserOverviewVM>> AsOverviewVM =
        dataItem => new AppUserOverviewVM
        {
            UserId = dataItem.UserId,
            Name = dataItem.Name,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            Created = dataItem.Created,
            RoleId = dataItem.UserRoleId,
            StateId = dataItem.UserStateId,
            Timezone = dataItem.IanaTimezone,
            CountryId = dataItem.CountryId,
            Rating = dataItem.AppUserReviewReviewees.Any()
                ? dataItem.AppUserReviewReviewees.Average(x => x.Rate)
                : CommonConstants.MaxRating,
            NumberOfRatings = dataItem.AppUserReviewReviewees.Count,
            NumberOfLessons = dataItem.HostAppReservations.Count,
            OrderNumber = dataItem.OrderNumber,
            Country = dataItem.CCountry != null ? CCountryMapper.ToVm(dataItem.CCountry) : null,
            Currency = dataItem.CCurrency != null ? CCurrencyMapper.ToVm(dataItem.CCurrency) : null,
            Subject = dataItem.TaughtSubject != null
                ? CSubjectMapper.ToVm(dataItem.TaughtSubject)
                : null,
            HourRate = dataItem.HourRate,
            SubjectId = dataItem.SubjectId,
            CertificationsVerified = dataItem.CertificationsVerified,
        };

    private static readonly Expression<Func<AppUser, AppTutorCardVm>> AsTutorCardVM =
        dataItem => new AppTutorCardVm
        {
            UserId = dataItem.UserId,
            Name = dataItem.Name,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            Created = dataItem.Created,
            RoleId = dataItem.UserRoleId,
            StateId = dataItem.UserStateId,
            Timezone = dataItem.IanaTimezone,
            CountryId = dataItem.CountryId,
            Rating = dataItem.AppUserReviewReviewees.Any()
                ? dataItem.AppUserReviewReviewees.Average(x => x.Rate)
                : CommonConstants.MaxRating,
            OrderNumber = dataItem.OrderNumber,
            NumberOfRatings = dataItem.AppUserReviewReviewees.Count,
            NumberOfLessons = dataItem.HostAppReservations.Count,
            HasConfiguredAvailability = dataItem.AppTimeslots.Any() || dataItem.AppRecurrentTimeslots.Any(),
            Country = dataItem.CCountry != null ? CCountryMapper.ToVm(dataItem.CCountry) : null,
            Currency = dataItem.CCurrency != null ? CCurrencyMapper.ToVm(dataItem.CCurrency) : null,
            Subject = dataItem.TaughtSubject != null
                ? CSubjectMapper.ToVm(dataItem.TaughtSubject)
                : null,
            HourRate = dataItem.HourRate,
            SubjectId = dataItem.SubjectId,
            CertificationsVerified = dataItem.CertificationsVerified,
            Files = dataItem.RUserFiles
                .OrderBy(x => x.FileTypeId)
                .Select(x => new UserFileVM
                {
                    FileTypeId = x.FileTypeId,
                    ContentType = x.ContentType,
                    FilePath = x.FilePath,
                    FileName = x.FileName
                }),
        };

    public static readonly Expression<Func<AppUser, AppUserCommonVM>> AsCommonVM =
        dataItem => new AppUserCommonVM
        {
            UserId = dataItem.UserId,
            Email = dataItem.Email,
            Ico = dataItem.Ico,
            Dic = dataItem.Dic,
            IsVatPayer = dataItem.IsVatPayer,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            UserRoleId = dataItem.UserRoleId,
            TranslationLanguageId = dataItem.TranslationLanguageId,
            Timezone = dataItem.IanaTimezone,
            CertificationsVerified = dataItem.CertificationsVerified,
            Files = dataItem.RUserFiles
                .OrderBy(x => x.FileTypeId)
                .Select(x => new UserFileVM
                {
                    FileTypeId = x.FileTypeId,
                    ContentType = x.ContentType,
                    FilePath = x.FilePath,
                    FileName = x.FileName
                })
        };

    public static readonly Expression<Func<AppUser, AppUserSitemapVM>> AsSitemapVm =
        dataItem => new AppUserSitemapVM
        {
            UserId = dataItem.UserId,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            Subject = dataItem.TaughtSubject.EnumKey
        };

    public static readonly Expression<Func<AppUser, AppUserInfoForNotificationVM>> AsNotificationInfoVM =
        dataItem => new AppUserInfoForNotificationVM
        {
            Email = dataItem.Email,
            RoleEnumKey = dataItem.CUserRole.EnumKey,
            TranslationLanguageId = dataItem.TranslationLanguageId
        };

    public static readonly Expression<Func<AppUser, AppUserEmployeeVM>> AsAppUserColleagueVM =
        dataItem => new AppUserEmployeeVM
        {
            UserId = dataItem.UserId,
            Email = dataItem.Email,
            Ico = dataItem.Ico,
            Dic = dataItem.Dic,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            UserRoleId = dataItem.UserRoleId,
            UserStateId = dataItem.UserStateId,
            TranslationLanguageId = dataItem.TranslationLanguageId,
            WeeklyGoalId = dataItem.WeeklyGoalId,
            Timezone = dataItem.IanaTimezone,
            State = CUserStateMapper.ToVm(dataItem.UserState),
            Files = dataItem.RUserFiles
                .OrderBy(x => x.FileTypeId)
                .Select(x => new UserFileVM
                {
                    FileTypeId = x.FileTypeId,
                    ContentType = x.ContentType,
                    FilePath = x.FilePath,
                    FileName = x.FileName
                })
        };

    public static readonly Expression<Func<AppUser, AppUserPublicVM>> AsPublicVM =
        dataItem => new AppUserPublicVM
        {
            UserId = dataItem.UserId,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            Timezone = dataItem.IanaTimezone,
            SubjectId = dataItem.SubjectId,
            UserRoleId = dataItem.UserRoleId,
            CertificationsVerified = dataItem.CertificationsVerified,
            TeachesLessonsForChildren = dataItem.RUserSpecializations.Any(x => x.Specialization.IsLessonForChildrenType),
            Country = dataItem.CCountry != null
                ? CCountryMapper.ToVm(dataItem.CCountry)
                : null,
            TeachingExperience = dataItem.CTeachingExperience != null
                ? CTeachingExperienceMapper.ToVm(dataItem.CTeachingExperience)
                : null,
            WeeklyGoal = dataItem.CWeeklyGoal != null
                ? CWeeklyGoalMapper.ToVm(dataItem.CWeeklyGoal)
                : null,
            Currency = dataItem.CCurrency != null
                ? CCurrencyMapper.ToVm(dataItem.CCurrency)
                : null,
            Subject = dataItem.TaughtSubject != null
                ? CSubjectMapper.ToVm(dataItem.TaughtSubject)
                : null,
            HourRate = dataItem.HourRate,
            Rating = dataItem.AppUserReviewReviewees.Any()
                ? dataItem.AppUserReviewReviewees.Average(x => x.Rate)
                : CommonConstants.MaxRating
        };

    public static readonly Expression<Func<AppUser, AppTutorAdminInfoVM>> AsAppTutorAdminStatisticsVM =
        dataItem => new AppTutorAdminInfoVM
        {
            Email = dataItem.Email,
            Phone = dataItem.Phone,
            Ico = dataItem.Ico,
            IsVatPayer = dataItem.IsVatPayer,
            Dic = dataItem.Dic,
            BusinessAddress = dataItem.PlaceOfBusiness,
            OrderNumber = dataItem.OrderNumber,
            Excluded = dataItem.Excluded
        };

    public static double GetRating(ICollection<AppUserReview> reviews)
    {
        return reviews.Any()
            ? reviews.Average(x => x.Rate)
            : CommonConstants.MaxRating;
    }

    public static AppUserCommonVM ToCommonVm(AppUser dataItem)
    {
        return new AppUserCommonVM
        {
            UserId = dataItem.UserId,
            Email = dataItem.Email,
            Ico = dataItem.Ico,
            Dic = dataItem.Dic,
            IsVatPayer = dataItem.IsVatPayer,
            FirstName = dataItem.FirstName,
            LastName = dataItem.LastName,
            UserRoleId = dataItem.UserRoleId,
            TranslationLanguageId = dataItem.TranslationLanguageId,
            Timezone = dataItem.IanaTimezone,
            CertificationsVerified = dataItem.CertificationsVerified,
            Files = dataItem.RUserFiles
                ?.OrderBy(x => x.FileTypeId)
                .Select(x => new UserFileVM
                {
                    FileTypeId = x.FileTypeId,
                    ContentType = x.ContentType,
                    FilePath = x.FilePath,
                    FileName = x.FileName
                })
        };
    }

    public static IQueryable<AppUserBasicVM> ToBasicApiList(this IQueryable<AppUser> dataCollection)
    {
        return dataCollection.Select(AsBasicVM);
    }

    public static IQueryable<AppUserPublicVM> ToPublicApiList(this IQueryable<AppUser> dataCollection)
    {
        return dataCollection.Select(AsPublicVM);
    }

    public static IQueryable<AppUserVM> ToApiList(this IQueryable<AppUser> dataCollection)
    {
        return dataCollection.Select(AsVM);
    }

    public static IQueryable<AppUserCommonVM> ToCommonList(this IQueryable<AppUser> dataCollection)
    {
        return dataCollection.Select(AsCommonVM);
    }

    public static IQueryable<AppUserInfoForNotificationVM> ToNotificationInfoList(
        this IQueryable<AppUser> dataCollection)
    {
        return dataCollection.Select(AsNotificationInfoVM);
    }

    public static IQueryable<AppUserOverviewVM> ToOverviewApiList(
        this IQueryable<AppUser> dataCollection
    )
    {
        return dataCollection.Select(AsOverviewVM);
    }

    public static IQueryable<AppTutorCardVm> ToAppTutorCardList(
        this IQueryable<AppUser> dataCollection
    )
    {
        return dataCollection.Select(AsTutorCardVM);
    }

    public static IQueryable<AppUserEmployeeVM> ToAppUserEmployeeList(
        this IQueryable<AppUser> dataCollection
    )
    {
        return dataCollection.Select(AsAppUserColleagueVM);
    }

    public static IQueryable<AppUserSitemapVM> ToAppUserSitemapList(
        this IQueryable<AppUser> dataCollection
    )
    {
        return dataCollection.Select(AsSitemapVm);
    }

    public static IQueryable<AppTutorAdminInfoVM> ToAppTutorAdminStatisticsList(
        this IQueryable<AppUser> dataCollection
    )
    {
        return dataCollection.Select(AsAppTutorAdminStatisticsVM);
    }
}