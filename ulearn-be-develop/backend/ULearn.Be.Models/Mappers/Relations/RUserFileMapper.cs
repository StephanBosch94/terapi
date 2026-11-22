using System.Linq.Expressions;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.Mappers.Relations;

public static class RUserFileMapper
{
    public static readonly Expression<Func<RUserFile, UserFileVM>> AsVM =
        dataItem => new UserFileVM
        {
            UserId = dataItem.UserId,
            ContentType = dataItem.ContentType,
            FileName = dataItem.FileName,
            FilePath = dataItem.FilePath,
            FileTypeId = dataItem.FileTypeId
        };

    public static IQueryable<UserFileVM> ToApiList(this IQueryable<RUserFile> dataItems)
    {
        return dataItems.Select(AsVM);
    }
}