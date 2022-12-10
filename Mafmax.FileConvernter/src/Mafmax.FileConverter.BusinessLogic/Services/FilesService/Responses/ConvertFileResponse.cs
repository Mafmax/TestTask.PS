namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;

/// <summary>
/// Represents a response for file converting operation.
/// </summary>
/// <param name="FileId">Id of file after convertion.</param>
public record ConvertFileResponse(string FileId);
