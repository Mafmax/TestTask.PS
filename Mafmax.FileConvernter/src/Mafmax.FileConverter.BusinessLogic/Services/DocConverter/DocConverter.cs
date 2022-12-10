using Mafmax.FileConverter.BusinessLogic.Services.DocConverter.Abstractions;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Mafmax.FileConverter.BusinessLogic.Services.DocConverter;

/// <summary>
/// Class used for documents conversion.
/// </summary>
public class DocConverter : IDocConverter
{
    private readonly Func<Task<IBrowser>> _browserFactory;
    private static readonly PdfOptions PdfOptions = new() { Format = PaperFormat.A4 };

    /// <summary>
    /// Creates an instance of <see cref="DocConverter"/>.
    /// </summary>
    /// <param name="browserFactory">Factory for getting browser.</param>
    public DocConverter(Func<Task<IBrowser>> browserFactory)
    {
        _browserFactory = browserFactory;
    }

    /// <inheritdoc />
    public async Task<Stream> ConvertToPdfAsync(string html,  CancellationToken cancellationToken = default)
    {
        await using var browser = await _browserFactory();
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        var stream = await page.PdfStreamAsync(PdfOptions);

        return stream;
    }
}