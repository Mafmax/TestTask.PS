using Mafmax.FileConverter.SharedConfiguration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mafmax.FileConverter.SharedConfiguration.DependencyInjection;

/// <summary>
/// Contains extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.shared.json")
        .Build();

    /// <summary>
    /// Configures configuration models used anywhere.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <returns>Service collection with configured services to allow method chaining.</returns>
    public static IServiceCollection SetupSharedConfiguration(this IServiceCollection services) =>
        services
            .Configure<ApplicationSettings>(Configuration.GetSection(nameof(ApplicationSettings)))
            .Configure<MongoDbSettings>(Configuration.GetSection(nameof(MongoDbSettings)));
}
