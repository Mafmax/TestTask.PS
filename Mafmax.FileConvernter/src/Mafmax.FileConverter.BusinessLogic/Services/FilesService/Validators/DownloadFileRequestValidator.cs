using FluentValidation;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;
using Mafmax.FileConverter.Utils.Validators;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Validators;

/// <summary>
/// Validator for checking properties of <see cref="DownloadFileRequest"/>.
/// </summary>
public class DownloadFileRequestValidator : AbstractValidator<DownloadFileRequest>
{
    /// <summary>
    /// Used to set validation rules.
    /// </summary>
    public DownloadFileRequestValidator(IRegexHelper regexHelper)
    {
        var fileIdValidator = new FileIdPropertyValidator<DownloadFileRequest>(regexHelper.FileIdRegex);

        RuleFor(x => x.FileId)
            .SetValidator(fileIdValidator);
    }
}
