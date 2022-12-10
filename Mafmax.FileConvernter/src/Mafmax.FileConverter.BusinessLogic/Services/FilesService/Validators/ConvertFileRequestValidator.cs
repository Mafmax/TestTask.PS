using FluentValidation;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;
using Mafmax.FileConverter.Utils.Validators;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Validators;

/// <summary>
/// Validator for checking properties of <see cref="ConvertFileRequest"/>.
/// </summary>
public class ConvertFileRequestValidator : AbstractValidator<ConvertFileRequest>
{
    /// <summary>
    /// Used to set validation rules.
    /// </summary>
    public ConvertFileRequestValidator(IRegexHelper regexHelper)
    {
        var fileIdValidator = new FileIdPropertyValidator<ConvertFileRequest>(regexHelper.FileIdRegex);
        RuleFor(x => x.FileId)
            .SetValidator(fileIdValidator);
    }
}