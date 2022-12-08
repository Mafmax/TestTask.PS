using Mafmax.FileConverter.DataAccess.Configuration;
using Mafmax.FileConverter.DataAccess.Extensions;
using Mafmax.FileConverter.DataAccess.Repositories;
using Mafmax.FileConverter.DataAccess.Repositories.Abstractions;
using Mafmax.FileConverter.Utils.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Mafmax.FileConverter.DataAccess.DependencyInjection;
public static class ServiceCollectionExtensions
{

    public static IServiceCollection SetupDataAccessLayer(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IFilesRepository, FilesRepository>()
            .SetupUtilities()
            .SetupMongoDb(configuration);

    private static IServiceCollection SetupMongoDb(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)))
            .AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var mongoClient = new MongoClient(options.ConnectionString);

                return mongoClient
                    .GetDatabase(options.DatabaseName)
                    .SetUpDatabase(options);
            });
}
