using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class RNotificationTypeUserRoleSeeder
{
    public static readonly List<RNotificationTypeUserRole> Entities =
        new List<(int id, CNotificationTypeEnum type, CUserRoleEnum role)>()
        {
            (1, CNotificationTypeEnum.PackageCancellation, CUserRoleEnum.Tutor),
            (2, CNotificationTypeEnum.LessonReminder, CUserRoleEnum.Tutor),
            (3, CNotificationTypeEnum.LessonReminder, CUserRoleEnum.Student),
            (4, CNotificationTypeEnum.StudentReschedule, CUserRoleEnum.Tutor),
            (5, CNotificationTypeEnum.NewReservation, CUserRoleEnum.Tutor),
            (6, CNotificationTypeEnum.NewPackageReservation, CUserRoleEnum.Tutor),
            (7, CNotificationTypeEnum.NewReview, CUserRoleEnum.Tutor),
            (8, CNotificationTypeEnum.NewMessage, CUserRoleEnum.Tutor),
            (9, CNotificationTypeEnum.NewMessage, CUserRoleEnum.Student),
            (10, CNotificationTypeEnum.TutorReschedule, CUserRoleEnum.Student),
            (11, CNotificationTypeEnum.ReservationCancellation, CUserRoleEnum.Tutor),
            (12, CNotificationTypeEnum.TutorProfileVisited, CUserRoleEnum.Tutor),
            (13, CNotificationTypeEnum.Coupon, CUserRoleEnum.Student),
        }.Select(x => new RNotificationTypeUserRole
        {
            NotificationTypeRoleId = x.id,
            NotificationTypeId = (int)x.type,
            UserRoleId = (int)x.role
        }).ToList();
}