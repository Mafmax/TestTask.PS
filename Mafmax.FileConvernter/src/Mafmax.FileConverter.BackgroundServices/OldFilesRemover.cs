using Mafmax.FileConverter.BackgroundServices.Settings;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;
using Microsoft.Extensions.Options;

namespace Mafmax.FileConverter.BackgroundServices;
public class OldFilesRemover : BackgroundService
{
    private readonly ILogger<OldFilesRemover> _logger;
    private readonly IFilesService _filesService;
    private readonly TimeSpan _maxFileAge;
    public OldFilesRemover(ILogger<OldFilesRemover> logger,
        IFilesService filesService, IOptions<ApplicationSettings> appSettings)
    {
        _logger = logger;
        _filesService = filesService;
        _maxFileAge = appSettings.Value.FileAgeLimit;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            _logger.LogInformation("Start removing old files.");
            var filesRemovedCount =
                await _filesService.RemoveOldFilesAsync(_maxFileAge, stoppingToken);
            _logger.LogInformation("Removed {0} old files.", filesRemovedCount);

            await Task.Delay(_maxFileAge, stoppingToken);
        }
    }
}
