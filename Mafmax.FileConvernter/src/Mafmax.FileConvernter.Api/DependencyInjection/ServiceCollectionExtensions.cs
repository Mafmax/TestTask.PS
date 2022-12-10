using FluentValidation.AspNetCore;
using Mafmax.FileConvernter.Api.Filters.OpenApiFilters;
using Mafmax.FileConvernter.Api.Middlewares;
using Mafmax.FileConverter.BusinessLogic.DependencyInjection;

namespace Mafmax.FileConvernter.Api.DependencyInjection;

/// <summary>
/// Contains extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Configures API. Central configuration for entire current project.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <param name="configuration">Configuration.</param>
    /// <returns>Collection of configured services to allow method chaining.</returns>
    public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration) =>
        services.AddControllers().Services
            .AddFluentValidationAutoValidation()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(s => s.OperationFilter<RequiresMultipartFileOperationFilter>())
            .SetupBusinessLayer(configuration)
            .AddScoped<ExceptionHandlingMiddleware>()
            .AddCors(cfg => cfg.AddPolicy("allow_all", b =>
            {
                b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));
}