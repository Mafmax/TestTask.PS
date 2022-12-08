using Mafmax.FileConverter.DataAccess.Configuration;
using Mafmax.FileConverter.DataAccess.Extensions;
using Mafmax.FileConverter.DataAccess.Models;
using Mafmax.FileConverter.DataAccess.Repositories.Abstractions;
using Mafmax.FileConverter.DataAccess.Responses;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;
using Mafmax.FileConverter.Utils.Proxies;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Mafmax.FileConverter.DataAccess.Repositories;

public class FilesRepository : IFilesRepository
{
    private readonly GridFSBucket _gridFs;
    private readonly IThrowHelper _throwHelper;
    private readonly IDateTimeProvider _timeService;
    private readonly IMongoCollection<FileModel> _filesCollection;

    public FilesRepository(
        IMongoDatabase db,
        IThrowHelper throwHelper,
        IDateTimeProvider timeService,
        IOptions<MongoDbSettings> mongoDbSettings)
    {
        _filesCollection = db.GetCollection<FileModel>(mongoDbSettings.Value.FilesCollection);
        _gridFs = db.GetGridFs();
        _throwHelper = throwHelper;
        _timeService = timeService;
    }

    /// <inheritdoc />
    public async Task<ReadFileResponse> ReadFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var file = await _filesCollection
            .Find(x => x.Id == fileId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (file is null)
        {
            _throwHelper.FileNotFound(fileId);
        }

        var downloadStream = await _gridFs
            .OpenDownloadStreamAsync(ObjectId.Parse(file.GridFsFileId), new(), cancellationToken);

        return new(file, downloadStream);
    }

    /// <inheritdoc />
    public async Task<string> SaveFileAsync(ReadFileResponse fileToSave, CancellationToken cancellationToken = default)
    {
        var fileName = fileToSave.File.Name;
        var sourceStream = fileToSave.StreamToReadFile;
        var fileGridFsId = await _gridFs
            .UploadFromStreamAsync(fileName, sourceStream, new(), cancellationToken);
        PrepareFileModelToSave(fileToSave.File);
        fileToSave.File.GridFsFileId = fileGridFsId.ToString();
        await _filesCollection.InsertOneAsync(fileToSave.File, new(), cancellationToken);

        return fileToSave.File.Id;
    }

    /// <inheritdoc />
    public async Task<WriteFileResponse> SaveFileAsync(FileModel file, CancellationToken cancellationToken = default)
    {
        var uploadStream = await _gridFs.OpenUploadStreamAsync(file.Name, cancellationToken: cancellationToken);

        file.GridFsFileId = uploadStream.Id.ToString();
        var fileNewId = ObjectId.GenerateNewId().ToString();
        file.Id = fileNewId;
        file.CreatedAt = _timeService.UtcNow;
        var insertOptions = new InsertOneOptions();
        var uploadStreamProxy = new CallbackStreamProxy(uploadStream)
            .SetAsyncCallback(async () => await _filesCollection
                .InsertOneAsync(file, insertOptions, cancellationToken))
            .SetSyncCallback(() => _filesCollection
                .InsertOne(file, insertOptions, cancellationToken));

        return new(fileNewId, uploadStreamProxy);
    }

    /// <inheritdoc />
    public async Task DeleteFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var file = await _filesCollection
            .Find(x => x.Id == fileId)
            .FirstOrDefaultAsync(cancellationToken);

        if (file is null)
        {
            _throwHelper.FileNotFound(fileId);
        }

        var deleteFileTask = _gridFs
            .DeleteAsync(ObjectId.Parse(file.GridFsFileId), cancellationToken);

        var deleteGridFsTask = _filesCollection
           .DeleteOneAsync(x => x.Id == fileId, cancellationToken);

        await Task.WhenAll(deleteFileTask, deleteGridFsTask);
    }

    /// <inheritdoc />
    public async Task<string> GetFileNameAsync(string fileId, CancellationToken cancellationToken)
    {
        var filePointer = await _filesCollection
            .Find(x => x.Id == fileId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (filePointer is null)
        {
            _throwHelper.FileNotFound(fileId);
        }

        return filePointer.Name;
    }

    /// <inheritdoc />
    public async Task<int> RemoveOldFilesAsync(TimeSpan fileAgeLimit, CancellationToken cancellationToken = default)
    {
        var now = _timeService.UtcNow;
        var deleteResult = await _filesCollection.DeleteManyAsync(x => (now - x.CreatedAt) >= fileAgeLimit, new(), cancellationToken);

        return (int)deleteResult.DeletedCount;
    }

    private void PrepareFileModelToSave(FileModel file)
    {
        file.CreatedAt = _timeService.UtcNow;
        if (string.IsNullOrEmpty(file.Id))
        {
            file.Id = ObjectId.GenerateNewId().ToString();
        }
    }
}