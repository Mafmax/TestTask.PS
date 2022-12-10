namespace Mafmax.FileConverter.SharedConfiguration.Options;

/// <summary>
/// Settings binded with application.
/// </summary>
public class ApplicationSettings
{
    /// <summary>
    /// Indicates whether to provide detailed message of errors.
    /// </summary>
    public bool DetailedError { get; set; } = default!;

    /// <summary>
    /// Period after which file marks as expired.
    /// </summary>
    public TimeSpan FilesAgeLimit { get; set; } = default!;
}