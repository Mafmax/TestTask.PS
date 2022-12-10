namespace Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Responses;

/// <summary>
/// Represents a response to write file.
/// </summary>
/// <param name="FilePointerId">Writed file pointer id.</param>
/// <param name="Stream">Stream to write file.</param>
public record WriteFileResponse(string FilePointerId, Stream Stream) : IDisposable, IAsyncDisposable
{
    /// <inheritdoc />
    public void Dispose()
    {
        Stream.Dispose();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
}
