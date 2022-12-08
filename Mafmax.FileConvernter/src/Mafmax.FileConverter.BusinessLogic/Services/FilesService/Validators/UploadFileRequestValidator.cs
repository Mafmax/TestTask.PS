using FluentValidation;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.Utils.Validators;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Validators;

public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequestValidator()
    {
        var fileExtensionValidator = new FileExtensionPropertyValidator<UploadFileRequest>("html");
        RuleFor(x => x.Name)
            .SetValidator(fileExtensionValidator)
            .WithMessage("Invalid upload file model.");
    }
}
