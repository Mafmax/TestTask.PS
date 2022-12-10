namespace Mafmax.FileConverter.Utils.Exceptions;

/// <summary>
/// Represents an application error.
/// Used for errors with user frendly message.
/// </summary>
public class AppException : Exception
{
    /// <summary>
    /// Creates an instance of an exception.
    /// </summary>
    /// <param name="message">Text of error.</param>
    public AppException(string? message) : base(message)
    {
    }
}
