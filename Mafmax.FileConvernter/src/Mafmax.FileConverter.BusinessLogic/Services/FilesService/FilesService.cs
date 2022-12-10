using AutoMapper;
using HtmlAgilityPack;
using Mafmax.FileConverter.BusinessLogic.Services.DocConverter.Abstractions;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;
using Mafmax.FileConverter.DataAccess.Models;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Abstractions;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Responses;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService;

/// <summary>
/// Servise to work with files.
/// </summary>
public class FilesService : IFilesService
{
    private readonly IDocConverter _docConverter;
    private readonly IFilesRepository _filesRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of <see cref="FilesService"/>.
    /// </summary>
    public FilesService(IDocConverter docConverter,
        IFilesRepository filesRepository,
        IMapper mapper)
    {
        _docConverter = docConverter;
        _filesRepository = filesRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<UploadFileResponse> UploadFileAsync(UploadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var file = _mapper.Map<FilePointerModel>(request);
        await using var writeFileResponse = await _filesRepository.OpenFileToSaveAsync(file, cancellationToken);
        var fileContent = request.PartitionFileContent
            .WithCancellation(cancellationToken);

        await foreach (var filePart in fileContent)
        {
            await writeFileResponse.Stream.WriteAsync(filePart, cancellationToken);
        }

        var response = _mapper.Map<UploadFileResponse>(writeFileResponse);

        return response;
    }

    /// <inheritdoc />
    public async Task<ConvertFileResponse> ConvertFileAsync(ConvertFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var fileToConvert = await _filesRepository.OpenFileToReadAsync(request.FileId, cancellationToken);
        var fileContent = await ConvertFileAsync(fileToConvert, cancellationToken);
        var fileId = await _filesRepository.SaveFileAsync(fileContent, cancellationToken);
       await _filesRepository.DeleteFileAsync(fileToConvert.FilePointer.Id, cancellationToken);

        return new ConvertFileResponse(fileId);
    }

    /// <inheritdoc />
    public async Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var file = await _filesRepository.OpenFileToReadAsync(request.FileId, cancellationToken);
        var response = _mapper.Map<DownloadFileResponse>(file);

        return response;
    }

    /// <inheritdoc />
    public async Task<int> RemoveOldFilesAsync(TimeSpan fileAgeLimit, CancellationToken cancellationToken = default) => 
        await _filesRepository.RemoveOldFilesAsync(fileAgeLimit, cancellationToken);

    /// <inheritdoc />
    public async Task<GetFileNameResponse> GetFileNameAsync(GetFileNameRequest request, CancellationToken cancellationToken)
    {
        var fileName = await _filesRepository.GetFileNameAsync(request.FileId, cancellationToken);

        return new GetFileNameResponse(fileName);
    }

    private static string GetHtmlFromStream(Stream stream)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.Load(stream);

        return htmlDoc.DocumentNode.InnerHtml;
    }

    private async Task<ReadFileResponse> ConvertFileAsync(ReadFileResponse fileToConvert,
        CancellationToken cancellationToken = default)
    {
        var fileName =  Path.ChangeExtension(fileToConvert.FilePointer.Name, "pdf");
        var fileModel = new FilePointerModel(fileName);
        var fileContent = await ConvertFileContentAsync(fileToConvert.Stream, cancellationToken);
        var file = new ReadFileResponse(fileModel, fileContent);

        return file;
    }

    private async Task<Stream> ConvertFileContentAsync(Stream fileToConvert, CancellationToken cancellationToken = default)
    {
        var html = GetHtmlFromStream(fileToConvert);
       
        var file = await _docConverter
            .ConvertToPdfAsync(html, cancellationToken);

        return file;
    }
}
