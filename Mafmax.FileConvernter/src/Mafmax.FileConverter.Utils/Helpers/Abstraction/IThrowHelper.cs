using System.Diagnostics.CodeAnalysis;

namespace Mafmax.FileConverter.Utils.Helpers.Abstraction;

/// <summary>
/// Defines methods for working throwing exceptions.
/// </summary>
public interface IThrowHelper
{
    /// <summary>
    /// Throws an exception saying that file with provided id not found.
    /// </summary>
    /// <param name="fileId">File id passed to exception message.</param>
    /// <typeparam name="TId">Id type.</typeparam>
    [DoesNotReturn]
    void FileNotFound<TId>(TId fileId);
}
