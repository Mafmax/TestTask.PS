using Mafmax.FileConverter.DataAccess.Extensions;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Abstractions;
using Mafmax.FileConverter.SharedConfiguration.DependencyInjection;
using Mafmax.FileConverter.SharedConfiguration.Options;
using Mafmax.FileConverter.Utils.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Mafmax.FileConverter.DataAccess.DependencyInjection;

/// <summary>
/// Contains extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures data access layer services. Central configuration of entire current project.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <returns>Service collection with configured services to allow method chaining.</returns>
    public static IServiceCollection SetupDataAccessLayer(this IServiceCollection services) =>
        services
            .AddScoped<IFilesRepository, FilesRepository>()
            .SetupUtilities()
            .SetupSharedConfiguration()
            .SetupMongoDb();

    private static IServiceCollection SetupMongoDb(this IServiceCollection services) =>
        services
            .AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var mongoClient = new MongoClient(options.ConnectionString);

                return mongoClient
                    .GetDatabase(options.DatabaseName)
                    .SetUpDatabase(options);
            });
}
