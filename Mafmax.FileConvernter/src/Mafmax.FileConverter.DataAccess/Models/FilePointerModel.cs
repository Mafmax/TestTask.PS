using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mafmax.FileConverter.DataAccess.Models;

/// <summary>
/// Represents a lightweight file model with link to content.
/// </summary>
public record FilePointerModel
{
    /// <summary>
    /// Id of model.
    /// </summary>
    [BsonId]
    public string Id { get; init; } = null!;

    /// <summary>
    /// Id of file content.
    /// </summary>
    public string FileId { get; init; } = null!;

    /// <summary>
    /// File name with extension and without path.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Created date.
    /// </summary>
    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset CreatedAt { get; init; }
}
