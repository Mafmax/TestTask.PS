using Mafmax.FileConvernter.Api.Settings;
using Mafmax.FileConverter.Utils.Exceptions;
using Microsoft.Extensions.Options;

namespace Mafmax.FileConvernter.Api.Middlewares;
public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IOptions<ApplicationSettings> _appSettings;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IOptions<ApplicationSettings> appSettings)
    {
        _logger = logger;
        _appSettings = appSettings;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (AppException applicationException)
        {
            await ProcessAppException(context, applicationException);
        }
        catch (Exception exception)
        {
            await ProcessException(context, exception);
        }
    }

    private async Task ProcessException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var message = _appSettings.Value.DetailedError
             ? exception.Message + exception.StackTrace
             : "Something went wrong. Please, try again.";

        await WriteError(context, message);

        _logger.LogCritical(exception, "An unhandled exception was occure.");
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private static async Task ProcessAppException(HttpContext context, AppException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        context.Response.ContentType = "text/plain";

        await WriteError(context, exception.Message);
    }

    private static async Task WriteError(HttpContext context, string message)
    {
        await context.Response.WriteAsJsonAsync(new { Error = message });
    }
}
