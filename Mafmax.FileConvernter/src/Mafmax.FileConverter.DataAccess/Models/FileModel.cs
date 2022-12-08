using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mafmax.FileConverter.DataAccess.Models;

public class FileModel
{
    /// <summary>
    /// Id of model.
    /// </summary>
    [BsonId]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Id of file content in a GridFS.
    /// </summary>
    public string GridFsFileId { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset CreatedAt { get; set; }
}
