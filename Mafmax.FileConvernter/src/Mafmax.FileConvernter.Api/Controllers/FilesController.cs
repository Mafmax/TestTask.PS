using FluentValidation;
using Mafmax.FileConvernter.Api.Controllers.Abstracrions;
using Mafmax.FileConvernter.Api.Extensions;
using Mafmax.FileConvernter.Api.Filters;
using Mafmax.FileConvernter.Api.Filters.OpenApiFilters;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;
using Mafmax.FileConverter.SharedConfiguration.Options;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace Mafmax.FileConvernter.Api.Controllers;

/// <summary>
/// Controller for process requests binded with files.
/// </summary>
public class FilesController : ApplicationControllerBase
{
    private readonly IFilesService _filesService;

    /// <summary>
    /// Creates an instance of <see cref="FilesController"/>.
    /// </summary>
    public FilesController(IFilesService filesService)
    {
        _filesService = filesService;
    }

    /// <summary>
    /// Uploads file as multipart form data.
    /// </summary>
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

        if (requestValidator.ModelIsInvalid(request, out var error)) 
            return error;

        return await _filesService.UploadFileAsync(request, cancellationToken);
    }

    /// <summary>
    /// Downloads file.
    /// </summary>
    [HttpGet("download/{FileId}")]
    public async Task<ActionResult<DownloadFileResponse>> DownloadFileAsync(
       [FromRoute] DownloadFileRequest request,
       [FromServices] IValidator<DownloadFileRequest> requestValidator,
       CancellationToken cancellationToken = default)
    {
        if (requestValidator.ModelIsInvalid(request, out var error))
            return error;

        var (name, content) = await _filesService.DownloadFileAsync(request, cancellationToken);

        if (new FileExtensionContentTypeProvider().TryGetContentType(name, out var contentType))
        {
            return File(content, contentType, name, enableRangeProcessing: true);
        }

        throw new InvalidOperationException($"Couldn't recognize MIME type of file content. File name was {name}");
    }

    /// <summary>
    /// Converts file.
    /// </summary>
    [HttpGet("convert/{FileId}")]
    public async Task<ActionResult<ConvertFileResponse>> ConvertFileAsync(
        [FromRoute] ConvertFileRequest request,
        [FromServices] IValidator<ConvertFileRequest> requestValidator,
        CancellationToken cancellationToken = default)
    {
        if (requestValidator.ModelIsInvalid(request, out var error))
            return error;

        return await _filesService.ConvertFileAsync(request, cancellationToken);
    }

    /// <summary>
    /// Gets file name.
    /// </summary>
    [HttpGet("name/{FileId}")]
    public async Task<ActionResult<GetFileNameResponse>> GetFileNameAsync(
        [FromRoute] GetFileNameRequest request,
        [FromServices] IValidator<GetFileNameRequest> requestValidator,
        CancellationToken cancellationToken = default)
    {
        if(requestValidator.ModelIsInvalid(request, out var error))
            return error;

        return await _filesService.GetFileNameAsync(request, cancellationToken);
    }
}
