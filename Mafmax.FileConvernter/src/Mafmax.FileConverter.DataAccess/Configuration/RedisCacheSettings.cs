namespace Mafmax.FileConverter.DataAccess.Configuration;
public class RedisCacheSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}
