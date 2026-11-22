using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data;
using ULearn.Be.Data.Context;
using ULearn.Be.Models.ViewModels.AppMessage;
using ULearn.Be.Repositories.Interfaces;

namespace ULearn.Be.Repositories;

public class AppMessageAttachmentRepository : IAppMessageAttachmentRepository
{
    private readonly RepositoryContext _db;
    private readonly ITimeUtility _timeUtility;

    public AppMessageAttachmentRepository(
        RepositoryContext db,
        ITimeUtility timeUtility
    )
    {
        _db = db;
        _timeUtility = timeUtility;
    }

    public Task Add(int messageId, IEnumerable<MessageAttachmentInputVM> attachments)
    {
        var appMessageAttachments = attachments.Select(
            x => new AppMessageAttachment
            {
                MessageId = messageId,
                Name = x.FileName,
                ContentType = x.ContentType,
                FilePath = x.FilePath,
                Content = x.Content
            }).ToList();
        _db.AppMessageAttachments.AddRange(appMessageAttachments);
        return _db.SaveChangesAsync();
    }

    public Task<AppMessageAttachment> GetByFilePath(string filePath)
    {
        return Query()
            .Where(x => x.FilePath == filePath)
            .FirstOrDefaultAsync();
    }

    private IQueryable<AppMessageAttachment> Query(bool asNoTracking = true)
    {
        var query = _db.AppMessageAttachments;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}