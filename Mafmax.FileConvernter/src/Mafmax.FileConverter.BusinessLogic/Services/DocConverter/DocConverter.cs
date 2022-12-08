using Mafmax.FileConverter.BusinessLogic.Services.DocConverter.Abstractions;
using PuppeteerSharp;

namespace Mafmax.FileConverter.BusinessLogic.Services.DocConverter;

public class DocConverter : IDocConverter
{
    private readonly Func<Task<IBrowser>> _browserFactory;

    public DocConverter(Func<Task<IBrowser>> browserFactory)
    {
        _browserFactory = browserFactory;
    }

    /// <inheritdoc />
    public async Task<Stream> ConvertToPdfAsync(string html, PdfOptions options, CancellationToken cancellationToken = default)
    {
        await using var browser = await _browserFactory();
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        var stream = await page.PdfStreamAsync(options);

        return stream;
    }
}