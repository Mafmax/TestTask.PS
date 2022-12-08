using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace Mafmax.FileConverter.Utils.Validators;
public class FileIdPropertyValidator<T> : PropertyValidator<T, string>
{
    private readonly Regex _fileIdRegex;

    public FileIdPropertyValidator(Regex fileIdRegex)
    {
        _fileIdRegex = fileIdRegex;
    }

    /// <inheritdoc />
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (_fileIdRegex.IsMatch(value.Trim(' ','\"'))) return true;

        var errorMessage = "Invalid file Id format. " +
                           "File Id should be consists of 24 digits or [a-f] symbols only. " +
                           $"File Id was {value}.";

        context.AddFailure(context.PropertyName, errorMessage);

        return false;

    }

    /// <inheritdoc />
    public override string Name { get; } = nameof(FileIdPropertyValidator<T>);
}
