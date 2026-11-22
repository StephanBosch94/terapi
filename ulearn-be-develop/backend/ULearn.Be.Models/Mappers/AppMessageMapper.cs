using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.AppMessage;

namespace ULearn.Be.Models.Mappers;

public static class AppMessageMapper
{
    private static readonly Expression<Func<AppMessage, AppMessageVM>> AsVM =
        dataItem => new AppMessageVM
        {
            ConversationId = dataItem.ConversationId,
            MessageId = dataItem.MessageId,
            Created = dataItem.Created,
            Updated = dataItem.Updated,
            TextValue = dataItem.TextValue,
            MessageStateId = (CMessageStateEnum)dataItem.MessageStateId,
            SenderId = dataItem.SenderId,
            ReceiverId = dataItem.ReceiverId,
            Attachments = dataItem.AppMessageAttachments.Select(x =>
                new MessageAttachmentVM
                {
                    Name = x.Name,
                    ContentType = x.ContentType,
                    FilePath = x.FilePath
                })
        };

    private static readonly Expression<Func<AppMessage, AppMessageForNotificationVM>>
        AsAppConversationForNotificationVM =
            dataItem => new AppMessageForNotificationVM
            {
                ConversationId = dataItem.ConversationId,
                MessageId = dataItem.MessageId,
                Sender = AppUserMapper.ToCommonVm(dataItem.Sender),
                Recipient = AppUserMapper.ToCommonVm(dataItem.Receiver),
            };

    public static IQueryable<AppMessageVM> ToApiList(this IQueryable<AppMessage> dataCollection)
    {
        return dataCollection.Select(AsVM);
    }

    public static IQueryable<AppMessageForNotificationVM> ToAppMessageForNotificationList(
        this IQueryable<AppMessage> dataCollection)
    {
        return dataCollection
            .Include(x => x.Sender)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Receiver)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsAppConversationForNotificationVM);
    }
}