namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;

/// <summary>
/// Represents a response for file downloading operation.
/// </summary>
/// <param name="Name">File name.</param>
/// <param name="Content">File content.</param>
public record DownloadFileResponse(string Name, Stream Content)
{
    /// <summary>
    /// Used for AutoMapper.
    /// </summary>
    protected DownloadFileResponse() : this(string.Empty, Stream.Null) { }
}
