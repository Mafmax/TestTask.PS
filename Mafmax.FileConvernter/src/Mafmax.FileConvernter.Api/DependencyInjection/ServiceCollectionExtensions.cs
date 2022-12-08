using FluentValidation.AspNetCore;
using Mafmax.FileConvernter.Api.Filters.OpenApiFilters;
using Mafmax.FileConvernter.Api.Middlewares;
using Mafmax.FileConvernter.Api.Settings;
using Mafmax.FileConverter.BusinessLogic.DependencyInjection;

namespace Mafmax.FileConvernter.Api.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration) =>
        services.AddControllers().Services
            .AddFluentValidationAutoValidation()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(s => s.OperationFilter<RequiresMultipartFileOperationFilter>())
            .SetupBusinessLayer(configuration)
            .AddScoped<ExceptionHandlingMiddleware>()
            .Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)))
            .AddCors(cfg => cfg.AddPolicy("allow_all", b =>
            {
                b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));
}