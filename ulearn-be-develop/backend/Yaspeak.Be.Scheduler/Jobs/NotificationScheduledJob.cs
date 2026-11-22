using Quartz;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace Yaspeak.Be.Scheduler.Jobs;

public class NotificationScheduledJob : IJob
{
    private const int LessonTimePeriodInMinutesToNotify = 15;
    private const int UnreadNewMessageNotificationTimeInMinutes = 1;
    public static readonly JobKey Key = new("app-notification-job");
    private readonly IAppMessageRepository _appMessageRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppUserRepository _appUserRepository;

    private readonly ILogger<NotificationScheduledJob> _logger;
    private readonly INotificationSharedService _notificationSharedService;

    public NotificationScheduledJob(
        IAppReservationRepository appReservationRepository,
        INotificationSharedService notificationSharedService,
        IAppMessageRepository appMessageRepository,
        IAppUserRepository appUserRepository,
        ILogger<NotificationScheduledJob> logger
    )
    {
        _appReservationRepository = appReservationRepository;
        _notificationSharedService = notificationSharedService;
        _appMessageRepository = appMessageRepository;
        _appUserRepository = appUserRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await CreateLessonReminderNotifications();
        await CreateNewMessageNotifications();
        await SendWelcomeEmails();
    }

    private async Task CreateLessonReminderNotifications()
    {
        var reservations = await _appReservationRepository
            .GetLessonsAboutToStartWithoutNotification(LessonTimePeriodInMinutesToNotify);
        await _notificationSharedService.SendBatchLessonReminders(reservations);
        await _appReservationRepository.SetNotified(reservations.Select(x => x.ReservationId));
    }

    private async Task CreateNewMessageNotifications()
    {
        var unreadMessagesWithoutNotification = await _appMessageRepository
            .GetUnreadMessagesWithoutNotification(UnreadNewMessageNotificationTimeInMinutes);
        await _notificationSharedService.SendBatchNewMessagesGroupedEmail(unreadMessagesWithoutNotification);
        await _appMessageRepository.SetNotified(unreadMessagesWithoutNotification.Select(x => x.MessageId));
    }

    private async Task SendWelcomeEmails()
    {
        var userIds = await _appUserRepository.GetWelcomeEmailUserIdsToSend() ?? Array.Empty<int>();
        if (userIds.Any())
        {
            _logger.LogInformation("Sending welcome email to [{Number}] of users", userIds.Count);
        }

        foreach (var userId in userIds)
        {
            await _notificationSharedService.SendWelcomeEmail(userId);
        }
    }
}