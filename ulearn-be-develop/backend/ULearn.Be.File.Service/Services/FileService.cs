using System.IO.Compression;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Common.Services.Utilities;
using ULearn.Be.File.Data;
using ULearn.Be.File.Data.Context;
using ULearn.Be.File.Data.Enums;
using ULearn.Be.File.Service.Services.Extensions;
using ULearn.Be.File.Service.Services.Providers;
using ULearn.Be.File.Service.ViewModels;
using ULearn.Be.File.Service.ViewModels.Mappers;

namespace ULearn.Be.File.Service.Services;

public class FileService : BaseService, IFileService
{
    private readonly FileRepositoryContext _db;
    private readonly IFileProviderService _fileProviderService;
    private readonly IGuidUtility _guidUtility;
    private readonly ITimeUtility _timeUtility;

    public FileService(
        FileRepositoryContext db,
        ITimeUtility timeUtility,
        IGuidUtility guidUtility,
        IFileProviderService fileProviderService
    )
    {
        _db = db;
        _timeUtility = timeUtility;
        _guidUtility = guidUtility;
        _fileProviderService = fileProviderService;
    }

    public async Task<(Stream, AppFileRaw)> GetFileStream(
        string environment,
        string filepath,
        bool ignoreDeleted = true
    )
    {
        var entity = await GetFile(environment, filepath, ignoreDeleted: ignoreDeleted)
            .ToRawList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"Invalid path {filepath} for environment {environment}", entity);
        var stream = _fileProviderService.GetFile(entity.UniqueKey, (CCompressionTypeEnum)entity.CompressionTypeId);

