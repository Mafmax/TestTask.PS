using Mafmax.FileConverter.Utils.Helpers;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Mafmax.FileConverter.Utils.DependencyInjection;

/// <summary>
/// Contains extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures utilities. Central configuration method for the entire current project.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Service collection with configured services to allow method chaining.</returns>
    public static IServiceCollection SetupUtilities(this IServiceCollection services) =>
        services
            .AddSingleton<IThrowHelper, ThrowHelper>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<IRegexHelper, RegexHelper>();

}
