using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Abstractions;
using Mafmax.FileConverter.SharedConfiguration.Options;
using Microsoft.Extensions.Options;

namespace Mafmax.FileConverter.BackgroundServices;
public class OldFilesRemover : BackgroundService
{
    private readonly ILogger<OldFilesRemover> _logger;
    private readonly IFilesRepository _filesRepository;
    private readonly TimeSpan _maxFileAge;
    public OldFilesRemover(ILogger<OldFilesRemover> logger,
        IFilesRepository filesRepository,
        IOptions<ApplicationSettings> appSettings)
    {
        _logger = logger;
        _filesRepository = filesRepository;
        _maxFileAge = appSettings.Value.FilesAgeLimit;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            _logger.LogInformation("Start removing old files.");
            var filesRemovedCount =
                await _filesRepository.RemoveOldFilesAsync(_maxFileAge, stoppingToken);
            _logger.LogInformation("Removed {0} old files.", filesRemovedCount);

            await Task.Delay(_maxFileAge, stoppingToken);
        }
    }
}
