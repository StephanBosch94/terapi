using System.Linq.Expressions;
using ULearn.Be.File.Data;

namespace ULearn.Be.File.Service.ViewModels.Mappers;

public static class AppFileMapper
{
    public static readonly Expression<Func<AppFile, AppFileRaw>> AsRawViewModel =
        dataItem => new AppFileRaw
        {
            Path = dataItem.Path,
            FileName = dataItem.FileName,
            Environment = dataItem.Environment,
            ContentType = dataItem.ContentType,
            FileId = dataItem.FileId,
            UniqueKey = dataItem.UniqueKey,
            CreatorId = dataItem.CreatorId,
            UpdaterId = dataItem.UpdaterId,
            Updated = dataItem.Updated,
            Created = dataItem.Created,
            FileStateId = dataItem.StateId,
            CompressionTypeId = dataItem.CompressionTypeId
        };


    public static IQueryable<AppFileRaw> ToRawList(this IQueryable<AppFile> dataCollection)
    {
        return dataCollection.Select(AsRawViewModel);
    }

    public static AppFileRaw ToAppFileRaw(this AppFile appFile)
    {
        return AsRawViewModel.Compile()(appFile);
    }
}
