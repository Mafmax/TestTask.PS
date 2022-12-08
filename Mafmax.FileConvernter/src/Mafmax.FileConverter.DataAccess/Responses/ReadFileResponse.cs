using Mafmax.FileConverter.DataAccess.Models;

namespace Mafmax.FileConverter.DataAccess.Responses;

public record ReadFileResponse(FileModel File, Stream StreamToReadFile);