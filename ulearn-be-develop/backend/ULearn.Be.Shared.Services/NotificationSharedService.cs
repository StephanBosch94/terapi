using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Enums;
using ULearn.Be.Data.Utils;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.AppNotification.Templates;
using ULearn.Be.Models.ViewModels.Email;
using ULearn.Be.Models.ViewModels.Reservation;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.Notifications.Email.Interfaces;
using ULearn.Be.Providers.Notifications.Slack.Enums;
using ULearn.Be.Providers.Notifications.Slack.Inrefaces;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;
using ULearn.Be.Websocket.Services.HubServices.Interfaces;

namespace ULearn.Be.Shared.Services;

public class NotificationSharedService : INotificationSharedService
{
    private readonly IAppConversationRepository _appConversationRepository;
    private readonly IAppCouponRepository _appCouponRepository;
    private readonly IAppNotificationRepository _appNotificationRepository;
    private readonly IAppReservationIssueRepository _appReservationIssueRepository;
    private readonly IAppReservationPackageRepository _appReservationPackageRepository;
    private readonly IAppReservationRepository _appReservationRepository;
    private readonly IAppTransactionRepository _appTransactionRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAppUserReviewRepository _appUserReviewRepository;
    private readonly IChatHubService _chatHubService;
    private readonly ICTranslationRepository _cTranslationRepository;
    private readonly IEmailProvider _emailProvider;
    private readonly FeSettings _feSettings;
    private readonly InstanceSettings _instanceSettings;
    private readonly RegistrationSettings _registrationSettings;
    private readonly IRUserNotificationBlacklistSettingRepository _rUserNotificationBlacklistSettingRepository;
    private readonly IRUserNotificationRepository _rUserNotificationRepository;
    private readonly ISlackProvider _slackProvider;
    private readonly ITimeUtility _timeUtility;

    public NotificationSharedService(
        IEmailProvider emailProvider,
        ISlackProvider slackProvider,
        IEnvConfigurationProvider envConfigurationProvider,
        IAppNotificationRepository appNotificationRepository,
        IAppUserRepository appUserRepository,
        ITimeUtility timeUtility,
        IChatHubService chatHubService,
        IAppConversationRepository appConversationRepository,
        IAppReservationRepository appReservationRepository,
        IAppUserReviewRepository appUserReviewRepository,
        IAppReservationIssueRepository appReservationIssueRepository,
        ICTranslationRepository cTranslationRepository,
        IAppTransactionRepository appTransactionRepository,
        IAppReservationPackageRepository appReservationPackageRepository,
        IRUserNotificationRepository rUserNotificationRepository,
        IRUserNotificationBlacklistSettingRepository rUserNotificationBlacklistSettingRepository,
        IAppCouponRepository appCouponRepository
    )
    {
        _emailProvider = emailProvider;
        _appNotificationRepository = appNotificationRepository;
        _appUserRepository = appUserRepository;
        _timeUtility = timeUtility;
        _chatHubService = chatHubService;
        _appConversationRepository = appConversationRepository;
        _appReservationRepository = appReservationRepository;
        _appUserReviewRepository = appUserReviewRepository;
        _appReservationIssueRepository = appReservationIssueRepository;
        _cTranslationRepository = cTranslationRepository;
        _appTransactionRepository = appTransactionRepository;
        _appReservationPackageRepository = appReservationPackageRepository;
        _rUserNotificationRepository = rUserNotificationRepository;
        _rUserNotificationBlacklistSettingRepository = rUserNotificationBlacklistSettingRepository;
        _appCouponRepository = appCouponRepository;
        _slackProvider = slackProvider;
        _registrationSettings =
            envConfigurationProvider.GetSettings<RegistrationSettings>(EnvSettingsKeyEnum.RegistrationSettings);
        _feSettings = envConfigurationProvider.GetSettings<FeSettings>(EnvSettingsKeyEnum.FeSettings);
        _instanceSettings = envConfigurationProvider.GetSettings<InstanceSettings>(EnvSettingsKeyEnum.InstanceSettings);
    }

