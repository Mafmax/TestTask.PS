using PuppeteerSharp;

namespace Mafmax.FileConverter.BusinessLogic.Services.DocConverter.Abstractions;

public interface IDocConverter
{
    public Task<Stream> ConvertToPdfAsync(string html, PdfOptions options, CancellationToken cancellationToken = default);
}