namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;

public record UploadFileRequest
{
    public string Name { get; set; } = string.Empty;
    public IAsyncEnumerable<ReadOnlyMemory<byte>> PartitionFileContent { get; set; } = null!;
}