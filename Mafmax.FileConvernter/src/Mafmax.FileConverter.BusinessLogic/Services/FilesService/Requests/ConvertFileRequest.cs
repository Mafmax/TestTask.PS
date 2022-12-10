namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;

/// <summary>
/// Represents a request to converting file operation.
/// </summary>
/// <param name="FileId">File to convert id.</param>
public record ConvertFileRequest(string FileId);
