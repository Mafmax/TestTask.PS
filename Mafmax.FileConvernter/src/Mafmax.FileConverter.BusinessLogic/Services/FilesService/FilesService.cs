﻿using AutoMapper;
using HtmlAgilityPack;
using Mafmax.FileConverter.BusinessLogic.Services.DocConverter.Abstractions;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Abstractions;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;
using Mafmax.FileConverter.DataAccess.Models;
using Mafmax.FileConverter.DataAccess.Repositories.Abstractions;
using Mafmax.FileConverter.DataAccess.Responses;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService;

public class FilesService : IFilesService
{
    private readonly IDocConverter _docConverter;
    private readonly IFilesRepository _filesRepository;
    private readonly IMapper _mapper;

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
        var file = _mapper.Map<FileModel>(request);
        var writeFileResponse = await _filesRepository.SaveFileAsync(file, cancellationToken);
        await using var fileStream = writeFileResponse.StreamToWriteFile;
        var fileContent = request.PartitionFileContent
            .WithCancellation(cancellationToken);

        await foreach (var filePart in fileContent)
        {
            await fileStream.WriteAsync(filePart, cancellationToken);
        }

        var response = _mapper.Map<UploadFileResponse>(writeFileResponse);

        return response;
    }

    /// <inheritdoc />
    public async Task<ConvertFileResponse> ConvertFileAsync(ConvertFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var fileToConvert = await _filesRepository.ReadFileAsync(request.FileId, cancellationToken);
        var fileContent = await ConvertFileAsync(fileToConvert, cancellationToken);
        var fileId = await _filesRepository.SaveFileAsync(fileContent, cancellationToken);
        await _filesRepository.DeleteFileAsync(fileToConvert.File.Id, cancellationToken);

        return new ConvertFileResponse(fileId);
    }

    /// <inheritdoc />
    public async Task<DownloadFileResponse> DownloadFileAsync(DownloadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var file = await _filesRepository.ReadFileAsync(request.FileId, cancellationToken);
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
        var fileName =  Path.ChangeExtension(fileToConvert.File.Name, "pdf");
        var fileModel = new FileModel { Name = fileName };
        var fileContent = await ConvertFileContentAsync(fileToConvert.StreamToReadFile, cancellationToken);
        var file = new ReadFileResponse(fileModel, fileContent);

        return file;
    }

    private async Task<Stream> ConvertFileContentAsync(Stream fileToConvert, CancellationToken cancellationToken = default)
    {
        var html = GetHtmlFromStream(fileToConvert);
        var pdfOptions = new PdfOptions { Format = PaperFormat.A4 };
        var file = await _docConverter
            .ConvertToPdfAsync(html, pdfOptions, cancellationToken);

        return file;
    }
}