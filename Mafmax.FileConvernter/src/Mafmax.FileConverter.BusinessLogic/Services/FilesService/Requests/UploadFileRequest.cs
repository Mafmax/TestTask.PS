namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;

/// <summary>
/// Represents a request for file uploading operation;
/// </summary>
public record UploadFileRequest
{
    /// <summary>
    /// Uploaded file name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Collection of file content parts.
    /// </summary>
    public IAsyncEnumerable<ReadOnlyMemory<byte>> PartitionFileContent { get; set; } = null!;
}