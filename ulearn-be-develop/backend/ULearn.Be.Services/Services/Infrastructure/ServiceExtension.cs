using Microsoft.Extensions.DependencyInjection;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Services.Other;

namespace ULearn.Be.Services.Services.Infrastructure;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<IAppNotificationService, AppNotificationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<ICodetableService, CodetableService>();
        services.AddScoped<IRUserFileService, RUserFileService>();
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IAppUserStatisticsService, AppUserStatisticsService>();
        services.AddScoped<IAppMessageService, AppMessageService>();
        services.AddScoped<IAppConversationService, AppConversationService>();
        services.AddScoped<IAppReservationPackageService, AppReservationPackageService>();
        services.AddScoped<IAppReservationService, AppReservationService>();
        services.AddScoped<IAppRoomService, AppRoomService>();
        services.AddScoped<IAppRoomActivityService, AppRoomActivityService>();
        services.AddScoped<IAppCouponService, AppCouponService>();
        services.AddScoped<IUserRoomStatisticService, UserRoomStatisticService>();
        services.AddScoped<IAppRecurrentTimeslotService, AppRecurrentTimeslotService>();
        services.AddScoped<IAppRecurrentTimeslotBlacklistService, AppRecurrentTimeslotBlacklistService>();
        services.AddScoped<ICalendarService, CalendarService>();
        services.AddScoped<IAppTimeslotService, AppTimeslotService>();
        services.AddScoped<IMasterHashService, MasterHashService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAppUserRecommendationService, AppUserRecommendationService>();
        services.AddScoped<IAppReservationReviewService, AppReservationReviewService>();
        services.AddScoped<IAppUserReviewService, AppUserReviewService>();
        services.AddScoped<IAppReservationIssueService, AppReservationIssueService>();
        services.AddScoped<IAppUserWalletService, AppUserWalletService>();
        services.AddScoped<IAppTransactionService, AppTransactionService>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        services.AddScoped<ICTransactionTemplateService, CTransactionTemplateService>();
        services.AddScoped<IUserBehaviourService, UserBehaviourService>();
        services.AddScoped<ITransactionExportService, TransactionExportService>();

        services.AddSingleton<ITokenService, TokenService>();
    }
}