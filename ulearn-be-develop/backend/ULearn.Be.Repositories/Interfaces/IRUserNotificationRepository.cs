using ULearn.Be.Data.Enums;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserNotificationRepository
{
    Task Create(int recipientId, CNotificationChannelEnum channelEnum, CNotificationTypeEnum notificationTypeEnum,
        CNotificationStateEnum stateEnum);
}