    public async Task SendRegistrationCode(int recipientId, string registrationCode, string registrationToken)
    {
        var recipient = await _appUserRepository.GetNotificationInfo(recipientId);

        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = CNotificationTypeEnum.EmailConfirmation2,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    {
                        CEmailDataEnum.ActionUrl.GetDescription(),
                        GetRegisterUrlWithCodeParameter(registrationCode, registrationToken)
                    },
                    {
                        CEmailDataEnum.Code.GetDescription(),
                        registrationCode
                    }
                }
            }, throwIfNotSuccess: true);
    }

    public async Task SendInvite(int recipientId, string registrationCode, string registrationToken)
    {
        var recipient = await _appUserRepository.GetById(recipientId, onlyConfirmed: false);
        var admin = await _appUserRepository.GetById(recipient.InviterId!.Value, onlyConfirmed: false);
        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = CNotificationTypeEnum.Invitation,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    {
                        CEmailDataEnum.AdminEmail.GetDescription(),
                        admin.GetUserName()
                    },
                    {
                        CEmailDataEnum.ActionUrl.GetDescription(),
                        GetRegisterUrlWithCodeParameter(registrationCode, registrationToken)
                    }
                }
            }, throwIfNotSuccess: true);
    }

    public async Task SendProfiledVerified(int recipientId)
    {
        var recipient = await _appUserRepository.GetNotificationInfo(recipientId);
        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = CNotificationTypeEnum.ProfileVerified,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    {
                        CEmailDataEnum.ActionUrl.GetDescription(),
                        _feSettings.FeUrl
                    }
                }
            }, throwIfNotSuccess: true);
    }

    public async Task SendForgottenPassword(int recipientId, string token)
    {
        var recipient = await _appUserRepository.GetNotificationInfo(recipientId);
        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = CNotificationTypeEnum.PasswordReset,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.ActionUrl.GetDescription(), GetResetPasswordUrl(token) }
                }
            }, throwIfNotSuccess: true);
    }

    public async Task SendWelcomeEmail(int recipientId)
    {
        var recipient = await _appUserRepository.GetNotificationInfo(recipientId);
        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = CNotificationTypeEnum.Welcome,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.ActionUrl.GetDescription(), GetMatchingTutorsUrl() }
                }
            });
    }

    public async Task SendPackageCancelled(int reservationPackageId, int senderId, int recipientId)
    {
        var reservations = await _appReservationRepository.GetNotificationVmsByPackageId(reservationPackageId);
        var canceledReservations =
            reservations.Where(x => x.ReservationStateId == (int)CReservationStateEnum.CanceledByUser).ToList();
        var notificationTypeEnum = reservations.Count == 1
            ? CNotificationTypeEnum.ReservationCancellation
            : CNotificationTypeEnum.PackageCancellation;

        await CreateAndSendAppNotifications(new AppNotificationIM
        {
            RecipientId = recipientId,
            SenderId = senderId,
            NotificationTypeEnum = notificationTypeEnum,
            Data = reservations.Count == 1
                ? new ReservationCancellationNotificationVM(reservations.First())
                : new PackageCancellationNotificationVm
                {
                    NumberOfLessons = canceledReservations.Count
                }
        });

        var sender = await _appUserRepository.GetById(senderId);
        var recipient = reservations.First().Host;

        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), sender.GetUserName() },
                    {
                        CEmailDataEnum.UserPhotoUrl.GetDescription(),
                        sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                    },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservations.First().StartTime, recipient.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservations.First().ExpectedEndTime, recipient.Timezone)
                    },
                    { CEmailDataEnum.NumberOfLessons.GetDescription(), reservations.Count },
                    { CEmailDataEnum.ActionUrl.GetDescription(), _feSettings.FeUrl },
                }
            });
    }

    public async Task SendBatchLessonReminders(IReadOnlyCollection<AppReservationForNotificationVM> reservations)
    {
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.LessonReminder;
        var notifications = reservations.Select(x =>
            new
            {
                Reservation = x,
                Sender = x.Attender,
                Recipient = x.Host,
            }
        ).Concat(reservations.Select(x =>
            new
            {
                Reservation = x,
                Sender = x.Host,
                Recipient = x.Attender,
            })
        ).ToList();

        await CreateAndSendAppNotifications(
            notifications.Select(x =>
                new AppNotificationIM
                {
                    RecipientId = x.Recipient.UserId,
                    SenderId = x.Sender.UserId,
                    NotificationTypeEnum = notificationTypeEnum,
                    Data = new ReservationReminderNotificationVM(x.Reservation),
                }).ToList()
        );

        foreach (var notification in notifications)
        {
            await SendEmail(
                notification.Recipient.UserId,
                new EmailDataInputVM
                {
                    EmailTemplateEnum = notificationTypeEnum,
                    To = notification.Recipient.Email,
                    LangCode = TranslationUtil.GetLanguageCode(notification.Recipient.TranslationLanguageId),
                    Params = new Dictionary<string, object>
                    {
                        { CEmailDataEnum.Username.GetDescription(), notification.Sender.GetUserName() },
                        {
                            CEmailDataEnum.UserPhotoUrl.GetDescription(),
                            notification.Sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                        },
                        {
                            CEmailDataEnum.From.GetDescription(),
                            GetFormattedFromDate(notification.Reservation.StartTime, notification.Recipient.Timezone)
                        },
                        {
                            CEmailDataEnum.To.GetDescription(),
                            GetFormattedToDate(notification.Reservation.ExpectedEndTime,
                                notification.Recipient.Timezone)
                        },
                        {
                            CEmailDataEnum.NumberOfMinutes.GetDescription(),
                            GetNumberOfMinutesUntilStart(notification.Reservation.StartTime)
                        },
                        { CEmailDataEnum.ActionUrl.GetDescription(), GetRoomUrl(notification.Reservation.RoomKey) },
                    }
                });
        }
    }

    public async Task SendBatchNewMessagesGroupedEmail(IReadOnlyCollection<AppMessageForNotificationVM> messages)
    {
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.NewMessage;

        var numberOfUnreadMessagesByConversationId =
            await _appConversationRepository.GetNumberOfUnreadMessagesByConversationId(
                messages.Select(x => x.ConversationId).ToHashSet()
            );

        var emailNotifications = messages
            .GroupBy(x => x.ConversationId)
            .Select(x => new
            {
                ConversationId = x.Key,
                CountOfUnreadMessages = numberOfUnreadMessagesByConversationId.ContainsKey(x.Key)
                    ? numberOfUnreadMessagesByConversationId[x.Key]
                    : 0,
                LatestMessage = x.MaxBy(y => y.MessageId)
            })
            .Select(x => new
            {
                x.ConversationId,
                x.CountOfUnreadMessages,
                x.LatestMessage!.Recipient,
                x.LatestMessage.Sender
            })
            .ToList();

        foreach (var notification in emailNotifications)
        {
            await SendEmail(
                notification.Recipient.UserId,
                new EmailDataInputVM
                {
                    EmailTemplateEnum = notificationTypeEnum,
                    To = notification.Recipient.Email,
                    LangCode = TranslationUtil.GetLanguageCode(notification.Recipient.TranslationLanguageId),
                    Params = new Dictionary<string, object>
                    {
                        { CEmailDataEnum.Username.GetDescription(), notification.Sender.GetUserName() },
                        {
                            CEmailDataEnum.UserPhotoUrl.GetDescription(),
                            notification.Sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                        },
                        { CEmailDataEnum.NumberOfUnreadMessages.GetDescription(), notification.CountOfUnreadMessages },
                        { CEmailDataEnum.ActionUrl.GetDescription(), GetConversationUrl(notification.ConversationId) },
                    }
                });
        }
    }

    public async Task SendReservationRescheduled(int newReservationId, int senderId, int recipientId)
    {
        var reservation = await _appReservationRepository.GetNotificationVmById(newReservationId);
        var notificationTypeEnum = reservation.Attender.UserId == senderId
            ? CNotificationTypeEnum.StudentReschedule
            : CNotificationTypeEnum.TutorReschedule;
        var sender = reservation.Attender.UserId == senderId ? reservation.Attender : reservation.Host;
        var recipient = reservation.Attender.UserId == recipientId ? reservation.Attender : reservation.Host;

        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), sender.GetUserName() },
                    {
                        CEmailDataEnum.UserPhotoUrl.GetDescription(),
                        sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                    },
                    {
                        CEmailDataEnum.PreviousFrom.GetDescription(),
                        GetFormattedFromDate(reservation.SourceStartTime!.Value, recipient.Timezone)
                    },
                    {
                        CEmailDataEnum.PreviousTo.GetDescription(),
                        GetFormattedFromDate(reservation.SourceExpectedEndTime!.Value, recipient.Timezone)
                    },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservation.StartTime, recipient.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservation.ExpectedEndTime, recipient.Timezone)
                    },
                    { CEmailDataEnum.ActionUrl.GetDescription(), _feSettings.FeUrl },
                }
            });

        await CreateAndSendAppNotifications(
            new AppNotificationIM
            {
                RecipientId = recipientId,
                SenderId = senderId,
                NotificationTypeEnum = notificationTypeEnum,
                Data = new ReservationRescheduleNotificationVm(reservation)
            }
        );
    }

    public async Task SendNewReservation(int reservationPackageId)
    {
        var totalPrice = await _appReservationPackageRepository.GetTotalPrice(reservationPackageId);
        var reservations = await _appReservationRepository.GetNotificationVmsByPackageId(reservationPackageId);
        var recipient = reservations.First().Host;
        var sender = reservations.First().Attender;
        var notificationType = reservations.Count == 1
            ? CNotificationTypeEnum.NewReservation
            : CNotificationTypeEnum.NewPackageReservation;
        var reservation = reservations.First();
        await CreateAndSendAppNotifications(new AppNotificationIM
        {
            RecipientId = reservation.Host.UserId,
            SenderId = reservation.Attender.UserId,
            NotificationTypeEnum = notificationType,
            Data = reservations.Count == 1
                ? new NewReservationNotificationVm(reservation)
                : new NewPkgReservationNotificationVm
                {
                    NumberOfLessons = reservations.Count
                }
        });
        await SendEmail(
            recipient.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationType,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), sender.GetUserName() },
                    {
                        CEmailDataEnum.UserPhotoUrl.GetDescription(),
                        sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                    },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservations.First().StartTime, recipient.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservations.First().ExpectedEndTime, recipient.Timezone)
                    },
                    { CEmailDataEnum.NumberOfLessons.GetDescription(), reservations.Count },
                    { CEmailDataEnum.ActionUrl.GetDescription(), _feSettings.FeUrl },
                }
            });
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                UserName = sender.GetUserName(),
                Email = sender.Email,
                RoleId = sender.UserRoleId,
                NotificationTypeEnum = CNotificationTypeEnum.StudentBoughtPackage,
                Text = $"bought {reservations.Count} lesson(s) with {recipient.GetUserName()} for {totalPrice} Kč"
            });
    }

    public async Task SendNewUserReview(int userReviewId)
    {
        var userReview = await _appUserReviewRepository.GetNotificationVmById(userReviewId);
        var sender = userReview.Reviewer;
        var recipient = userReview.Reviewee;
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.NewReview;
        await CreateAndSendAppNotifications(
            new AppNotificationIM
            {
                RecipientId = recipient.UserId,
                SenderId = sender.UserId,
                NotificationTypeEnum = notificationTypeEnum,
                Data = new NewReviewNotificationVm
                {
                    Comment = userReview.Comment,
                    Rate = userReview.Rate
                }
            });
        await SendEmail(
            recipient.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), sender.GetUserName() },
                    {
                        CEmailDataEnum.UserPhotoUrl.GetDescription(),
                        sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                    },
                    { CEmailDataEnum.ActionUrl.GetDescription(), GetReviewPageUrl() },
                }
            });
    }

    public async Task SendNewReservationIssue(int reservationIssueId)
    {
        var reservationIssue = await _appReservationIssueRepository.GetNotificationVmById(reservationIssueId);
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.RefundRequestedForYourLesson;
        var sender = reservationIssue.Reservation.Attender;
        var recipient = reservationIssue.Reservation.Attender;
        await CreateAndSendAppNotifications(
            new AppNotificationIM
            {
                RecipientId = recipient.UserId,
                SenderId = sender.UserId,
                NotificationTypeEnum = notificationTypeEnum,
                Data = new RefundRequestedNotificationVM(reservationIssue)
            });
        await SendEmail(
            recipient.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), sender.GetUserName() },
                    {
                        CEmailDataEnum.UserPhotoUrl.GetDescription(),
                        sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                    },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.StartTime, recipient.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.ExpectedEndTime, recipient.Timezone)
                    },
                    {
                        CEmailDataEnum.IssueType.GetDescription(), await GetTranslation(
                            reservationIssue.ReservationIssueType.NameKey,
                            recipient.TranslationLanguageId
                        )
                    },
                    { CEmailDataEnum.IssueDescription.GetDescription(), reservationIssue.Description },
                    { CEmailDataEnum.ActionUrl.GetDescription(), _feSettings.FeUrl },
                }
            });
        await _slackProvider.SendNotification(new SlackInputVM
        {
            NotificationTypeEnum = CNotificationTypeEnum.RefundRequestForAdmin,
            RoleId = sender.UserRoleId,
            UserName = sender.GetUserName(),
            Email = sender.Email,
            Text =
                $"student created a refund request ({reservationIssue.ReservationIssueType.EnumKey}) with message: {reservationIssue.Description}"
        });
    }

    public async Task SendRefundApproved(int reservationIssueId)
    {
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.RequestRefundApproved;
        var reservationIssue = await _appReservationIssueRepository.GetNotificationVmById(reservationIssueId);
        var studentRefundTransaction = await _appTransactionRepository.GetRawDbEntityByIssueIdAndType(
            reservationIssue.ReservationIssueId,
            CTransactionTypeEnum.RefundCreditMemo,
            reservationIssue.Reservation.Attender.UserId
        );
        var scheduledTransaction = await _appTransactionRepository.GetRawDbEntityByReservationIdIdAndType(
            reservationIssue.Reservation.ReservationId,
            CTransactionTypeEnum.LessonPayment
        );
        var tutorRefundTransaction = await _appTransactionRepository.GetRawDbEntityByIssueIdAndType(
            reservationIssue.ReservationIssueId,
            CTransactionTypeEnum.RefundCreditMemo
        );
        var tutorNewAmountToPay = scheduledTransaction.Amount - studentRefundTransaction.Amount;
        await CreateAndSendAppNotifications(
            new List<int> { reservationIssue.Reservation.Attender.UserId, reservationIssue.Reservation.Host.UserId }
                .Select(userId =>
                    new AppNotificationIM
                    {
                        RecipientId = userId,
                        SenderId = CommonConstants.TechnicalAdminUserId,
                        NotificationTypeEnum = notificationTypeEnum,
                        Data = new RefundApprovedNotificationVM(reservationIssue)
                        {
                            RefundAmount = userId == reservationIssue.Reservation.Host.UserId
                                ? tutorNewAmountToPay
                                : studentRefundTransaction?.Amount ?? 0,
                            RefundAmountCurrencyEnumKey = userId == reservationIssue.Reservation.Host.UserId
                                ? tutorRefundTransaction?.Currency?.EnumKey ?? ""
                                : studentRefundTransaction?.Currency?.EnumKey ?? ""
                        }
                    }
                ).ToList()
        );

        var tutor = reservationIssue.Reservation.Host;
        var student = reservationIssue.Reservation.Attender;
        var adminCommentDictionary = !string.IsNullOrEmpty(reservationIssue.DecisionText)
            ? new Dictionary<string, object>
            {
                { CEmailDataEnum.AdminComment.GetDescription(), reservationIssue.DecisionText }
            }
            : new Dictionary<string, object>();

        await SendEmail(
            tutor.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = tutor.Email,
                LangCode = TranslationUtil.GetLanguageCode(tutor.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), student.GetUserName() },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.StartTime, tutor.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.ExpectedEndTime, tutor.Timezone)
                    },
                    {
                        CEmailDataEnum.IssueType.GetDescription(), await GetTranslation(
                            reservationIssue.ReservationIssueType.NameKey,
                            tutor.TranslationLanguageId
                        )
                    },
                    { CEmailDataEnum.IssueDescription.GetDescription(), reservationIssue.Description },
                    { CEmailDataEnum.Tutor.GetDescription(), true },
                    {
                        CEmailDataEnum.PreviousAmountToPay.GetDescription(),
                        PriceUtil.GetPriceString(scheduledTransaction.Amount)
                    },
                    {
                        CEmailDataEnum.AmountToPay.GetDescription(),
                        PriceUtil.GetPriceString(tutorNewAmountToPay)
                    },
                    { CEmailDataEnum.ActionUrl.GetDescription(), GetTransactionTabUrl() },
                }.Concat(adminCommentDictionary).ToDictionary(x => x.Key, x => x.Value)
            });
        await SendEmail(
            student.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = student.Email,
                LangCode = TranslationUtil.GetLanguageCode(student.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), tutor.GetUserName() },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.StartTime, student.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.ExpectedEndTime, student.Timezone)
                    },
                    {
                        CEmailDataEnum.IssueType.GetDescription(), await GetTranslation(
                            reservationIssue.ReservationIssueType.NameKey,
                            student.TranslationLanguageId
                        )
                    },
                    { CEmailDataEnum.IssueDescription.GetDescription(), reservationIssue.Description },
                    { CEmailDataEnum.Student.GetDescription(), true },
                    {
                        CEmailDataEnum.RefundAmount.GetDescription(),
                        PriceUtil.GetPriceString(studentRefundTransaction.Amount)
                    },
                    { CEmailDataEnum.ActionUrl.GetDescription(), GetTransactionTabUrl() },
                }.Concat(adminCommentDictionary).ToDictionary(x => x.Key, x => x.Value)
            });
    }

    public async Task SendRefundDeclined(int reservationIssueId)
    {
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.RequestRefundDeclined;
        var reservationIssue = await _appReservationIssueRepository.GetNotificationVmById(reservationIssueId);
        await CreateAndSendAppNotifications(
            new List<int> { reservationIssue.Reservation.Attender.UserId, reservationIssue.Reservation.Host.UserId }
                .Select(userId =>
                    new AppNotificationIM
                    {
                        RecipientId = userId,
                        SenderId = CommonConstants.TechnicalAdminUserId,
                        NotificationTypeEnum = notificationTypeEnum,
                        Data = new RefundDeclinedNotificationVM(reservationIssue)
                    }
                ).ToList()
        );

        var tutor = reservationIssue.Reservation.Host;
        var student = reservationIssue.Reservation.Attender;
        var adminCommentDictionary = !string.IsNullOrEmpty(reservationIssue.DecisionText)
            ? new Dictionary<string, object>
            {
                { CEmailDataEnum.AdminComment.GetDescription(), reservationIssue.DecisionText }
            }
            : new Dictionary<string, object>();

        await SendEmail(
            tutor.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = tutor.Email,
                LangCode = TranslationUtil.GetLanguageCode(tutor.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), student.GetUserName() },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.StartTime, tutor.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.ExpectedEndTime, tutor.Timezone)
                    },
                    {
                        CEmailDataEnum.IssueType.GetDescription(), await GetTranslation(
                            reservationIssue.ReservationIssueType.NameKey,
                            tutor.TranslationLanguageId
                        )
                    },
                    { CEmailDataEnum.IssueDescription.GetDescription(), reservationIssue.Description },
                    { CEmailDataEnum.Tutor.GetDescription(), true },
                    { CEmailDataEnum.ActionUrl.GetDescription(), GetTransactionTabUrl() },
                }.Concat(adminCommentDictionary).ToDictionary(x => x.Key, x => x.Value)
            });

        await SendEmail(
            student.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = student.Email,
                LangCode = TranslationUtil.GetLanguageCode(student.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), tutor.GetUserName() },
                    {
                        CEmailDataEnum.From.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.StartTime, student.Timezone)
                    },
                    {
                        CEmailDataEnum.To.GetDescription(),
                        GetFormattedFromDate(reservationIssue.Reservation.ExpectedEndTime, student.Timezone)
                    },
                    {
                        CEmailDataEnum.IssueType.GetDescription(), await GetTranslation(
                            reservationIssue.ReservationIssueType.NameKey,
                            student.TranslationLanguageId
                        )
                    },
                    { CEmailDataEnum.IssueDescription.GetDescription(), reservationIssue.Description },
                    { CEmailDataEnum.Student.GetDescription(), true },
                    { CEmailDataEnum.ActionUrl.GetDescription(), GetTransactionTabUrl() },
                }.Concat(adminCommentDictionary).ToDictionary(x => x.Key, x => x.Value)
            });
    }

    public async Task SendMoneySent(int transactionId)
    {
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.MoneySent;
        var transaction = await _appTransactionRepository.GetNotificationVmById(transactionId);
        var sender = transaction.Approver;
        var recipient = transaction.Creator;

        await CreateAndSendAppNotifications(
            new AppNotificationIM
            {
                RecipientId = recipient.UserId,
                SenderId = sender.UserId,
                NotificationTypeEnum = notificationTypeEnum,
                Data = new WithdrawalApprovedNotificationVm()
            });
        await SendEmail(
            recipient.UserId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), sender.GetUserName() },
                    {
                        CEmailDataEnum.UserPhotoUrl.GetDescription(),
                        sender.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                    },
                    { CEmailDataEnum.ActionUrl.GetDescription(), _feSettings.FeUrl },
                }
            });
    }

    public async Task SendNewTutorWaitingForApproval(int userId)
    {
        var user = await _appUserRepository.GetById(userId, onlyConfirmed: false);
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.TutorProfileApprovalRequest,
                UserName = user.GetUserName(),
                Email = user.Email,
                RoleId = user.UserRoleId,
                Text = "finished registration and now she/he is waiting for our profile approval"
            });
    }

    public async Task SendNewWithdrawalRequest(int userId)
    {
        var user = await _appUserRepository.GetById(userId);
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.MoneyWithdrawalRequested,
                UserName = user.GetUserName(),
                Email = user.Email,
                RoleId = user.UserRoleId,
                Text = "created a withdrawal request"
            });
    }

    public async Task SendStudentDepositedMoney(int transactionId)
    {
        var transaction = await _appTransactionRepository.GetNotificationVmById(transactionId);
        var user = transaction.Creator;

        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.StudentDepositedMoney,
                UserName = user.GetUserName(),
                Email = user.Email,
                RoleId = user.UserRoleId,
                Text = $"deposited {transaction.Amount} Kč on her/his account"
            });
    }

    public async Task SendRegistrationCompleted(int userId)
    {
        var user = await _appUserRepository.GetById(userId, onlyConfirmed: false);
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.RegistrationCompleted,
                UserName = user.GetUserName(),
                Email = user.Email,
                RoleId = user.UserRoleId,
                Text = "finished registration"
            }, priorityEnum: SlackChannelPriorityEnum.Low);
    }

    public async Task SendAccountCreated(int userId)
    {
        var user = await _appUserRepository.GetRawDbEntity(userId);
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.AccountCreated,
                UserName = UserUtil.GetUserName(user.FirstName, user.LastName, user.Login),
                Email = user.Email,
                RoleId = user.UserRoleId,
                Text = user.RegistrationMethodId.HasValue
                    ? $"created an account via [{((CAuthenticationMethodEnum)user.RegistrationMethodId.Value).GetDescription()}]"
                    : "created an account"
            }, priorityEnum: SlackChannelPriorityEnum.Low);
    }

    public async Task SendCompanyInvitedEmployees(int userId, int count)
    {
        var user = await _appUserRepository.GetById(userId, onlyConfirmed: false);
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.AccountCreated,
                UserName = user.GetUserName(),
                Email = user.Email,
                RoleId = user.UserRoleId,
                Text = $"invited {count} employee(s)"
            }, priorityEnum: SlackChannelPriorityEnum.Low);
    }

    public async Task SendCompanySendMoney(int adminId, double amount, int count)
    {
        var user = await _appUserRepository.GetById(adminId, onlyConfirmed: false);
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.AccountCreated,
                UserName = user.GetUserName(),
                Email = user.Email,
                RoleId = user.UserRoleId,
                Text =
                    $"sent {PriceUtil.GetPriceString(amount)} each to {count} employees (total {PriceUtil.GetPriceString(count * amount)})"
            }, priorityEnum: SlackChannelPriorityEnum.Low);
    }

    public async Task SendTutorProfileVisited(int tutorId, int visitorId)
    {
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.TutorProfileVisited;
        var visitor = await _appUserRepository.GetById(visitorId, onlyConfirmed: false);
        var recipient = await _appUserRepository.GetNotificationInfo(tutorId);
        var conversation = await _appConversationRepository.GetRawDbEntity(new List<int> { tutorId, visitorId });
        var conversationUrl = GetConversationUrl(conversation.ConversationId);
        await CreateAndSendAppNotifications(
            new AppNotificationIM
            {
                RecipientId = tutorId,
                SenderId = visitorId,
                NotificationTypeEnum = notificationTypeEnum,
                Data = new TutorProfileVisitedNotificationVm
                {
                    UserName = visitor.GetUserName(),
                    Url = conversationUrl
                }
            });

        await SendEmail(
            tutorId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Username.GetDescription(), visitor.GetUserName() },
                    {
                        CEmailDataEnum.UserPhotoUrl.GetDescription(),
                        visitor.GetProfileThumbnailUrl(_instanceSettings.BeApiUrl)
                    },
                    { CEmailDataEnum.ActionUrl.GetDescription(), conversationUrl },
                    {
                        CEmailDataEnum.NotificationSettingsUrl.GetDescription(),
                        GetNotificationSettingsUrl(recipient.RoleEnumKey)
                    }
                }
            });
    }

    public async Task SendPersonalCoupon(int couponId)
    {
        const CNotificationTypeEnum notificationTypeEnum = CNotificationTypeEnum.Coupon;
        var coupon = await _appCouponRepository.GetRawDbEntity(couponId);
        var recipientId = coupon.UserId!.Value;
        var recipient = await _appUserRepository.GetById(coupon.UserId!.Value);
        var now = _timeUtility.GetUtcNow();
        var expiresInDays = (coupon.ValidTo - now).Days;
        await SendEmail(
            recipientId,
            new EmailDataInputVM
            {
                EmailTemplateEnum = notificationTypeEnum,
                To = recipient.Email,
                LangCode = TranslationUtil.GetLanguageCode(recipient.TranslationLanguageId),
                Params = new Dictionary<string, object>
                {
                    { CEmailDataEnum.Code.GetDescription(), coupon.Code },
                    { CEmailDataEnum.ValidTo.GetDescription(), coupon.ValidTo.ToString("dd.MM.yyyy") },
                    { CEmailDataEnum.ExpiresInDays.GetDescription(), expiresInDays },
                    { CEmailDataEnum.DiscountPercentage.GetDescription(), coupon.DiscountPercentage },
                    {
                        CEmailDataEnum.NotificationSettingsUrl.GetDescription(),
                        GetNotificationSettingsUrl(CUserRoleEnum.Student.GetEnumKey())
                    },
                }
            });
        await _slackProvider.SendNotification(
            new SlackInputVM
            {
                NotificationTypeEnum = CNotificationTypeEnum.AccountCreated,
                UserName = recipient.GetUserName(),
                Email = recipient.Email,
                RoleId = recipient.UserRoleId,
                Text =
                    $"Sent a coupon [{coupon.DiscountPercentage}%] off the first lesson that expires in [{expiresInDays}] days"
            }, priorityEnum: SlackChannelPriorityEnum.Low);
    }

    private async Task SendEmail(int recipientId, EmailDataInputVM input, bool throwIfNotSuccess = false)
    {
        var exclude =
            await _rUserNotificationBlacklistSettingRepository.ExistsEmail(recipientId, input.EmailTemplateEnum);
        if (exclude)
        {
            return;
        }

        try
        {
            await _emailProvider.SendEmail(input, throwIfNotSuccess);
            await _rUserNotificationRepository.Create(recipientId, CNotificationChannelEnum.Email,
                input.EmailTemplateEnum, CNotificationStateEnum.Created);
        }
        catch (Exception)
        {
            await _rUserNotificationRepository.Create(recipientId, CNotificationChannelEnum.Email,
                input.EmailTemplateEnum, CNotificationStateEnum.FailedToSend);
            if (throwIfNotSuccess)
            {
                throw;
            }
        }
    }

    private async Task<string> GetTranslation(string nameKey, int? languageId)
    {
        var translations =
            await _cTranslationRepository.GetTranslationByKeyPrefix(languageId ?? (int)CTranslationLanguageEnum.English,
                nameKey);
        return TranslationUtil.Translate(translations, nameKey);
    }

    private Task CreateAndSendAppNotifications(AppNotificationIM notification)
    {
        return CreateAndSendAppNotifications(new List<AppNotificationIM> { notification });
    }

    private async Task CreateAndSendAppNotifications(IEnumerable<AppNotificationIM> notifications)
    {
        var notificationIds = await _appNotificationRepository.CreateRange(notifications);
        var notificationVms = await _appNotificationRepository.GetAll(notificationIds);
        await _chatHubService.SendNewNotification(notificationVms);
    }

    private string GetFormattedFromDate(DateTime fromTime, string timezone)
    {
        var convertedTime = _timeUtility.ConvertFromUtcToLocalTime(timezone, fromTime);
        return convertedTime.ToString("dd.MM.yyyy HH:mm");
    }

    private string GetFormattedToDate(DateTime toTime, string timezone)
    {
        var convertedTime = _timeUtility.ConvertFromUtcToLocalTime(timezone, toTime);
        return convertedTime.ToString("HH:mm");
    }

    private string GetNumberOfMinutesUntilStart(DateTime startTime)
    {
        var now = _timeUtility.GetUtcNow();

        return $"{(int)Math.Round((startTime - now).TotalMinutes)}";
    }

    private string GetRoomUrl(string roomKey)
    {
        return $"{UrlUtil.GetBaseUrl(_feSettings.FeUrl)}meeting/{roomKey}";
    }

    private string GetReviewPageUrl()
    {
        return $"{UrlUtil.GetBaseUrl(_feSettings.FeUrl)}tutor/dashboard?tab=reviews";
    }

    private string GetTransactionTabUrl()
    {
        return $"{UrlUtil.GetBaseUrl(_feSettings.FeUrl)}tutor/dashboard?tab=transactions";
    }

    private string GetRegisterUrlWithCodeParameter(string registrationCode, string registrationToken)
    {
        return
            $"{UrlUtil.GetBaseUrl(_registrationSettings.RedirectUrl)}?code={registrationCode}&registrationToken={registrationToken}";
    }

    private string GetResetPasswordUrl(string registrationToken)
    {
        return
            $"{UrlUtil.GetBaseUrl(_registrationSettings.ResetPasswordRedirectUrl)}?registrationToken={registrationToken}";
    }

    private string GetConversationUrl(int conversationId)
    {
        return $"{UrlUtil.GetBaseUrl(_feSettings.FeUrl)}messages?conversationId={conversationId}";
    }

    private object GetMatchingTutorsUrl()
    {
        return $"{UrlUtil.GetBaseUrl(_feSettings.FeUrl)}student/dashboard?tab=matching_tutors";
    }

    private string GetNotificationSettingsUrl(string role)
    {
        return $"{UrlUtil.GetBaseUrl(_feSettings.FeUrl)}{role}/settings";
    }
}