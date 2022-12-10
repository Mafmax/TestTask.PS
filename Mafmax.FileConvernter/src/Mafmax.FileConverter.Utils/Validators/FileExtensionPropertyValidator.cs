using FluentValidation;
using FluentValidation.Validators;

namespace Mafmax.FileConverter.Utils.Validators;

/// <summary>
/// Represents validator for FileExtension property.
/// </summary>
public class FileExtensionPropertyValidator<T> : PropertyValidator<T, string>
{
    private readonly HashSet<string> _extensions;

    /// <inheritdoc />
    public override string Name { get; } = nameof(FileExtensionPropertyValidator<T>);

    /// <summary>
    /// Creates an instance of <see cref="FileExtensionPropertyValidator{T}"/>.
    /// </summary>
    /// <param name="extensions">Array of extension strings.
    /// Note that extensions should contain starting point. Use ".pdf" instead of "pdf".</param>
    public FileExtensionPropertyValidator(params string[] extensions)
    {
        ValidateInputExtensions(extensions);
        _extensions = extensions.ToHashSet();
    }

    /// <inheritdoc />
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var extension = Path.GetExtension(value);

        if (_extensions.Contains(extension)) return true;

        context.AddFailure(context.PropertyName,
            $"File extension must be one of follow: {string.Join(",", _extensions)}.");

        return false;
    }

    private static void ValidateInputExtensions(IEnumerable<string> extensions)
    {
        var incorrectExtensions = extensions
            .Where(x => !x.StartsWith('.'))
            .ToArray();

        if (incorrectExtensions.Length == 0) return;

        var incorrectExtensionsString = string.Join(';', incorrectExtensions);

        throw new InvalidOperationException(
            $"Incorrect extensions: {incorrectExtensionsString}. Example of correct extension: \".pdf\"");
    }
}
