using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;

[SwaggerSubType(typeof(NewPkgReservationNotificationVm))]
[SwaggerSubType(typeof(NewReservationNotificationVm))]
[SwaggerSubType(typeof(NewReviewNotificationVm))]
[SwaggerSubType(typeof(PackageCancellationNotificationVm))]
[SwaggerSubType(typeof(RefundApprovedNotificationVM))]
[SwaggerSubType(typeof(RefundDeclinedNotificationVM))]
[SwaggerSubType(typeof(RefundRequestedNotificationVM))]
[SwaggerSubType(typeof(ReservationCancellationNotificationVM))]
[SwaggerSubType(typeof(ReservationReminderNotificationVM))]
[SwaggerSubType(typeof(ReservationRescheduleNotificationVm))]
[SwaggerSubType(typeof(WithdrawalApprovedNotificationVm))]
[SwaggerSubType(typeof(TutorProfileVisitedNotificationVm))]
public abstract class NotificationBaseVM
{
    [Required] public abstract string Type { get; }
}