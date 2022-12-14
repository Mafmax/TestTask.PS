using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Mafmax.FileConvernter.Api.Extensions;

/// <summary>
/// Contains extensions for <see cref="IValidator{T}"/>.
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// Validates model and gets error if model is invalid.
    /// </summary>
    /// <param name="validator">Validator for model.</param>
    /// <param name="model">Model for validate.</param>
    /// <param name="error">Validation error.</param>
    /// <typeparam name="T">Model type.</typeparam>
    /// <returns><see langword="true"/> if model is invalid. Otherwise <see langword="false"/>.</returns>
    public static bool ModelIsInvalid<T>(this IValidator<T> validator,
        T model,
        [NotNullWhen(returnValue: true)] out BadRequestObjectResult? error)
    {
        var validationResult = validator.Validate(model);

        if (validationResult.IsValid)
        {
            error = null;
            return false;
        }

        error = new BadRequestObjectResult(new
        {
            Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .Distinct()
        });

        return true;
    }

}
