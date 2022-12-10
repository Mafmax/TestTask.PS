using AutoMapper;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Responses;
using Mafmax.FileConverter.DataAccess.Models;
using Mafmax.FileConverter.DataAccess.Repositories.FilesRepository.Responses;

namespace Mafmax.FileConverter.BusinessLogic.MappingProfiles;
public class FilesServiceProfile : Profile
{
    public FilesServiceProfile()
    {
        CreateMap<UploadFileRequest, FilePointerModel>()
            .ForMember(dest => dest.Name, x => x.MapFrom(src => src.Name));

        CreateMap<WriteFileResponse, UploadFileResponse>()
            .ForMember(dest => dest.FileId, x => x.MapFrom(src => src.FilePointerId));

        CreateMap<ReadFileResponse, DownloadFileResponse>()
            .ForMember(dest => dest.Name, x => x.MapFrom(src => src.FilePointer.Name))
            .ForMember(dest => dest.Content, x => x.MapFrom(src => src.Stream));
    }
}
