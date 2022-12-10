using Mafmax.FileConverter.DataAccess.Configuration;
using Mafmax.FileConverter.DataAccess.Extensions;
using Mafmax.FileConverter.DataAccess.Models;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Abstractions;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Responses;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;
using Mafmax.FileConverter.Utils.Proxies;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Mafmax.FileConverter.DataAccess.Repositories.FilesRepository;

/// <summary>
/// Class to abstracts work with files storage.
/// </summary>
public class FilesRepository : IFilesRepository
{
    private readonly GridFSBucket _gridFs;
    private readonly IThrowHelper _throwHelper;
    private readonly IDateTimeProvider _timeService;
    private readonly IMongoCollection<FilePointerModel> _filesCollection;

    /// <summary>
    /// Creates an instance of <see cref="FilesRepository"/>.
    /// </summary>
    public FilesRepository(
        IMongoDatabase db,
        IThrowHelper throwHelper,
        IDateTimeProvider timeService,
        IOptions<MongoDbSettings> mongoDbSettings)
    {
        _filesCollection = db.GetCollection<FilePointerModel>(mongoDbSettings.Value.FilesCollection);
        _gridFs = db.GetGridFs();
        _throwHelper = throwHelper;
        _timeService = timeService;
    }

    /// <inheritdoc />
    public async Task<ReadFileResponse> OpenFileToReadAsync(string filePointerId, CancellationToken cancellationToken = default)
    {
        var file = await _filesCollection
            .Find(x => x.Id == filePointerId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (file is null)
        {
            _throwHelper.FileNotFound(filePointerId);
        }

        var downloadStream = await _gridFs
            .OpenDownloadStreamAsync(ObjectId.Parse(file.FileId), new(), cancellationToken);

        return new(file, downloadStream);
    }

    /// <inheritdoc />
    public async Task<string> SaveFileAsync(ReadFileResponse file, CancellationToken cancellationToken = default)
    {
        var fileName = file.FilePointer.Name;
        var sourceStream = file.Stream;
        var fileGridFsId = await _gridFs
            .UploadFromStreamAsync(fileName, sourceStream, new(), cancellationToken);
        var fileToSave = CreateFilePointerToSave(file.FilePointer, fileGridFsId.ToString());
        await _filesCollection.InsertOneAsync(fileToSave, new(), cancellationToken);

        return fileToSave.Id;
    }

    /// <inheritdoc />
    public async Task<WriteFileResponse> OpenFileToSaveAsync(FilePointerModel filePointer, CancellationToken cancellationToken = default)
    {
        var uploadStream = await _gridFs.OpenUploadStreamAsync(filePointer.Name, cancellationToken: cancellationToken);

        var fileToSave = CreateFilePointerToSave(filePointer, uploadStream.Id.ToString())
            with
        {
            Id = ObjectId.GenerateNewId().ToString()
        };
        var insertOptions = new InsertOneOptions();
        var uploadStreamProxy = new CallbackStreamProxy(uploadStream)
            .SetAsyncCallback(async () => await _filesCollection
                .InsertOneAsync(fileToSave, insertOptions, cancellationToken))
            .SetSyncCallback(() => _filesCollection
                .InsertOne(fileToSave, insertOptions, cancellationToken));

        return new(fileToSave.Id, uploadStreamProxy);
    }

    /// <inheritdoc />
    public async Task DeleteFileAsync(string filePointerId, CancellationToken cancellationToken = default)
    {
        var file = await _filesCollection
            .Find(x => x.Id == filePointerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (file is null)
        {
            _throwHelper.FileNotFound(filePointerId);
        }

        var deleteFileTask = _gridFs
            .DeleteAsync(ObjectId.Parse(file.FileId), cancellationToken);

        var deleteGridFsTask = _filesCollection
           .DeleteOneAsync(x => x.Id == filePointerId, cancellationToken);

        await Task.WhenAll(deleteFileTask, deleteGridFsTask);
    }

    /// <inheritdoc />
    public async Task<string> GetFileNameAsync(string filePointerId, CancellationToken cancellationToken)
    {
        var filePointer = await _filesCollection
            .Find(x => x.Id == filePointerId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (filePointer is null)
        {
            _throwHelper.FileNotFound(filePointerId);
        }

        return filePointer.Name;
    }

    /// <inheritdoc />
    public async Task<int> RemoveOldFilesAsync(TimeSpan fileAgeLimit, CancellationToken cancellationToken = default)
    {
        var now = _timeService.UtcNow;
        var deleteResult = await _filesCollection.DeleteManyAsync(x => now - x.CreatedAt >= fileAgeLimit, new(), cancellationToken);

        return (int)deleteResult.DeletedCount;
    }

    private FilePointerModel CreateFilePointerToSave(FilePointerModel templateFilePointer, string? newFileId = null) =>
        templateFilePointer with
        {
            CreatedAt = _timeService.UtcNow,
            Id = ObjectId.GenerateNewId().ToString(),
            FileId = newFileId ?? templateFilePointer.FileId
        };
}