using Mafmax.FileConverter.Utils.Helpers.Abstraction;

namespace Mafmax.FileConverter.Utils.Helpers;
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTime.UtcNow;
}
