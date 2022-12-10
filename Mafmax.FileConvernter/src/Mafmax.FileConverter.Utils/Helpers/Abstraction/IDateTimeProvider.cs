namespace Mafmax.FileConverter.Utils.Helpers.Abstraction;
/// <summary>
/// Defines methods to work with system date time.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// <inheritdoc cref="DateTimeOffset.UtcNow"/>
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
