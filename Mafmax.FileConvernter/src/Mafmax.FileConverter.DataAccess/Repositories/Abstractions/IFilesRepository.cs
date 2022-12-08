using Mafmax.FileConverter.DataAccess.Models;
using Mafmax.FileConverter.DataAccess.Responses;

namespace Mafmax.FileConverter.DataAccess.Repositories.Abstractions;
public interface IFilesRepository
{
    Task<WriteFileResponse> SaveFileAsync(FileModel file, CancellationToken cancellationToken = default);
    Task<string> SaveFileAsync(ReadFileResponse fileToSave, CancellationToken cancellationToken = default);
    Task<int> RemoveOldFilesAsync(TimeSpan fileAgeLimit, CancellationToken cancellationToken = default);
    Task<ReadFileResponse> ReadFileAsync(string fileId, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileId, CancellationToken cancellationToken = default);
    Task<string> GetFileNameAsync(string fileId, CancellationToken cancellationToken);
}
