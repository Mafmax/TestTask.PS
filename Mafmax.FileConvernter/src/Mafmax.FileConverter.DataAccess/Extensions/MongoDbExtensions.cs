using Mafmax.FileConverter.DataAccess.Configuration;
using Mafmax.FileConverter.DataAccess.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Mafmax.FileConverter.DataAccess.Extensions;
public static class MongoDbExtensions
{
    public static IMongoDatabase SetUpDatabase(this IMongoDatabase db, MongoDbSettings settings)
    {
        var indexKeysDefine = Builders<FileModel>.IndexKeys.Hashed(x => x.GridFsFileId);
        var filesCollection = db.GetCollection<FileModel>(settings.FilesCollection);
        filesCollection.Indexes.CreateOne(new CreateIndexModel<FileModel>(indexKeysDefine));

        return db;
    }

    public static GridFSBucket GetGridFs(this IMongoDatabase db) => 
        new (db);
}
