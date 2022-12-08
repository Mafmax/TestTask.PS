using FluentValidation;
using FluentValidation.AspNetCore;
using Mafmax.FileConvernter.Api.Controllers.Abstracrions;
using Mafmax.FileConvernter.Api.Filters;
using Mafmax.FileConvernter.Api.Filters.OpenApiFilters;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;
using Mafmax.FileConverter.DataAccess.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace Mafmax.FileConvernter.Api.Controllers;
public class FilesController : ApplicationControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController(IFilesService filesService)
    {
        _filesService = filesService;
    }

    // TODO mafmax: Delete
    [HttpGet("variables")]
    public ActionResult<object> GetEnvAsync([FromServices] IOptions<MongoDbSettings> settings)
    {
        var result = new
        {
            MongoDbConnectionString = Environment.GetEnvironmentVariable("MongoDbSettings__ConnectionString"),
            AppsettingsSection = settings.Value.ConnectionString
        };

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestValidator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [DisableFormValueModelBinding]
    [RequiresMultipartFile]
    [HttpPost("upload")]
    [EnableCors]
    public async Task<ActionResult<UploadFileResponse>> UploadFileAsync(
        [FromServices] IValidator<UploadFileRequest> requestValidator,
        CancellationToken cancellationToken = default)
    {
        var request = new UploadFileRequest();

        await LoadFileAsync(request,
            (model, c) => model.PartitionFileContent = c,
            cancellationToken);

        var validationResult = await requestValidator
            .ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await _filesService.UploadFileAsync(request, cancellationToken);
        }

        validationResult.AddToModelState(ModelState);

        return BadRequest(ModelState);

    }

    [HttpGet("download/{FileId}")]
    public async Task<ActionResult<DownloadFileResponse>> DownloadFileAsync(
       [FromRoute] DownloadFileRequest request,
       [FromServices] IValidator<DownloadFileRequest> requestValidator,
       CancellationToken cancellationToken = default)
    {
        var validationResult = await requestValidator
            .ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);

            return BadRequest(ModelState);
        }

        var (name, content) = await _filesService.DownloadFileAsync(request, cancellationToken);

        if (new FileExtensionContentTypeProvider().TryGetContentType(name, out var contentType))
        {
            return File(content, contentType, name, enableRangeProcessing: true);
        }

        throw new InvalidOperationException($"Couldn't recognize MIME type of file content. File name was {name}");
    }

    // ReSharper disable once RouteTemplates.RouteParameterIsNotPassedToMethod
    [HttpGet("convert/{FileId}")]
    public async Task<ActionResult<ConvertFileResponse>> ConvertFileAsync(
        [FromRoute] ConvertFileRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _filesService.ConvertFileAsync(request, cancellationToken);
    }

    [HttpGet("name/{FileId}")]
    public async Task<ActionResult<GetFileNameResponse>> GetFileNameAsync(
        [FromRoute] GetFileNameRequest request,
        CancellationToken cancellationToken = default) =>
        await _filesService.GetFileNameAsync(request, cancellationToken);
}
