using FluentValidation;
using Mafmax.FileConverter.BusinessLogic.Services.DocConverter;
using Mafmax.FileConverter.BusinessLogic.Services.DocConverter.Abstractions;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;
using Mafmax.FileConverter.DataAccess.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;

namespace Mafmax.FileConverter.BusinessLogic.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private static bool _isChromiumDownloaded = false;

    public static IServiceCollection SetupBusinessLayer(this IServiceCollection services, IConfiguration configuration) =>
        services
            .SetupDataAccessLayer(configuration)
            .AddScoped<IDocConverter, DocConverter>()
            .AddScoped<IFilesService, FilesService>()
            .AddAutoMapper(cfg => cfg.AddMaps(typeof(BusinessLayerAssemblyMarker).Assembly))
            .AddValidatorsFromAssembly(typeof(BusinessLayerAssemblyMarker).Assembly)
            .SetupPuppeteer();

    private static IServiceCollection SetupPuppeteer(this IServiceCollection services) =>
        services.AddScoped<Func<Task<IBrowser>>>(_ => async () =>
        {
            if (_isChromiumDownloaded is false)
            {
                await DownloadChromiumAsync(Environment.OSVersion.Platform);

                _isChromiumDownloaded = true;
            }

            var launchOptions = new LaunchOptions()
            {
                Headless = true,
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-gpu",
                    "--disable-setuid-sandbox",
                    "--no-first-run",
                    "--no-zygote",
                    "--single-process"
                }
            };

            return await Puppeteer.LaunchAsync(launchOptions);
        });

    private static async Task DownloadChromiumAsync(PlatformID platform)
    {
        switch (platform)
        {
            case not PlatformID.Unix and not PlatformID.MacOSX:
                var fetcher = new BrowserFetcher();
                await fetcher.DownloadAsync();
                return;
            default:
                return;
        }
    }
}