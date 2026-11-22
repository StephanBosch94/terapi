using ULearn.Be.Common.Utils;
using ULearn.Be.Models.ViewModels.AppNotification.Templates.Base;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.ViewModels.AppNotification.Templates;

public class ReservationCancellationNotificationVM : ReservationBaseNotificationVm
{
    public ReservationCancellationNotificationVM(AppReservationForNotificationVM entity) : base(entity)
    {
    }

    public ReservationCancellationNotificationVM()
    {
    }

    public override string Type => EnumUtils.GetObjectName(NotificationTemplateTypeEnum.ReservationCancellation);
}