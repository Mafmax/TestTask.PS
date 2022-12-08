using FluentValidation;
using FluentValidation.Validators;

namespace Mafmax.FileConverter.Utils.Validators;
public class FileExtensionPropertyValidator<T> : PropertyValidator<T, string>
{
    private readonly string[] _extensions;

    public FileExtensionPropertyValidator(params string[] extensions)
    {
        _extensions = extensions;
    }

    /// <inheritdoc />
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var extension = Path.GetExtension(value);

        var modifiedExtensions = _extensions.Select(x => '.' + x);

        if (modifiedExtensions.Contains(extension)) return true;

        context.AddFailure(context.PropertyName,
            $"File extension must be one of follow: {string.Join(",", _extensions)}.");

        return false;
    }

    /// <inheritdoc />
    public override string Name { get; } = nameof(FileExtensionPropertyValidator<T>);
}