        return (stream, entity);
    }

    public async Task<(Stream, string, string)> GetArchiveStream(
        string environment,
        string filepath,
        bool ignoreDeleted = true
    )
    {
        var entities = await GetFileByPath(environment, filepath, ignoreDeleted: ignoreDeleted)
            .ToRawList()
            .ToListAsync();

        if (!entities.Any())
        {
            throw new NotFoundException($"Invalid path {filepath} for environment {environment}");
        }

        var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var entity in entities)
            {
                var stream = _fileProviderService.GetFile(entity.UniqueKey,
                    (CCompressionTypeEnum)entity.CompressionTypeId);

                var file = archive.CreateEntry(entity.FileName);
                await using var entryStream = file.Open();
                await stream.CopyToAsync(entryStream);

                stream.Close();
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        return (memoryStream, "application/gzip", "archive.gzip");
    }

    public async Task<IReadOnlyCollection<AppFileRaw>> SaveFile(
        string environment,
        string filepath,
        IFormFileCollection files,
        ClaimsPrincipal principal
    )
    {
        if (await Exists(files.Select(x => CreateFullPath(filepath, x.FileName))))
        {
            throw new ConflictException(ErrorCodesEnum.Exists);
        }

        var result = new List<AppFileRaw>(files.Count);
        var timeStamp = _timeUtility.GetUtcNow();
        var userId = principal.GetUserId();
        foreach (var file in files)
        {
            var uniqueKey = GenerateUniqueKey(environment);
            var compressionType = await GetCompressionType(file.FileName);
            var dbEntity = new AppFile
            {
                Created = timeStamp,
                Updated = timeStamp,
                Environment = environment,
                FileName = file.FileName,
                Path = CreateFullPath(filepath, file.FileName),
                CreatorId = userId,
                UpdaterId = userId,
                UniqueKey = uniqueKey,
                ContentType = file.ContentType ?? MimeTypeConstants.Octet,
                CompressionTypeId = (int)compressionType
            };

            await _fileProviderService.SaveFile(uniqueKey, file, compressionType);
            _db.Add(dbEntity);
            result.Add(dbEntity.ToAppFileRaw());
        }

        await _db.SaveChangesAsync();

        return result;
    }

    public async Task DeleteFile(string environment, string filePath, ClaimsPrincipal user)
    {
        var entity = await GetFile(environment, filePath, false)
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"Invalid path {filePath} for environment {environment}", entity);

        await _fileProviderService.DeleteFile(entity.UniqueKey);
        await Delete(entity);
    }

    public async Task EditFile(string environment, string filePath, AppFileEditVM edit, ClaimsPrincipal user)
    {
        var entity = await GetFile(environment, filePath, false)
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"Invalid path {filePath} for environment {environment}", entity);

        entity.FileName = edit.FileName;
        entity.Path = CreateFullPath(edit.Path, edit.FileName);
        await Update(entity, user.GetUserId());
    }

    public async Task<bool> ExistsFile(string environment, string filePath)
    {
        return await GetFile(environment, filePath)
            .AnyAsync();
    }

    public async Task<AppFileRaw> GetFileInfo(string environment, string filepath)
    {
        var entity = await GetFile(environment, filepath)
            .ToRawList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"File not found for path {filepath} and environment {environment}", entity);

        return entity;
    }

    public async Task<AppFileRaw> GetFileInfo(string fileKey)
    {
        var entity = await GetFile(fileKey)
            .ToRawList()
            .FirstOrDefaultAsync();

        ThrowIfNotFound($"File with fileKey {fileKey} not found", entity);

        return entity;
    }

    private async Task Delete(AppFile entity)
    {
        _db.Remove(entity);
        await _db.SaveChangesAsync();
    }

    private async Task<CCompressionTypeEnum> GetCompressionType(string fileFileName)
    {
        var compressionTypeId = await _db.RFileFormatCompressionTypes
            .Where(x => fileFileName.Contains(x.FileFormat.Extension))
            .Select(x => (int?)x.CompressionTypeId)
            .FirstOrDefaultAsync();
        if (compressionTypeId.HasValue)
        {
            return (CCompressionTypeEnum)compressionTypeId.Value;
        }

        return CCompressionTypeEnum.None;
    }

    private IQueryable<AppFile> GetFile(
        string environment,
        string path,
        bool asNoTracking = true,
        bool ignoreDeleted = true
    )
    {
        return Query(asNoTracking)
            .Where(o => o.Path == path)
            .Where(o => o.Environment == environment)
            .Where(o => !ignoreDeleted || o.StateId == (int)CFileStateEnum.Active);
    }

    private IQueryable<AppFile> GetFileByPath(
        string environment,
        string path,
        bool asNoTracking = true,
        bool ignoreDeleted = true
    )
    {
        return Query(asNoTracking)
            .Where(o => o.Path.StartsWith(path))
            .Where(o => o.Environment == environment)
            .Where(o => !ignoreDeleted || o.StateId == (int)CFileStateEnum.Active);
    }

    private IQueryable<AppFile> GetFile(
        string fileKey,
        bool asNoTracking = true,
        bool ignoreDeleted = true
    )
    {
        return Query(asNoTracking)
            .Where(o => o.UniqueKey == fileKey)
            .Where(o => !ignoreDeleted || o.StateId == (int)CFileStateEnum.Active);
    }

    private string GenerateUniqueKey(string env)
    {
        var uniqueKey = _guidUtility.GenerateGuidString();

        return CreateRealFileName(env, uniqueKey);
    }

    private static string CreateRealFileName(string env, string uniqueKey)
    {
        return $"{env}-{uniqueKey}";
    }

    private static string CreateFullPath(string directory, string fileName)
    {
        return Path.Combine(directory.NormalizeDirectoryPath(), fileName);
    }

    private Task<bool> Exists(IEnumerable<string> paths)
    {
        return Query()
            .Where(o => paths.Contains(o.Path))
            .AnyAsync();
    }

    private async Task Update(AppFile file, int updaterId)
    {
        file.UpdaterId = updaterId;
        file.Updated = _timeUtility.GetUtcNow();
        _db.Update(file);

        await _db.SaveChangesAsync();
    }


    private IQueryable<AppFile> Query(bool asNoTracking = true)
    {
        var query = _db.AppFiles;

        return asNoTracking ? query.AsNoTracking() : query;
    }
}