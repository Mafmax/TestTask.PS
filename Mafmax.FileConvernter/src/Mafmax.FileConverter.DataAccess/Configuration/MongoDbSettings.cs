namespace Mafmax.FileConverter.DataAccess.Configuration;
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public string FilesCollection { get; set; } = string.Empty;

    public string FilesGrid { get; set; } = string.Empty;
}