using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace Mafmax.FileConverter.Utils.Validators;

/// <summary>
/// Represents validator for FileId property.
/// </summary>
public class FileIdPropertyValidator<T> : PropertyValidator<T, string>
{
    private readonly Regex _fileIdRegex;

    /// <summary>
    /// Creates an instance of <see cref="FileIdPropertyValidator{T}"/>.
    /// </summary>
    /// <param name="fileIdRegex">Regular expression for allowed file ids.</param>
    public FileIdPropertyValidator(Regex fileIdRegex)
    {
        _fileIdRegex = fileIdRegex;
    }

    /// <inheritdoc />
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (_fileIdRegex.IsMatch(value.Trim(' ', '\"'))) return true;

        var errorMessage = string.Concat(
            "Invalid file Id format. ",
            "File Id should be consists of 24 digits or [a-f] symbols only. ",
            $"File Id was {value}.");

        context.AddFailure(context.PropertyName, errorMessage);

        return false;

    }

    /// <inheritdoc />
    public override string Name { get; } = nameof(FileIdPropertyValidator<T>);
}
