using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data.Context
{
    public partial class RepositoryContext : DbContext
    {
        public RepositoryContext()
        {
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        // Codetables
        public virtual DbSet<CCategory> CCategories { get; set; } = null!;
        public virtual DbSet<CCountry> CCountries { get; set; } = null!;
        public virtual DbSet<CCurrency> CCurrencies { get; set; } = null!;
        public virtual DbSet<CFileType> CFileTypes { get; set; } = null!;
        public virtual DbSet<CLanguage> CLanguages { get; set; } = null!;
        public virtual DbSet<CLanguageLevel> CLanguageLevels { get; set; } = null!;
        public virtual DbSet<CMasterHash> CMasterHashes { get; set; } = null!;
        public virtual DbSet<CPaymentServiceProvider> CPaymentServiceProviders { get; set; } = null!;
        public virtual DbSet<CReservationState> CReservationStates { get; set; } = null!;
        public virtual DbSet<CTeachingExperience> CTeachingExperiences { get; set; } = null!;
        public virtual DbSet<CSubject> CSubjects { get; set; } = null!;
        public virtual DbSet<CSubjectLevel> CSubjectLevels { get; set; } = null!;
        public virtual DbSet<CTokenProvider> CTokenProviders { get; set; } = null!;
        public virtual DbSet<CTokenType> CTokenTypes { get; set; } = null!;
        public virtual DbSet<CTransactionState> CTransactionStates { get; set; } = null!;
        public virtual DbSet<CTransactionType> CTransactionTypes { get; set; } = null!;
        public virtual DbSet<CTransactionTemplateType> CTransactionTemplateTypes { get; set; } = null!;
        public virtual DbSet<CTransactionTemplate> CTransactionTemplates { get; set; } = null!;
        public virtual DbSet<CUserAttributeType> CUserAttributeTypes { get; set; } = null!;
        public virtual DbSet<CUserRole> CUserRoles { set; get; } = null!;
        public virtual DbSet<CUserState> CUserStates { get; set; } = null!;
        public virtual DbSet<CTranslation> CTranslations { get; set; } = null!;
        public virtual DbSet<CTranslationKey> CTranslationKeys { get; set; } = null!;
        public virtual DbSet<CTranslationLanguage> CTranslationLanguages { get; set; } = null!;
        public virtual DbSet<CTranslationNamespace> CTranslationNamespaces { get; set; } = null!;
        public virtual DbSet<CMessageState> CMessageStates { get; set; } = null!;
        public virtual DbSet<CRoomState> CRoomStates { get; set; } = null!;
        public virtual DbSet<CRoomActivityType> CRoomActivityTypes { get; set; } = null!;
        public virtual DbSet<CRoomStatisticType> CRoomStatisticTypes { get; set; } = null!;
        public virtual DbSet<CSpecialization> CSpecializations { get; set; } = null!;
        public virtual DbSet<CWeeklyGoal> CWeeklyGoals { get; set; } = null!;
        public virtual DbSet<CInterest> CInterests { get; set; } = null!;
        public virtual DbSet<CReservationIssueType> CReservationIssueTypes { get; set; } = null!;
        public virtual DbSet<CNotificationType> CNotificationTypes { get; set; } = null!;
        public virtual DbSet<CNotificationState> CNotificationStates { get; set; } = null!;
        public virtual DbSet<CClassDuration> CClassDurations { get; set; } = null!;
        public virtual DbSet<CTransactionRefundReason> CTransactionRefundReasons { get; set; } = null!;
        public virtual DbSet<CAuthenticationMethod> CAuthenticationMethods { get; set; } = null!;
        public virtual DbSet<CCouponType> CCouponTypes { get; set; } = null!;
        public virtual DbSet<CSpecializationCategory> CSpecializationCategories { get; set; } = null!;

        // APP        
        public virtual DbSet<AppReservationPackage> AppReservationsPackages { get; set; } = null!;
        public virtual DbSet<AppReservationIssue> AppReservationIssues { get; set; } = null!;
        public virtual DbSet<AppReservation> AppReservations { get; set; } = null!;
        public virtual DbSet<AppRecurrentTimeslot> AppRecurrentTimeslots { get; set; } = null!;
        public virtual DbSet<AppRecurrentTimeslotBlacklist> AppRecurrentTimeslotBlacklists { get; set; } = null!;
        public virtual DbSet<AppTimeslot> AppTimeslots { get; set; } = null!;
        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<AppConversation> AppConversations { get; set; } = null!;
        public virtual DbSet<AppMessage> AppMessages { get; set; } = null!;
        public virtual DbSet<AppMessageAttachment> AppMessageAttachments { get; set; } = null!;
        public virtual DbSet<AppUserReview> AppUserReviews { get; set; } = null!;
        public virtual DbSet<AppUserWallet> AppUserWallets { get; set; } = null!;
        public virtual DbSet<AppTransaction> AppUserWalletTransactions { get; set; } = null!;
        public virtual DbSet<AppRoom> AppRooms { get; set; } = null!;
        public virtual DbSet<AppRoomActivity> AppRoomActivities { get; set; } = null!;
        public virtual DbSet<AppUserRegistrationCode> AppUserRegistrationCodes { get; set; } = null!;
        public virtual DbSet<AppReservationReview> AppReservationReviews { get; set; } = null!;
        public virtual DbSet<AppUserBehaviour> AppUserBehaviourTransactions { get; set; } = null!;
        public virtual DbSet<AppUserProfileBehaviour> AppUserProfileBehaviours { get; set; } = null!;
        public virtual DbSet<AppTransaction> AppTransactions { get; set; } = null!;
        public virtual DbSet<AppTransactionGroup> AppTransactionGroups { get; set; } = null!;
        public virtual DbSet<AppNotification> AppNotifications { get; set; } = null!;
        public virtual DbSet<AppCoupon> AppCoupons { get; set; } = null!;
        public virtual DbSet<EnvConfiguration> EnvConfigurations { get; set; } = null!;
        public virtual DbSet<EnvConfigurationDataType> EnvConfigurationDataTypes { get; set; } = null!;

        // Relation
        public DbSet<RUserAttribute> RUserAttributes { get; set; } = null!;
        public virtual DbSet<RUserFile> RUserFiles { get; set; } = null!;
        public virtual DbSet<RUserInterest> RUserInterests { get; set; } = null!;
        public virtual DbSet<RUserLanguage> RUserLanguages { get; set; } = null!;
        public virtual DbSet<RUserToken> RUserTokens { get; set; } = null!;
        public virtual DbSet<RUserRoomStatistic> RUserRoomStatistics { get; set; } = null!;
        public virtual DbSet<RUserSpecialization> RUserSpecializations { get; set; } = null!;
        public virtual DbSet<RUserFilePermission> RUserFilePermissions { get; set; } = null!;
        public virtual DbSet<RUserSubjectLevel> RUserSubjectLevels { get; set; } = null!;
        public virtual DbSet<RUserNotification> RUserNotifications { get; set; } = null!;

        public virtual DbSet<RUserNotificationBlacklistSetting> RUserNotificationBlacklistSettings { get; set; } =
            null!;

        public virtual DbSet<RNotificationTypeUserRole> RNotificationTypeUserRole { get; set; } = null!;
        public virtual DbSet<RUserActivatedCoupon> RUserActivatedCoupons { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationUtil.LoadConnectionString("RepositoryDb");
                optionsBuilder
                    .UseNpgsql(connectionString, builder =>
                    {
                        builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
            }

            optionsBuilder.EnableSensitiveDataLogging(EnvConfigUtils.IsDevelopment);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);
            modelBuilder.ApplyRemoveOneToManyCascade();
        }
    }
}