using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Shared.Services.Interfaces;

public interface INotificationSharedService
{
    Task SendRegistrationCode(int recipientId, string registrationCode, string registrationToken);
    Task SendInvite(int recipientId, string registrationCode, string registrationToken);
    Task SendProfiledVerified(int recipientId);
    Task SendForgottenPassword(int recipientId, string token);
    Task SendWelcomeEmail(int recipientId);
    Task SendPackageCancelled(int reservationPackageId, int senderId, int recipientId);
    Task SendBatchLessonReminders(IReadOnlyCollection<AppReservationForNotificationVM> reservations);
    Task SendBatchNewMessagesGroupedEmail(IReadOnlyCollection<AppMessageForNotificationVM> messages);
    Task SendReservationRescheduled(int newReservationId, int senderId, int recipientId);
    Task SendNewReservation(int reservationPackageId);
    Task SendNewUserReview(int userReviewId);
    Task SendNewReservationIssue(int reservationIssueId);
    Task SendRefundApproved(int reservationIssueId);
    Task SendRefundDeclined(int reservationIssueId);
    Task SendMoneySent(int transactionId);
    Task SendNewTutorWaitingForApproval(int userId);
    Task SendNewWithdrawalRequest(int userId);
    Task SendStudentDepositedMoney(int transactionId);
    Task SendRegistrationCompleted(int userId);
    Task SendAccountCreated(int userId);
    Task SendCompanyInvitedEmployees(int userId, int count);
    Task SendCompanySendMoney(int adminId, double amount, int count);
    Task SendTutorProfileVisited(int tutorId, int visitorId);
    Task SendPersonalCoupon(int couponId);
}