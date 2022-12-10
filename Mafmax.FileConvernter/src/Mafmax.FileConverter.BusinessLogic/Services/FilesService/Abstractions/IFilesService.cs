using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;

/// <summary>
/// Defines methods to work with files.
/// </summary>
public interface IFilesService
{
    /// <summary>
    /// Uploads file.
    /// </summary>
    Task<UploadFileResponse> UploadFileAsync(UploadFileRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Converts file.
    /// </summary>
    Task<ConvertFileResponse> ConvertFileAsync(ConvertFileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads file.
    /// </summary>
    Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets file name.
    /// </summary>
    Task<GetFileNameResponse> GetFileNameAsync(GetFileNameRequest request, CancellationToken cancellationToken);
}