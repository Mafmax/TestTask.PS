namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;

/// <summary>
/// Represents a response for file uploading operation.
/// </summary>
/// <param name="FileId">Uploading file id.</param>
public record UploadFileResponse(string FileId)
{
    /// <summary>
    /// Used for AutoMapper.
    /// </summary>
    protected UploadFileResponse():this(string.Empty){}
}
