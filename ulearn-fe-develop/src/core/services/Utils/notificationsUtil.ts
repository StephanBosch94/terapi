import { components, paths } from "apiTypes";

const notificationTemplateType = {
    newReservation: 'new_reservation',
    newPackageReservation: 'new_package_reservation',
    reservationCancellation: 'reservation_cancellation',
    packageCancellation: 'package_cancellation',
    reservationReschedule: 'reservation_reschedule',
    refundRequested: 'refund_requested',
    newReview: 'new_review',
    withdrawalApproved: 'withdrawal_approved',
    refundApproved: 'refund_approved',
    refundDeclined: 'refund_declined',
    reservationReminder: 'reservation_reminder',
    tutorProfileVisited: 'tutor_profile_visited'
};

type BaseType = paths["/api/notification/filter"]['post']['responses']['200']['content']['application/json']['data'][0]['data'];
export type NewReservationNotificationType = components['schemas']['NewReservationNotificationVm'];
export type NewPkgReservationNotificationType = components['schemas']['NewPkgReservationNotificationVm'];
export type ReservationCancellationNotificationType = components['schemas']['ReservationCancellationNotificationVM'];
export type PackageCancellationNotificationType = components['schemas']['PackageCancellationNotificationVm'];
export type ReservationRescheduleNotificationType = components['schemas']['ReservationRescheduleNotificationVm'];
export type RefundRequestedNotificationType = components['schemas']['RefundRequestedNotificationVM'];
export type NewReviewNotificationType = components['schemas']['NewReviewNotificationVm'];
export type WithdrawalApprovedNotificationType = components['schemas']['WithdrawalApprovedNotificationVm'];
export type RefundApprovedNotificationType = components['schemas']['RefundApprovedNotificationVM'];
export type RefundDeclinedNotificationType = components['schemas']['RefundDeclinedNotificationVM'];
export type ReservationReminderNotificationType = components['schemas']['ReservationReminderNotificationVM'];
export type TutorProfileVisitedNotificationType = components['schemas']['TutorProfileVisitedNotificationVm'];

export const isNotificationOfType = (templateData: BaseType, type: string): boolean => {
    return templateData?.type === type;
};

export const isNewReservationType = (templateData: BaseType): templateData is NewReservationNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.newReservation);
};

export const isNewPackageReservationType = (templateData: BaseType): templateData is NewPkgReservationNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.newPackageReservation);
};

export const isReservationCancellationType = (templateData: BaseType): templateData is ReservationCancellationNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.reservationCancellation);
};

export const isPackageCancellationType = (templateData: BaseType): templateData is PackageCancellationNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.packageCancellation);
};

export const isReservationRescheduleType = (templateData: BaseType): templateData is ReservationRescheduleNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.reservationReschedule);
};

export const isRefundRequestedType = (templateData: BaseType): templateData is RefundRequestedNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.refundRequested);
};

export const isNewReviewType = (templateData: BaseType): templateData is NewReviewNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.newReview);
};

export const isWithdrawalApprovedType = (templateData: BaseType): templateData is WithdrawalApprovedNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.withdrawalApproved);
};

export const isRefundApprovedType = (templateData: BaseType): templateData is RefundApprovedNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.refundApproved);
};

export const isRefundDeclinedType = (templateData: BaseType): templateData is RefundDeclinedNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.refundDeclined);
};

export const isReservationReminderType = (templateData: BaseType): templateData is ReservationReminderNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.reservationReminder);
};

export const isTutorProfileVisitedType = (templateData: BaseType): templateData is TutorProfileVisitedNotificationType => {
    return isNotificationOfType(templateData, notificationTemplateType.tutorProfileVisited);
};