using FluentValidation;
using Mafmax.FileConverter.BusinessLogic.Services.FilesService.Requests;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;
using Mafmax.FileConverter.Utils.Validators;

namespace Mafmax.FileConverter.BusinessLogic.Services.FilesService.Validators;
public class GetFileNameRequestValidator : AbstractValidator<GetFileNameRequest>
{
    public GetFileNameRequestValidator(IRegexHelper regexHelper)
    {
        var fileIdValidator = new FileIdPropertyValidator<GetFileNameRequest>(regexHelper.FileIdRegex);

        RuleFor(x => x.FileId)
            .SetValidator(fileIdValidator);
    }
}
