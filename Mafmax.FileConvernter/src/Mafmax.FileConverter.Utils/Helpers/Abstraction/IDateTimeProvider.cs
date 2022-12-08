namespace Mafmax.FileConverter.Utils.Helpers.Abstraction;
public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
