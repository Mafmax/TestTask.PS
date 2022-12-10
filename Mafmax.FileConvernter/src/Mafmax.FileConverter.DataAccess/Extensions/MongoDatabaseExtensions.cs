using Mafmax.FileConverter.DataAccess.Configuration;
using Mafmax.FileConverter.DataAccess.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Mafmax.FileConverter.DataAccess.Extensions;

/// <summary>
/// Contains extensions for <see cref="IMongoDatabase"/>.
/// </summary>
public static class MongoDatabaseExtensions
{
    /// <summary>
    /// Configures mongo db.
    /// </summary>
    /// <param name="db">Database.</param>
    /// <param name="settings">Configuration settings.</param>
    /// <returns></returns>
    public static IMongoDatabase SetUpDatabase(this IMongoDatabase db, MongoDbSettings settings)
    {
        var indexKeysDefine = Builders<FilePointerModel>.IndexKeys.Hashed(x => x.FileId);
        var filesCollection = db.GetCollection<FilePointerModel>(settings.FilesCollection);
        filesCollection.Indexes.CreateOne(new CreateIndexModel<FilePointerModel>(indexKeysDefine));

        return db;
    }

    /// <summary>
    /// Gets GridFS storage representation of <paramref name="db"/>.
    /// </summary>
    /// <param name="db">Database.</param>
    /// <returns>GridFS storage representation.</returns>
    public static GridFSBucket GetGridFs(this IMongoDatabase db) => 
        new (db);
}
