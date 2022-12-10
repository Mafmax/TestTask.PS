using Mafmax.FileConverter.Utils.Helpers.Abstraction;

namespace Mafmax.FileConverter.Utils.Helpers;

/// <summary>
/// Provider for current date and time.
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTime.UtcNow;
}
