using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data
{
    public partial class AppUser
    {
        [Column("user_id")] public int UserId { get; set; }
        [Column("login")] [MaxLength(512)] public string Login { get; set; }
        [Column("email")] [MaxLength(512)] public string Email { get; set; } = null!;
        [Column("name")] [MaxLength(256)] public string Name { get; set; }

        [Column("first_name")]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Column("last_name")] [MaxLength(256)] public string LastName { get; set; }
        [Column("created")] public DateTime Created { get; set; }
        [Column("user_state_id")] public int UserStateId { get; set; }
        [Column("phone")] [MaxLength(100)] public string Phone { get; set; }
        [Column("user_role_id")] public int UserRoleId { get; set; }
        [Column("password")] public string Password { get; set; }
        [Column("iana_timezone")] public string IanaTimezone { get; set; }
        [Column("country_id")] public int? CountryId { get; set; }
        [Column("teaching_experience_id")] public int? TeachingExperienceId { get; set; }
        [Column("currency_id")] public int? CurrencyId { get; set; }
        [Column("hour_rate")] public double? HourRate { get; set; }
        [Column("subject_id")] public int? SubjectId { get; set; }
        [Column("excluded")] public bool Excluded { get; set; }
        [Column("translation_language_id")] public int? TranslationLanguageId { get; set; }
        [Column("weekly_goal_id")] public int? WeeklyGoalId { get; set; }
        [Column("ico")] public string Ico { get; set; }
        [Column("place_of_business")] public string PlaceOfBusiness { get; set; }
        [Column("dic")] public string Dic { get; set; }
        [Column("company_name")] public string CompanyName { get; set; }
        [Column("billing_email")] public string BillingEmail { get; set; }
        [Column("inviter_id")] public int? InviterId { get; set; }
        [Column("is_vat_payer")] public bool IsVatPayer { get; set; }
        [Column("hubspot_contact_id")] public string HubspotContactId { get; set; }
        [Column("last_sync_to_hubspot")] public DateTime? LastSyncToHubspot { get; set; }
        [Column("certifications_verified")] public bool CertificationsVerified { get; set; }
        [Column("registration_method_id")] public int? RegistrationMethodId { get; set; }
        [Column("order_number")] public int OrderNumber { get; set; }
        [Column("coupon_last_sent_time")] public DateTime? CouponLastSentTime { get; set; }
        public virtual CTeachingExperience CTeachingExperience { get; set; } = null!;
        public virtual CUserState UserState { get; set; } = null!;
        public virtual CUserRole CUserRole { get; set; } = null!;
        public virtual CCountry CCountry { get; set; } = null!;
        public virtual CCurrency CCurrency { get; set; } = null!;
        public virtual CSubject TaughtSubject { get; set; } = null!;
        public virtual CWeeklyGoal CWeeklyGoal { get; set; } = null!;
        public virtual AppUser Inviter { get; set; } = null!;
        public virtual CAuthenticationMethod RegistrationMethod { get; set; } = null!;
        public virtual CTranslationLanguage CTranslationLanguage { get; set; } = null!;
        public virtual ICollection<AppReservation> AttenderAppReservations { get; set; } = new List<AppReservation>();
        public virtual ICollection<AppReservation> HostAppReservations { get; set; } = new List<AppReservation>();
        public virtual ICollection<AppRoom> AttenderAppRooms { get; set; } = new List<AppRoom>();
        public virtual ICollection<AppRoom> HostAppRooms { get; set; } = new List<AppRoom>();

        public virtual ICollection<AppRecurrentTimeslot> AppRecurrentTimeslots { get; set; } =
            new List<AppRecurrentTimeslot>();

        public virtual ICollection<AppTimeslot> AppTimeslots { get; set; } = new List<AppTimeslot>();
        public virtual ICollection<AppUserReview> AppUserReviewReviewees { get; set; } = new List<AppUserReview>();
        public virtual ICollection<AppUserReview> AppUserReviewReviewers { get; set; } = new List<AppUserReview>();
        public virtual ICollection<AppUserWallet> AppUserWallets { get; set; } = new List<AppUserWallet>();
        public virtual ICollection<RUserAttribute> RUserAttributes { get; set; } = new List<RUserAttribute>();
        public virtual ICollection<RUserFile> RUserFiles { get; set; } = new List<RUserFile>();

        public virtual ICollection<RUserFilePermission> RUserFilePermissions { get; set; } =
            new List<RUserFilePermission>();

        public virtual ICollection<RUserLanguage> RUserLanguages { get; set; } = new List<RUserLanguage>();
        public virtual ICollection<RUserToken> RUserTokens { get; set; } = new List<RUserToken>();

        public virtual ICollection<AppUserRegistrationCode> AppUserRegistrationCodes { get; set; } =
            new List<AppUserRegistrationCode>();

        public virtual ICollection<CTranslationNamespace> CreatorCTranslationNamespaces { get; set; } =
            new List<CTranslationNamespace>();

        public virtual ICollection<CTranslationNamespace> UpdaterCTranslationNamespaces { get; set; } =
            new List<CTranslationNamespace>();

        public virtual ICollection<CTranslationKey> CreatorCTranslationKeys { get; set; } = new List<CTranslationKey>();
        public virtual ICollection<CTranslationKey> UpdaterCTranslationKeys { get; set; } = new List<CTranslationKey>();
        public virtual ICollection<CTranslation> CreatorCTranslations { get; set; } = new List<CTranslation>();
        public virtual ICollection<CTranslation> UpdaterCTranslations { get; set; } = new List<CTranslation>();
        public virtual ICollection<AppMessage> SentAppMessages { get; set; } = new List<AppMessage>();
        public virtual ICollection<AppMessage> ReceivedAppMessages { get; set; } = new List<AppMessage>();

        public virtual ICollection<AppConversation> InitiatorAppConversations { get; set; } =
            new List<AppConversation>();

        public virtual ICollection<AppConversation> OpponentAppConversations { get; set; } =
            new List<AppConversation>();

        public virtual ICollection<AppRoomActivity> AppRoomActivities { get; set; } = new List<AppRoomActivity>();

        public virtual ICollection<RUserRoomStatistic> RUserRoomStatistics { get; set; } =
            new List<RUserRoomStatistic>();

        public virtual ICollection<RUserSpecialization> RUserSpecializations { get; set; } =
            new List<RUserSpecialization>();

        public virtual ICollection<RUserSubjectLevel> RUserSubjectLevels { get; set; } = new List<RUserSubjectLevel>();
        public virtual ICollection<RUserInterest> RUserInterests { get; set; } = new List<RUserInterest>();
        public virtual ICollection<AppUserBehaviour> AppUserBehaviours { get; set; } = new List<AppUserBehaviour>();

        public virtual ICollection<AppReservationReview> AppReservationReviews { get; set; } =
            new List<AppReservationReview>();

        public virtual ICollection<AppReservationPackage> AttenderReservationPackages { get; set; } =
            new List<AppReservationPackage>();

        public virtual ICollection<AppReservationPackage> HostReservationPackages { get; set; } =
            new List<AppReservationPackage>();

        public virtual ICollection<AppReservationIssue> CreatedAppReservationIssues { get; set; } =
            new List<AppReservationIssue>();

        public virtual ICollection<AppReservationIssue> ResolvedAppReservationIssues { get; set; } =
            new List<AppReservationIssue>();

        public virtual ICollection<AppTransaction> UpdatedTransactions { get; set; } = new List<AppTransaction>();
        public virtual ICollection<AppTransaction> CreatedTransactions { get; set; } = new List<AppTransaction>();
        public virtual ICollection<AppTransaction> ApprovedTransactions { get; set; } = new List<AppTransaction>();
        public virtual ICollection<AppTransaction> VendorTransactions { get; set; } = new List<AppTransaction>();
        public virtual ICollection<AppTransaction> CustomerTransactions { get; set; } = new List<AppTransaction>();
        public virtual ICollection<AppNotification> OwnedAppNotifications { get; set; } = new List<AppNotification>();
        public virtual ICollection<AppNotification> SentAppNotifications { get; set; } = new List<AppNotification>();
        public virtual ICollection<AppUser> InvitedUsers { get; set; } = new List<AppUser>();
        public virtual ICollection<RUserNotification> RUserNotifications { get; set; } = new List<RUserNotification>();

        public virtual ICollection<AppUserProfileBehaviour> UserAppUserProfileBehaviours { get; set; } =
            new List<AppUserProfileBehaviour>();

        public virtual ICollection<AppUserProfileBehaviour> VisitedUserAppUserProfileBehaviours { get; set; } =
            new List<AppUserProfileBehaviour>();

        public virtual ICollection<RUserNotificationBlacklistSetting> RUserNotificationBlacklistSettings { get; set; } =
            new List<RUserNotificationBlacklistSetting>();

        public virtual ICollection<AppCoupon> AppCoupons { get; set; } = new List<AppCoupon>();

        public virtual ICollection<RUserActivatedCoupon> RUserActivatedCoupons { get; set; } =
            new List<RUserActivatedCoupon>();
    }
}

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> entity)
    {
        entity.HasKey(e => e.UserId).HasName("pk_app_user");

        entity.ToTable("app_user");

        entity.HasOne(e => e.CTeachingExperience)
            .WithMany(e => e.AppUsers)
            .HasForeignKey(e => e.TeachingExperienceId)
            .HasConstraintName("fk_app_user_c_teaching_experience_teaching_experience_id");

        entity.HasOne(d => d.UserState)
            .WithMany(p => p.AppUsers)
            .HasForeignKey(d => d.UserStateId)
            .HasConstraintName("fk_app_user_c_user_state_user_state_id");

        entity.HasOne(e => e.CUserRole)
            .WithMany(e => e.AppUsers)
            .HasForeignKey(e => e.UserRoleId)
            .HasConstraintName("fk_app_user_c_user_role_user_role_id");

        entity.HasOne(e => e.CCountry)
            .WithMany(e => e.AppUsers)
            .HasForeignKey(e => e.CountryId)
            .HasConstraintName("fk_app_user_c_country_country_id");

        entity.HasOne(e => e.TaughtSubject)
            .WithMany(e => e.AppUsers)
            .HasForeignKey(e => e.SubjectId)
            .HasConstraintName("fk_app_user_c_subject_subject_id");

        entity.HasOne(e => e.CCurrency)
            .WithMany(e => e.AppUsers)
            .HasForeignKey(e => e.CurrencyId)
            .HasConstraintName("fk_app_user_c_currency_currency_id");

        entity.HasOne(e => e.CTranslationLanguage)
            .WithMany(e => e.AppUsers)
            .HasForeignKey(e => e.TranslationLanguageId)
            .HasConstraintName("fk_app_user_c_translation_language_translation_language_id");

        entity.HasOne(e => e.CWeeklyGoal)
            .WithMany(e => e.AppUsers)
            .HasForeignKey(e => e.WeeklyGoalId)
            .HasConstraintName("fk_app_user_c_weekly_goal_weekly_goal_id");

        entity.HasOne(e => e.Inviter)
            .WithMany(e => e.InvitedUsers)
            .HasForeignKey(e => e.InviterId)
            .HasConstraintName("fk_app_user_app_user_inviter_id");

        entity.HasOne(e => e.RegistrationMethod)
            .WithMany(e => e.RegistrationMethodAppUsers)
            .HasForeignKey(e => e.RegistrationMethodId)
            .HasConstraintName("fk_app_user_c_authentication_method_registration_method_id");

        entity.HasIndex(e => e.TeachingExperienceId, "ix_app_user_teaching_experience_id");
        entity.HasIndex(e => e.UserStateId, "ix_app_user_user_state_id");
        entity.HasIndex(e => e.UserRoleId, "ix_app_user_user_role_id");
        entity.HasIndex(e => e.CountryId, "ix_app_user_country_id");
        entity.HasIndex(e => e.CountryId, "ix_app_user_taught_subject_id");
        entity.HasIndex(e => e.CurrencyId, "ix_app_user_currency_id");
        entity.HasIndex(e => e.TranslationLanguageId, "ix_app_user_translation_language_id");
        entity.HasIndex(e => e.WeeklyGoalId, "ix_app_user_weekly_goal_id");
        entity.HasIndex(e => e.InviterId, "ix_app_user_inviter_id");
    }
}