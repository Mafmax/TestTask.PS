namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;

public record DownloadFileResponse(string Name, Stream Content)
{
    protected DownloadFileResponse() : this(string.Empty, Stream.Null) { }
}
