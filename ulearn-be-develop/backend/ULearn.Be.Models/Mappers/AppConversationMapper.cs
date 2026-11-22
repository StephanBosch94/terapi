using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppConversation;

namespace ULearn.Be.Models.Mappers;

public static class AppConversationMapper
{
    private static readonly Expression<Func<AppConversation, AppConversationVM>> AsVM =
        dataItem => new AppConversationVM
        {
            ConversationId = dataItem.ConversationId,
            Created = dataItem.Created,
            Updated = dataItem.Updated,
            Initiator = AppUserMapper.ToCommonVm(dataItem.Initiator),
            Opponent = AppUserMapper.ToCommonVm(dataItem.Opponent)
        };


    public static IQueryable<AppConversationVM> ToApiList(this IQueryable<AppConversation> dataCollection)
    {
        return dataCollection
            .Include(x => x.Initiator)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Opponent)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsVM);
    }
}