using Mafmax.FileConverter.DataAccess.Models;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Responses;

namespace Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Abstractions;

/// <summary>
/// Defines methods to work with file storage.
/// </summary>
public interface IFilesRepository
{
    /// <summary>
    /// Opens file to write content.
    /// </summary>
    /// <param name="filePointer">File model to save.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// An object that allows writing to storage.
    /// Use <see cref="Stream"/> to write content.
    /// </returns>
    Task<WriteFileResponse> OpenFileToSaveAsync(FilePointerModel filePointer, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves file to storage.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Saved file pointer id.</returns>
    Task<string> SaveFileAsync(ReadFileResponse file, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes expired files.
    /// </summary>
    /// <param name="fileAgeLimit">Max age for filter old files.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Count of removed files.</returns>
    Task<int> RemoveOldFilesAsync(TimeSpan fileAgeLimit, CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens file to read content.
    /// </summary>
    /// <param name="filePointerId">File pointer id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// An object that allows reading from storage.
    /// Use <see cref="ReadFileResponse.Stream"/> to read content.
    /// </returns>
    Task<ReadFileResponse> OpenFileToReadAsync(string filePointerId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes file pointer and binded file.
    /// </summary>
    /// <param name="filePointerId">File pointer id.</param>
    /// <param name="cancellationToken"></param>
    Task DeleteFileAsync(string filePointerId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets name of file.
    /// </summary>
    /// <param name="filePointerId">File pointer id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>File name.</returns>
    Task<string> GetFileNameAsync(string filePointerId, CancellationToken cancellationToken);
}
