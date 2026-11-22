using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

public enum NotificationTemplateTypeEnum
{
    [ObjectName("new_reservation")] NewReservation,

    [ObjectName("new_package_reservation")]
    NewPackageReservation,

    [ObjectName("reservation_cancellation")]
    ReservationCancellation,
    [ObjectName("package_cancellation")] PackageCancellation,
    [ObjectName("reservation_reschedule")] ReservationReschedule,
    [ObjectName("refund_requested")] RefundRequested,
    [ObjectName("new_review")] NewReview,
    [ObjectName("withdrawal_approved")] WithdrawalApproved,
    [ObjectName("refund_approved")] RefundApproved,
    [ObjectName("refund_declined")] RefundDeclined,
    [ObjectName("reservation_reminder")] ReservationReminder,
    [ObjectName("tutor_profile_visited")] TutorProfileVisited,
}