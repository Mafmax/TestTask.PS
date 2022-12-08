using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;

public interface IFilesService
{
    Task<UploadFileResponse> UploadFileAsync(UploadFileRequest request, CancellationToken cancellationToken = default);
    Task<ConvertFileResponse> ConvertFileAsync(ConvertFileRequest request, CancellationToken cancellationToken = default);
    Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request, CancellationToken cancellationToken = default);
    Task<int> RemoveOldFilesAsync(TimeSpan fileAgeLimit, CancellationToken cancellationToken = default);
    Task<GetFileNameResponse> GetFileNameAsync(GetFileNameRequest request, CancellationToken cancellationToken);
}