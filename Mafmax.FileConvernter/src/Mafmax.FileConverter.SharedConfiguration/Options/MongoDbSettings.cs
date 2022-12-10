namespace Mafmax.FileConverter.SharedConfiguration.Options;

/// <summary>
/// Configuration settings of Mongo database.
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// String to connect to database.
    /// </summary>
    public string ConnectionString { get; set; } = default!;

    /// <summary>
    /// Name of main database.
    /// </summary>
    public string DatabaseName { get; set; } = default!;

    /// <summary>
    /// Mongo collection with files.
    /// </summary>
    public string FilesCollection { get; set; } = default!;

    /// <summary>
    /// GridFS name.
    /// </summary>
    public string FilesGrid { get; set; } = default!;
}