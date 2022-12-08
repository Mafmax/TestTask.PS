using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Mafmax.FileConverter.Utils.Exceptions;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;

namespace Mafmax.FileConverter.Utils.Helpers;

[StackTraceHidden]
public class ThrowHelper : IThrowHelper
{
    /// <inheritdoc />
    [DoesNotReturn]
    public void FileNotFound<TId>(TId fileId) =>
        throw new AppException($"File with Id {fileId} not found.");
}