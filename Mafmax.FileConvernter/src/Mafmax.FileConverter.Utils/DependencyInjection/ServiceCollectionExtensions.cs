using Mafmax.FileConverter.Utils.Helpers;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Mafmax.FileConverter.Utils.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection SetupUtilities(this IServiceCollection services) =>
        services
            .AddSingleton<IThrowHelper, ThrowHelper>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<IRegexHelper, RegexHelper>();
}
