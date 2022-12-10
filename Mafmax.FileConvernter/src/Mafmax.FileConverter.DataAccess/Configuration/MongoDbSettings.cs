namespace Mafmax.FileConverter.DataAccess.Configuration;

/// <summary>
/// Configuration settings of Mongo database.
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// String to connect to database.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Name of main database.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// Mongo collection with files.
    /// </summary>
    public string FilesCollection { get; set; } = string.Empty;

    /// <summary>
    /// GridFS name.
    /// </summary>
    public string FilesGrid { get; set; } = string.Empty;
}