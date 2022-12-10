namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;

public record UploadFileResponse
{
    public string FileId { get; init; } = string.Empty;
}
