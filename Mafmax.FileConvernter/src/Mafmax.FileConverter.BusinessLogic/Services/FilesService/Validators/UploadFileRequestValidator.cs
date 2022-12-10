using FluentValidation;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.Utils.Validators;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Validators;

/// <summary>
/// Validator for checking properties of <see cref="UploadFileRequest"/>.
/// </summary>
public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    /// <summary>
    /// Used to set validation rules.
    /// </summary>
    public UploadFileRequestValidator()
    {
        var fileExtensionValidator = new FileExtensionPropertyValidator<UploadFileRequest>(".html");
        RuleFor(x => x.Name)
            .SetValidator(fileExtensionValidator)
            .WithMessage("Invalid upload file model.");
    }
}
