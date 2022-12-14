namespace Mafmax.FileConverter.BusinessLogic.Services.DocConverter.Abstractions;

/// <summary>
/// Defines methods to convert documents.
/// </summary>
public interface IDocConverter
{
    /// <summary>
    /// Convert pdf document from HTML content string.
    /// </summary>
    /// <param name="html">Content of input file in HTML format.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Stream with pdf file content.</returns>
    public Task<Stream> ConvertToPdfAsync(string html, CancellationToken cancellationToken = default);
}