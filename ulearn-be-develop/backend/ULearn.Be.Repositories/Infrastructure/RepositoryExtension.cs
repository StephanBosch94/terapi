using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Data.Infrastructure;
using ULearn.Be.Data.Infrastructure.Interfaces;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories.Infrastructure;

public static class RepositoryExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        // SF
        services.AddScoped<IRUserFileRepository, RUserFileRepository>();

        // Other
        services.AddScoped<IDbContextTransactionHandler, RepositoryContextTransactionHandler>();

        // DBO
        services.AddScoped<IMasterHashRepository, MasterHashRepository>();
        services.AddScoped<ICTranslationRepository, CTranslationRepository>();
        services.AddScoped<ICCurrencyRepository, CCurrencyRepository>();
        services.AddScoped<ICTranslationKeyRepository, CTranslationKeyRepository>();
        services.AddScoped<ICTranslationLanguageRepository, CTranslationLanguageRepository>();
        services.AddScoped<ICTransactionTemplateRepository, CTransactionTemplateRepository>();
        services.AddScoped<ICTransactionTemplateTypeRepository, CTransactionTemplateTypeRepository>();
        services.AddScoped<ICClassDurationRepository, CClassDurationRepository>();
        services.AddScoped<ICTransactionStateRepository, CTransactionStateRepository>();
        services.AddScoped<ICTransactionRefundReasonRepository, CTransactionRefundReasonRepository>();

        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IAppUserReviewRepository, AppUserReviewRepository>();
        services.AddScoped<IAppUserRegistrationCodeRepository, AppUserRegistrationCodeRepository>();
        services.AddScoped<IAppRecurrentTimeslotBlacklistRepository, AppRecurrentTimeslotBlacklistRepository>();
        services.AddScoped<IAppRecurrentTimeslotRepository, AppRecurrentTimeslotRepository>();
        services.AddScoped<IAppTimeslotRepository, AppTimeslotRepository>();
        services.AddScoped<IAppReservationPackageRepository, AppReservationPackageRepository>();
        services.AddScoped<IAppReservationRepository, AppReservationRepository>();
        services.AddScoped<IAppMessageRepository, AppMessageRepository>();
        services.AddScoped<IAppConversationRepository, AppConversationRepository>();
        services.AddScoped<IAppMessageAttachmentRepository, AppMessageAttachmentRepository>();
        services.AddScoped<IAppRoomRepository, AppRoomRepository>();
        services.AddScoped<IAppRoomActivityRepository, AppRoomActivityRepository>();
        services.AddScoped<IAppUserBehaviourRepository, AppUserBehaviourRepository>();
        services.AddScoped<IAppUserReservationReviewRepository, AppUserReservationReviewRepository>();
        services.AddScoped<IAppReservationIssueRepository, AppReservationIssueRepository>();
        services.AddScoped<IAppTransactionRepository, AppTransactionRepository>();
        services.AddScoped<IAppUserWalletRepository, AppUserWalletRepository>();
        services.AddScoped<IAppNotificationRepository, AppNotificationRepository>();
        services.AddScoped<IAppUserProfileBehaviourRepository, AppUserProfileBehaviourRepository>();
        services.AddScoped<IAppCouponRepository, AppCouponRepository>();

        services.AddScoped<IRUserAttributeRepository, RUserAttributeRepository>();
        services.AddScoped<IRUserLanguageRepository, RUserLanguageRepository>();
        services.AddScoped<IRUserTokenRepository, RUserTokenRepository>();
        services.AddScoped<IRUserRoomStatisticRepository, RUserRoomStatisticRepository>();
        services.AddScoped<IRUserSpecializationRepository, RUserSpecializationRepository>();
        services.AddScoped<IRUserFilePermissionRepository, RUserFilePermissionRepository>();
        services.AddScoped<IRUserSubjectLevelRepository, RUserSubjectLevelRepository>();
        services.AddScoped<IRUserInterestRepository, RUserInterestRepository>();
        services.AddScoped<IRUserNotificationRepository, RUserNotificationRepository>();
        services.AddScoped<IRUserNotificationBlacklistSettingRepository, RUserNotificationBlacklistSettingRepository>();
        services.AddScoped<IRNotificationTypeUserRoleRepository, RNotificationTypeUserRoleRepository>();
        services.AddScoped<IRUserActivatedCouponRepository, RUserActivatedCouponRepository>();
    }
}