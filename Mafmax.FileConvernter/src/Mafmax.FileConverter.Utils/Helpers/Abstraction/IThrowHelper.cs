using System.Diagnostics.CodeAnalysis;

namespace Mafmax.FileConverter.Utils.Helpers.Abstraction;

public interface IThrowHelper
{
    [DoesNotReturn]
    void FileNotFound<TId>(TId fileId);
}
