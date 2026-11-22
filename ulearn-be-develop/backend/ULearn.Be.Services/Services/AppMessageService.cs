using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Models;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Utils;
using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.AppConversation;
using ULearn.Be.Models.ViewModels.AppMessage;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.HandlerServices.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AppMessageService : IAppMessageService
{
    private readonly IAppConversationRepository _appConversationRepository;
    private readonly IAppMessageAttachmentRepository _appMessageAttachmentRepository;
    private readonly IAppMessageRepository _appMessageRepository;
    private readonly IGuidUtility _guidUtility;
    private readonly IMessageEventHandlerService _messageEventHandlerService;
    private readonly IRUserFilePermissionRepository _rUserFilePermissionRepository;

    public AppMessageService(
        IAppMessageRepository appMessageRepository,
        IAppConversationRepository appConversationRepository,
        IMessageEventHandlerService messageEventHandlerService,
        IAppMessageAttachmentRepository appMessageAttachmentRepository,
        IRUserFilePermissionRepository rUserFilePermissionRepository,
        IGuidUtility guidUtility
    )
    {
        _appMessageRepository = appMessageRepository;
        _appConversationRepository = appConversationRepository;
        _messageEventHandlerService = messageEventHandlerService;
        _appMessageAttachmentRepository = appMessageAttachmentRepository;
        _rUserFilePermissionRepository = rUserFilePermissionRepository;
        _guidUtility = guidUtility;
    }

    public Task<PagedVM<AppMessageVM>> Filter(
        int conversationId,
        FilterView<MessageFilterVM> filter,
        ClaimsPrincipal user
    )
    {
        return _appMessageRepository.Filter(conversationId, filter, user.GetUserId());
    }

    public async Task Create(int opponentId, string messageText, IFormFileCollection formFiles, ClaimsPrincipal user)
    {
        var attachments = new List<MessageAttachmentInputVM>();
        
        foreach (var file in formFiles)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            
            attachments.Add(new MessageAttachmentInputVM
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Directory = $"{FileUtil.MessageAttachmentDirectory}/{_guidUtility.GenerateGuidString()}",
                Content = memoryStream.ToArray()
            });
        }

        await CreateMessage(
            new MessageInputVM
            {
                OpponentId = opponentId,
                TextValue = messageText,
                Attachments = attachments
            }, user);
    }

    private async Task<int> CreateMessage(MessageInputVM message, ClaimsPrincipal user)
    {
        var messageSenderId = user.GetUserId();
        var conversation =
            await _appConversationRepository.GetRawDbEntity(new List<int> { messageSenderId, message.OpponentId },
                false)
            ?? await _appConversationRepository.Create(new AppConversationInputVM { OpponentId = message.OpponentId },
                messageSenderId);

        var messageReceiverId = messageSenderId == conversation.InitiatorId
            ? conversation.OpponentId
            : conversation.InitiatorId;

        var messageId = await _appMessageRepository.CreateMessage(conversation.ConversationId, messageSenderId,
            messageReceiverId, message.TextValue);
        await _appMessageAttachmentRepository.Add(messageId, message.Attachments);
        await _messageEventHandlerService.HandleAdd(messageId, messageSenderId);
        await _rUserFilePermissionRepository.Add(
            message.Attachments.Select(x => x.FilePath).ToHashSet(),
            new List<int> { messageSenderId, message.OpponentId }
        );
        return messageId;
    }
}