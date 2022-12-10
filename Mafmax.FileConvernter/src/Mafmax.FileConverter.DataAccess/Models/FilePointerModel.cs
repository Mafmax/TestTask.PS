using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mafmax.FileConverter.DataAccess.Models;

/// <summary>
/// Represents a lightweight file model with link to content.
/// </summary>
/// <param name="Id">Id of model.</param>
/// <param name="FileId">Id of file content.</param>
/// <param name="Name">File name with extension and without path.</param>
/// <param name="CreatedAt">Created date.</param>
public record FilePointerModel(
    [property: BsonId] string Id,
    string FileId,
    string Name,
    [property: BsonRepresentation(BsonType.DateTime)]
    DateTimeOffset CreatedAt)
{
    /// <summary>
    /// Creates an instance of <see cref="FilePointerModel"/>.
    /// </summary>
    /// <param name="name">Name of file.</param>
    public FilePointerModel(string name) : this(null!, null!, name, default)
    {
    }
}
