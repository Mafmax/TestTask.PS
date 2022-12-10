namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;

/// <summary>
/// Represents a request to downloading file operation.
/// </summary>
/// <param name="FileId">File to download id.</param>
public record DownloadFileRequest(string FileId);
