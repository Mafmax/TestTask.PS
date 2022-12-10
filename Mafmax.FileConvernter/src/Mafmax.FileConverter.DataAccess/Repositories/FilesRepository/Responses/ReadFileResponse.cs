using Mafmax.FileConverter.DataAccess.Models;

namespace Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Responses;

/// <summary>
/// Represents a response to read file.
/// </summary>
/// <param name="FilePointer">Pointer to file info.</param>
/// <param name="Stream">Stream to read file content.</param>
public record ReadFileResponse(FilePointerModel FilePointer, Stream Stream) : IDisposable, IAsyncDisposable
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