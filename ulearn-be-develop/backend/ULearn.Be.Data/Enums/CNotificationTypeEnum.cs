using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CNotificationTypeEnum
{
    [EnumKey("email_confirmation")] EmailConfirmation = 1,

    [EnumKey("profile_verified")] ProfileVerified = 2,

    [EnumKey("password_reset")] PasswordReset = 3,

    [EnumKey("welcome")] Welcome = 4,

    [EnumKey("package_cancellation")] PackageCancellation = 5,

    [EnumKey("lesson_reminder")] LessonReminder = 6,

    [EnumKey("student_rescheduled_lesson")]
    StudentReschedule = 7,

    [EnumKey("new_reservation")] NewReservation = 8,

    [EnumKey("new_package_reservation")] NewPackageReservation = 9,

    [EnumKey("new_review")] NewReview = 10,

    [EnumKey("new_message")] NewMessage = 11,

    [EnumKey("refund_requested_for_your_lesson")]
    RefundRequestedForYourLesson = 12,

    [EnumKey("request_refund_approved")] RequestRefundApproved = 13,

    [EnumKey("request_refund_declined")] RequestRefundDeclined = 14,

    [EnumKey("money_sent")] MoneySent = 15,

    [EnumKey("tutor_rescheduled_lesson")] TutorReschedule = 16,

    [EnumKey("tutor_profile_approval_requested")]
    TutorProfileApprovalRequest = 17,

    [EnumKey("refund_request_for_admin")] RefundRequestForAdmin = 18,

    [EnumKey("money_withdrawal_requested")]
    MoneyWithdrawalRequested = 19,

    [EnumKey("student_bought_package")] StudentBoughtPackage = 20,

    [EnumKey("student_deposited_money")] StudentDepositedMoney = 21,

    [EnumKey("reservation_cancellation")] ReservationCancellation = 22,

    [EnumKey("invitation")] Invitation = 23,

    [EnumKey("registration_completed")] RegistrationCompleted = 24,

    [EnumKey("account_created")] AccountCreated = 25,

    [EnumKey("company_invited_colleagues")]
    CompanyInvitedColleagues = 26,

    [EnumKey("email_confirmation_2")] EmailConfirmation2 = 27,

    [EnumKey("tutor_profile_visited")] TutorProfileVisited = 28,

    [EnumKey("coupon")] Coupon = 29
}