using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class ReservationRescheduleNotificationVm : ReservationBaseNotificationVm
{
    // ReSharper disable once UnusedMember.Global
    public ReservationRescheduleNotificationVm()
    {
    }

    public ReservationRescheduleNotificationVm(AppReservationForNotificationVM reservation) : base(reservation)
    {
    }

    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.ReservationReschedule);
}