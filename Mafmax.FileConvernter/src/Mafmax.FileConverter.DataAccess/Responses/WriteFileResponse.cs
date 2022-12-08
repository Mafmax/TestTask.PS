namespace Mafmax.FileConverter.DataAccess.Responses;

public record WriteFileResponse(string FileId, Stream StreamToWriteFile);
